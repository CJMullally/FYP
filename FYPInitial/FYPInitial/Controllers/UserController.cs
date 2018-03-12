using FYPInitial.CustomFilters;
using FYPInitial.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

//Controller to manage users based on role and serve personalised homepages
namespace FYPInitial.Controllers
{
    [RequireHttps]
    [AuthLog(Roles = "Customer, Admin, Employee")]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            //Passes values to populate customer landing page
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());


            using (DBModels dBModel = new DBModels())
            {
                //Retrieve customer's next appointment time (if exists)
                var nextAppointment = dBModel.userevents.Where(x => x.UserID == currentUser.Id).FirstOrDefault();

                if (nextAppointment != null)
                {
                    var appointment = nextAppointment.Start.ToShortDateString();
                    ViewData["nextAppointment"] = appointment;
                }
                else
                {
                    ViewData["nextAppointment"] = "No upcoming appointments.";
                }


                //Retrieve customer's last appointment (if exists)
                var lastAppointment = dBModel.servicehistories.Where(x => x.CustomerID == currentUser.Id).FirstOrDefault();

                if (lastAppointment != null)
                {
                    var appointment = lastAppointment.Start.Value;
                    var daysAgo = DateTime.Now.Subtract(appointment).Days.ToString();
                    ViewData["daysAgo"] = daysAgo + " days ago";
                }
                else
                {
                    ViewData["daysAgo"] = "No appointment history.";
                }

                //Retrieve customer's service reminders
                var serviceList = dBModel.servicehistories.Where(x => x.CustomerID == currentUser.Id).ToList();

                if (serviceList != null)
                {

                    foreach (var service in serviceList)
                    {
                        var type = service.ServiceType;
                        int serviceDays = 0;
                        switch (type)
                        {
                            case "Sound Testing":
                                break;
                            case "Airtightness Testing":
                                break;
                            case "Solar Electricity":
                                serviceDays = 365;
                                break;
                            case "Solar Hot Water":
                                serviceDays = 548;
                                break;
                            case "Heat Recovery and Ventilation":
                                break;
                            case "Heat Pump":
                                serviceDays = 730;
                                break;
                            default:
                                serviceDays = 0;
                                break;

                        }

                        var serviceDate = service.Start.Value.AddDays(serviceDays);
                        var daysLeft = serviceDate.Subtract(System.DateTime.Now).Days.ToString();

                        ViewData["equipment"] = type.ToString();
                        ViewData["serviceDate"] = serviceDate.ToShortDateString();
                        ViewData["daysLeft"] = daysLeft;

                    }
                }
                else
                {
                    ViewData["serviceDate"] = null;
                }

            }
            return View(currentUser);
            
        }


        public ActionResult EmployeeIndex()
        {
            //Passes values to populate employee landing page
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

            var EmployeeName = currentUser.FullName;
            var EmployeeID = User.Identity.GetUserId();

            ViewBag.Message = EmployeeName;

            
            using (DBModels dBModel = new DBModels())
            {


                var events = dBModel.userevents.Where(x => x.EmployeeID == EmployeeID).ToList();
                
                //Loop to determine if the employee has any appointments today
                foreach (var myevent in events)
                {
                    long starttime = long.Parse(myevent.Start.ToString("yyyyMMdd"));
                    long systime = long.Parse(DateTime.Now.ToString("yyyyMMdd"));
                   
                    if (starttime == systime)
                    {
                        ViewData["todaysappts"] = +1;
                    }
                    else {
                        ViewData["todaysappts"] = 0;
                    }
                }

                //Value for amount of appointments this week for current employee
                DateTime weekFromNow = DateTime.Today.AddDays(+7);
                var count = dBModel.userevents.Where(x => x.Start > DateTime.Now && x.Start < weekFromNow && x.EmployeeID == EmployeeID).Count();
                ViewData["count"] = count;
            }
         
            return View();
        }


        public Boolean IsAdminUser()
        {
            //Boolean to determine if the user has the admin role
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public string PopulateDonut()
        {
            //Populate donut chart with appointment data
            string UserID = User.Identity.GetUserId();

                using (DBModels dBModel = new DBModels())
                {

                    //Retrieve appointments associated with current employee
                    var empappointments = dBModel.userevents.Where(x => x.EmployeeID == UserID).ToList();

                    //Retrieve appointments from between today and a week from now
                    DateTime weekFromNow = DateTime.Today.AddDays(+7);
                    var appointments = empappointments.Where(x => x.Start > DateTime.Now && x.Start < weekFromNow).ToList();

                    //Examine the appointment data set and return list of days
                    var days = new List<string>();

                    foreach (var appointment in appointments)
                    {
                        var day = appointment.Start.DayOfWeek.ToString();
                        days.Add(day);
                    }

                    //Group the appointments by day and count their value
                    var count = days.GroupBy(i => i);

                    List<GraphData> dataList = new List<GraphData>();

                    foreach (var group in count)
                    {
                        GraphData details = new GraphData();
                        details.label = group.Key;
                        details.value = group.Count().ToString();
                        dataList.Add(details);
                    }

                    //Convert data to JSON format
                    var jsonSerializer = new JavaScriptSerializer();
                    string data = jsonSerializer.Serialize(dataList);
                    return data;                
                    


                }

        }

      



        private class GraphData
        {
            public string label { get; set; }
            public string value { get; set; }
        }





    }

}
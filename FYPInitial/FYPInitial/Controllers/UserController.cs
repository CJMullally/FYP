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
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
           
                return View(currentUser);
        }


        public ActionResult EmployeeIndex()
        {
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
            string EmployeeID = User.Identity.GetUserId();
            using (DBModels dBModel = new DBModels())
            {
                //Retrieve appointments associated with current employee
                var empappointments = dBModel.userevents.Where(x => x.EmployeeID == EmployeeID).ToList();

                //Retrieve appointments from between today and a week from now
                DateTime weekFromNow = DateTime.Today.AddDays(+7);
                var appointments = empappointments.Where(x => x.Start > DateTime.Now && x.Start < weekFromNow).ToList();

                //Examine the appointment data set and return list of days
                var days = new List<string>();

                foreach (var appointment in appointments) {
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
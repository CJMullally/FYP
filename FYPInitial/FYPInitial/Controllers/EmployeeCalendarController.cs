using FYPInitial.CustomFilters;
using FYPInitial.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FYPInitial.Controllers
{
    //Calendar which allows employees to view, create and delete appointments.
    [AuthLog(Roles = "Admin, Employee")]
    public class EmployeeCalendarController : Controller
    {
        // GET: EmployeeCalendar
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            //Populate the calendar
            using (Models.DBModels dbModel = new DBModels())
            {
                var appointments = dbModel.userevents.ToList();
                return new JsonResult { Data = appointments, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        public JsonResult DeleteAppointment(int EventID)
        {
            //Delete selected event on the calendar
            var status = false;
            using (Models.DBModels dbModel = new DBModels())
            {
                var v = dbModel.userevents.Where(a => a.EventID == EventID).FirstOrDefault();
                if (v != null)
                {
                    //Delete Event
                    dbModel.userevents.Remove(v);
                    dbModel.SaveChanges();
                    status = true;
                }
            }

            return new JsonResult { Data = new { status = status } };

        }

        public JsonResult CompleteAppointment(int EventID)
        {
            //Change the appointments completion status to true
            var status = false;
            using (Models.DBModels dbModel = new DBModels())
            {
                var v = dbModel.userevents.Where(a => a.EventID == EventID).FirstOrDefault();
                var w = dbModel.servicehistories.Create();

                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

                string empID = currentUser.Id;
                string empName = currentUser.FullName;


                if (v != null)
                {
                    //Add completed appointment details to service history table

                    w.AppointmentID = v.AppointmentID;
                    w.EmployeeID = empID;
                    w.EmployeeName = empName;
                    w.CustomerID = v.UserID;
                    w.Address = v.Address;
                    w.Eircode = v.Eircode;
                    w.Start = v.Start;
                    w.End = v.End;
                    w.Description = "";

                 
                    dbModel.servicehistories.Add(w);
                    //Remove completed appointment from appointments table
                    dbModel.userevents.Remove(v);

                }
                dbModel.SaveChanges();
                status = true;
            }

            return new JsonResult { Data = new { status = status } };

        }
    }
}
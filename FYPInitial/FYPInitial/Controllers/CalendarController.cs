using FYPInitial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYPInitial.CustomFilters;
using System.Globalization;
using Microsoft.AspNet.Identity;

// Tutorial Used: https://youtu.be/Jt9vSY802mM

//Controller for Calendar functionality

namespace FYPInitial.Controllers
{
    [AuthLog(Roles = "Customer, Admin, Employee")]
    public class CalendarController : Controller
    {
        // GET: Calendar
        
        public ActionResult Index()
        {
            return View();
        }

        
        public JsonResult GetEvents()
        {
            //Populate the calendar
            
            using (Models.DBModels dbModel = new DBModels()) {

                var events = dbModel.events.ToList();

                //If event date is in the past change event colour to orange
                foreach (var myevent in events)
                {
                    long systime = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    long starttime = long.Parse(myevent.Start.ToString("yyyyMMddHHmmss"));

                    if (systime > starttime)
                    {
                        myevent.ThemeColor = "orange";
                    }

                    dbModel.SaveChanges();
                }

                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [AuthLog(Roles = "Admin, Employee")]
        [HttpPost]
        public JsonResult SaveEvent(@event e)
        {

            //TO DO: Add logic to prevent employee creating multiple appointments on one time slot

            var EmployeeID = User.Identity.GetUserId();
            //Save a calendar event
            var status = false;
            using (Models.DBModels dbModel = new DBModels()) {
                    if (e.EventID > 0)
                {
                    //Update Event
                    var v = dbModel.events.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.ThemeColor = e.ThemeColor;
                        v.EmployeeID = EmployeeID;
                    }
                }
                else
                {
                    e.EmployeeID = EmployeeID;
                    dbModel.events.Add(e);
                }

                dbModel.SaveChanges();
                status = true;
            }
                return new JsonResult { Data = new { status = status } };
        }

        [AuthLog(Roles = "Admin, Employee")]
        [HttpPost]
        public JsonResult DeleteEvent(int EventID)
        {
            //Delete selected event on the calendar
            var status = false;
            using (Models.DBModels dbModel = new DBModels())
            {
                var v = dbModel.events.Where(a => a.EventID == EventID).FirstOrDefault();
                if (v != null) {
                    //Delete Event
                    dbModel.events.Remove(v);
                    dbModel.SaveChanges();
                    status = true;
                }
            }

            return new JsonResult { Data = new { status = status } };
              
        }

    }
}
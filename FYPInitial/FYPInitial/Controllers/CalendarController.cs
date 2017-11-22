using FYPInitial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYPInitial.CustomFilters;

// Tutorial Used: https://youtu.be/Jt9vSY802mM

//Controller for Calendar functionality

namespace FYPInitial.Controllers
{
    [AuthLog(Roles = "Admin")]
    public class CalendarController : Controller
    {
        // GET: Calendar
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public JsonResult GetEvents()
        {
            //Populate the calendar
            using (Models.DBModels dbModel = new DBModels()) {
                var events = dbModel.events.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(@event e)
        {
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
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColour = e.ThemeColour;
                    }
                }
                else
                {
                    dbModel.events.Add(e);
                }

                dbModel.SaveChanges();
                status = true;
            }
                return new JsonResult { Data = new { status = status } };
        }

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
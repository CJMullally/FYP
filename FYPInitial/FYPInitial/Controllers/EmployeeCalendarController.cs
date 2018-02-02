using FYPInitial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FYPInitial.Controllers
{
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
    }
}
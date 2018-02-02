using FYPInitial.CustomFilters;
using FYPInitial.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace FYPInitial.Controllers
{
    public class CustomerCalendarController : Controller
    {
        // GET: CustomerCalendar
        public ActionResult Index()
        {
            return View();
        }

        [AuthLog(Roles = "Customer")]
        [HttpPost]
        public JsonResult BookAppointment(int EventID)
        {
            //Assigns selected available appointment to customer
            var status = false;
            using (Models.DBModels dbModel = new DBModels())
            {
                var v = dbModel.events.Where(a => a.EventID == EventID).FirstOrDefault();
                var w = dbModel.userevents.Create();
                string currentUserId = User.Identity.GetUserId();
                string currentUserName = User.Identity.GetUserName();

                //Check if an appointment is already associated with this user
                var usereventmodel = dbModel.userevents.Where(x => x.UserID == currentUserId).FirstOrDefault();

                if (usereventmodel == null)
                {
                    if (v != null)
                    {

                        //Add event to customer diary
                        w.EventID = v.EventID;
                        w.Subject = v.Subject;
                        w.Start = v.Start;
                        w.UserID = currentUserId;
                        w.UserName = currentUserName;
                        v.ThemeColor = "red";

                        dbModel.userevents.Add(w);

                    }
                    dbModel.SaveChanges();
                    status = true;

                }

                return new JsonResult { Data = new { status = status } };
            }
        }

    }
}
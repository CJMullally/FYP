using FYPInitial.CustomFilters;
using FYPInitial.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web.Security;


namespace FYPInitial.Controllers
{
    //Calendar which allows users with customer role to book appointments
    [AuthLog(Roles = "Customer, Admin, Employee")]
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

                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

                string UserId = currentUser.Id;
                string UserName = currentUser.UserName;
                string FullName = currentUser.FullName;
                string Address = currentUser.Address;
                string PhoneNum = currentUser.PhoneNo;
                string Eircode = currentUser.Eircode;


                //Check if an appointment is already associated with this user
                var usereventmodel = dbModel.userevents.Where(x => x.UserID == UserId).FirstOrDefault();

                if (usereventmodel == null)
                {
                    if (v != null)
                    {

                        //Add event to customer diary
                        w.EventID = v.EventID;
                        w.Subject = v.Subject;
                        w.Start = v.Start;
                        w.End = v.End;
                        w.UserID = UserId;
                        w.UserName = UserName;
                        w.FullName = FullName;
                        w.Address = Address;
                        w.Eircode = Eircode;
                        w.PhoneNumber = PhoneNum;
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
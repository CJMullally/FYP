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
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Collections;
using System.Web.Script.Serialization;

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

        public JsonResult ExistingCustomer()
        {
            //Checks if the user has any current or previous appointments
            //If no appointment history found JQuery will display a one-time Welcome popup with help
            var result = false;
            var UserID = User.Identity.GetUserId();
   
            using (Models.DBModels dbModel = new DBModels())
            {

                var activeAppointments = dbModel.userevents.Where(x => x.UserID == UserID).FirstOrDefault();
                var previousAppointments = dbModel.servicehistories.Where(x => x.CustomerID == UserID).FirstOrDefault();

                if (previousAppointments != null && activeAppointments != null)
                {

                    result = true;
                }
                else
                {
                    result = false;
                }

                
            }

            return new JsonResult { Data = new { result = result } };

        }

      
        [AuthLog(Roles = "Customer")]
        [HttpPost]
        public JsonResult BookAppointment(@userevent e)
        {
            //Assigns selected available appointment to customer
            var status = false;
            using (Models.DBModels dbModel = new DBModels())
            {
                var v = dbModel.events.Where(a => a.EventID == e.EventID).FirstOrDefault();
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
                        w.EmployeeID = v.EmployeeID;
                        w.Subject = v.Subject;
                        w.Start = v.Start;
                        w.End = v.End;
                        w.ServiceType = e.ServiceType;
                        w.UserID = UserId;
                        w.UserName = UserName;
                        w.FullName = FullName;
                        w.Address = Address;
                        w.Eircode = Eircode;
                        w.PhoneNumber = PhoneNum;
                        w.Message = e.Message;
                        v.ThemeColor = "red";

                        dbModel.userevents.Add(w);

                    }

                    dbModel.SaveChanges();
                    status = true;

                }

                return new JsonResult { Data = new { status = status } };
            }
        }




        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public void FileUpload(HttpPostedFileBase uploadFile)
        //{
        //    ApplicationDbContext context = new ApplicationDbContext();
        //    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
        //    ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

        //    if (uploadFile != null && uploadFile.ContentLength > 0)
        //            {
        //        var attachment = new FilePath
        //        {
        //            FileName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(uploadFile.FileName),
        //            FileType = FileType.Attachment

        //        };

        //        currentUser.FilePaths = new List<FilePath>();
        //        currentUser.FilePaths.Add(attachment);
        //        uploadFile.SaveAs(Path.Combine(Server.MapPath("~/App_Data/Uploads"), attachment.FileName));
        //    }
        //}

        private class ActionInfo
        {
            public ActionInfo()
            {
            }

            public bool Success { get; set; }
        }

    }


}
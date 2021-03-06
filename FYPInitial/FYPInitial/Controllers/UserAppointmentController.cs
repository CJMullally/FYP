﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using FYPInitial.Models;
using FYPInitial.CustomFilters;

namespace FYPInitial.Controllers
{

    [RequireHttps]
    [AuthLog(Roles = "Customer, Admin, Employee")]
    public class UserAppointmentController : Controller
    {
        // GET: UserAppointment
        public ActionResult Index()
        {
            userevent usereventModel = new userevent();
            string currentUserId = User.Identity.GetUserId();
            using (DBModels dBModel = new DBModels())
            {
                usereventModel = dBModel.userevents.Where(x => x.UserID == currentUserId).FirstOrDefault();

                //Retrieve last appointment associated with current user
                var appointment = dBModel.servicehistories.Where(x => x.CustomerID == currentUserId).FirstOrDefault();

                if (appointment != null)
                {
                    var lastAppointment = appointment.Start.Value;
                    var daysAgo = DateTime.Now.Subtract(lastAppointment).Days.ToString();
                    ViewData["daysAgo"] = daysAgo + " days ago";
                }
                else
                {
                    ViewData["daysAgo"] = "No appointment history.";
                }
            }
                return View(usereventModel);


        }

        // GET: UserAppointment/Delete
        // Returns a view confirming product deletion
        public ActionResult Delete()
        {
            userevent usereventModel = new userevent();
            string userid = User.Identity.GetUserId();
            using (DBModels dBModel = new DBModels())
            {
                usereventModel = dBModel.userevents.Where(x => x.UserID == userid).FirstOrDefault();
            }

            return View(usereventModel);
        }

        // POST: UserAppointment/Delete
        // Posts confirmation of appointment deletion to the server and deletes the booking
        [HttpPost]
        public ActionResult Delete(FormCollection collection)
        {
            string userid = User.Identity.GetUserId();

            using (DBModels dBModel = new DBModels())
            {
                userevent usereventModel = dBModel.userevents.Where(x => x.UserID == userid).FirstOrDefault();
                if (usereventModel != null)
                {
                    var v = dBModel.events.Where(a => a.EventID == usereventModel.EventID).FirstOrDefault();
                    v.ThemeColor = "";
                    dBModel.userevents.Remove(usereventModel);
                    dBModel.SaveChanges();
                }

            }
            return RedirectToAction("Index");
        }

        public ActionResult AppointmentHistory()
        {
            string userid = User.Identity.GetUserId();
            using (DBModels dBModel = new DBModels())
            {

                var servicehistory = from s in dBModel.servicehistories
                                     where s.CustomerID == userid
                               select s;

                return View(servicehistory.ToList());
            }


        }
    }
}

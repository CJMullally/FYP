using FYPInitial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FYPInitial.Controllers
{
    //Visitor view of calendar, may only view calendar - no CRUD or interactivity
    public class BookingsController : Controller
    {
        // GET: Bookings
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetEvents()
        {
            //Populate the calendar
            using (Models.DBModels dbModel = new DBModels())
            {
                var events = dbModel.events.ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
    }
}
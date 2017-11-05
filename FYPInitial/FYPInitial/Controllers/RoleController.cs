using System;
using System.Linq;
using System.Web.Mvc;
using FYPInitial.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using FYPInitial.CustomFilters;

namespace FYPInitial.Controllers
{
    // This controller manages the Role functionality 
    // This code was obtained from http://www.dotnetcurry.com/aspnet-mvc/1102/aspnet-mvc-role-based-security
    [AuthLog(Roles = "Admin")]
    public class RoleController : Controller
    {
        ApplicationDbContext context;

        public RoleController()
        {
            context = new ApplicationDbContext();
        }



        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var Roles = context.Roles.ToList();
            return View(Roles);
        }

        /// <summary>
        /// Create  a New role
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var Role = new IdentityRole();
            return View(Role);
        }

        /// <summary>
        /// Create a New Role
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            context.Roles.Add(Role);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYPInitial.Models;

namespace FYPInitial.ViewModels
{
    public class DashViewModel
    {
            public DBModels DBModel { get; set; }
            public ApplicationUser  ApplicationUser { get; set; }
        
    }
}

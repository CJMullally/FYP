using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using WebGrease.Activities;

namespace FYPInitial.Models
{

    public class FilePath
    {
        public int FilePathId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        public FileType FileType { get; set; }
        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
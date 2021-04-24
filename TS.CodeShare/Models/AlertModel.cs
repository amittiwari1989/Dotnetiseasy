using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TS.CodeShare.Models
{
    public class AlertModel
    {
        [Display(Name = "Subject")]
        public int AlertType { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

    }
}
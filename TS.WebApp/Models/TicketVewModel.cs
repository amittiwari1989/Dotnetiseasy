using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TS.Enums;
using TS.Tickets;

namespace TS.WebApp.Models
{
    public class TicketVewModel
    {

        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        public TicketPriority Priority { get; set; }

        [Display(Name = "Priority")]
        public int priority_type { get; set; }


        [Display(Name = "Type")]
        public List<SelectListItem> Type { get; set; }
        [Display(Name = "Type")]
        public string ticket_type { get; set; }


        [Display(Name = "Status")]
        public List<SelectListItem> Status { get; set; }
        public string StatusName { get; set; }


        public string custom_field_1 { get; set; }
        public string custom_field_2 { get; set; }
        public string custom_field_3 { get; set; }
        public string custom_field_4 { get; set; }
        public string custom_field_5 { get; set; }
        public string custom_field_6 { get; set; }
        public string custom_field_7 { get; set; }
        public string custom_field_8 { get; set; }
        public string custom_field_9 { get; set; }
        public string custom_field_10 { get; set; }
        public string custom_field_11 { get; set; }
        public string custom_field_12 { get; set; }
        public string custom_field_13 { get; set; }
        public string custom_field_14 { get; set; }
        public string custom_field_15 { get; set; }
        public string custom_field_16 { get; set; }
        public string custom_field_17 { get; set; }
        public string custom_field_18 { get; set; }
        public string custom_field_19 { get; set; }
        public string custom_field_20 { get; set; }

        public List<CustomFieldMaster> CustomField { get; set; }


    }

    public class TicketStatusModel
    {

        [Required]
        [Display(Name = "StatusName")]
        public string StatusName { get; set; }
        


    }

}
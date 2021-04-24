using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TS.ManageContents;

namespace TS.CodeShare.Models
{
   
    public class QuestionTestModel
    {
        public long Qid { get; set; }

        [AllowHtml]
        public string Question { get; set; }

        [Display(Name = "Question Group")]
        public int QuestionGroup { get; set; }
        public string QLanguage { get; set; }
        public string QuestionType { get; set; }
        public string CurrectAnswer { get; set; }
        [Required]
        public string Answer { get; set; }
        public string AnswerDetails { get; set; }
        // Additional 
        public List<QuestionOptions> Options { get; set; }
        public string QuestionGroupName { get; set; }


        public List<QuestionMaster> Questions { get; set; }
        public string Name { get; set; }
    }

    public class AddQuestionModel
    {
        public int QgrupId { get; set; }
        public string Qgrup { get; set; }
        [Required]
        [AllowHtml]
        public string Question { get; set; }
        [Required]
        public List<string> DynamicTextBox { get; set; }
        //public List<QuestionOptions> Options { get; set; }
        [Required]
        public string CurrectAnswer { get; set; }
        public string AnswerDescription { get; set; }


    }
}
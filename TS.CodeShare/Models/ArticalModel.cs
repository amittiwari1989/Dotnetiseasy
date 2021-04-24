using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TS.CodeShare.Models
{
    public class ArticalModel
    {
        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Serrch Tag")]
        public string SerachTag{ get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }


        [Display(Name = "Description1")]
        public string Description1 { get; set; }


        [Display(Name = "ArticalBody")]
        public string ArticalBody { get; set; }

        [Display(Name = "Comment")]
        public string Comment { get; set; }

        public List<ArticalCommentModel> Comments { get; set; }

        public string FileName { get; set; }
        public string Content { get; set; }
    }
    public class PostArticalModel
    {
        public int topicId { get; set; }

        public int userId { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string topic_details { get; set; }

        [Display(Name = "Search Tags")]
        public string search_tags { get; set; }

       
        [AllowHtml]
        [Display(Name = "Content")]
        public string Content
        {
            get; set;
        }

        [Display(Name = "File Name")]
        public string FileName { get; set; }

        [Display(Name = "URL")]
        public string URL{ get; set; }

        //public string code_topicscol { get; set; }
        [Required]
        public string Language { get; set; }
        

    }

    public class ArticalCommentModel
    {

        public long topic_id { get; set; }
        public long? commentId { get; set; }
        public long userId { get; set; }
        [Required]
        public string Comments { get; set; }
    }

}
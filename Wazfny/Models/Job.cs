using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Wazfny.Models
{
    public class Job
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="إسم الوظيفة")]
        public string JobTitle { get; set; }
        [Required]
        [Display(Name = "وصف الوظيفة")]
        [AllowHtml]
        public string JobContent { get; set; }
        [Display(Name = "صورة الوظيفة")]
        public string JobImage { get; set; }
        [Required]
        [Display(Name = "نوع الوظيفة")]
        public int CategoryId { get; set; }

        public string UserId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}
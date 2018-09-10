using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wazfny.Models
{
    public class ApplyForJob
    {
        public int Id { get; set; }
        [Display(Name ="رسالة إلى الناشر")]
        public string Message { get; set; }
        [Display(Name = "تاريخ التقدم إلى الوظيفة")]
        public DateTime ApplyDate { get; set; }
        public int JobId { get; set; }
        public string UserId { get; set; }
        public virtual Job job { get; set; }
        public virtual ApplicationUser user { get; set; }

    }
}
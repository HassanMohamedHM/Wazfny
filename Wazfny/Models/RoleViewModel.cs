using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wazfny.Models
{
    public class RoleViewModel
    {

        public string Id { get; set; }
        [Required]
        [Display(Name="إسم الدور")]
        public string Name { get; set; }
    }
}
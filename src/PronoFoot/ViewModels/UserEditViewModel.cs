using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PronoFoot.ViewModels
{
    public class UserEditViewModel
    {
        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Adresse email")]
        public string Email { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class AddMenuViewModel
    {
        [Required(ErrorMessage = "You must have a menu name")]
        [Display(Name = "Menu Name")]
        public string Name { get; set; }
    }
}

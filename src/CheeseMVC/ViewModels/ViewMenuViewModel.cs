
using CheeseMVC.Models;
using System.Collections.Generic;

namespace CheeseMVC.ViewModels
{
    public class ViewMenuViewModel
    {
        public IList<CheeseMenu> Items { get; set; }
        public Menu Menu { get; set; }
    }
}

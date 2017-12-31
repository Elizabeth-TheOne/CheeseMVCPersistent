using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using CheeseMVC.Data;
using Microsoft.EntityFrameworkCore;


namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private CheeseDbContext context;
        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            List < Menu > menus = context.Menus.ToList();
            return View(menus);
        }
        [HttpGet]
        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View(addMenuViewModel);
        }
        //TODO change the following to add menus instead of cheeses        
        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu()
                // Add the new cheese to my existing cheeses
                {
                    Name = addMenuViewModel.Name
                };
                context.Menus.Add(newMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }
            return View(addMenuViewModel);
        }

        [HttpGet]
        public IActionResult ViewMenu(int id)
        {
            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Where(cm => cm.MenuID == id)
                .ToList();
            Menu theMenu = context.Menus.Single(c => c.ID == id);

            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel()
            {
                Items = items,
                Menu = theMenu
            };

            return View(viewMenuViewModel);
        }

        [HttpGet]
        public IActionResult AddItem(int id)
        {
            Menu theMenu = context.Menus.Single(m => m.ID == id);
            List<Cheese> cheeses = context.Cheeses.ToList();

            return View(new AddMenuItemViewModel(theMenu, cheeses));
        }

        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {
                IList<CheeseMenu> existingItems = context.CheeseMenus
                    .Where(cm => cm.CheeseID == addMenuItemViewModel.CheeseID)
                    .Where(cm => cm.MenuID == addMenuItemViewModel.MenuID).ToList();

                if (existingItems.Count == 0)
                {
                    CheeseMenu newCheeseMenu = new CheeseMenu();
                    newCheeseMenu.MenuID = addMenuItemViewModel.MenuID;
                    newCheeseMenu.CheeseID = addMenuItemViewModel.CheeseID;

                    context.CheeseMenus.Add(newCheeseMenu);
                    context.SaveChanges();

                    return Redirect(string.Format("/Menu/ViewMenu/{0}", newCheeseMenu.MenuID));

                }

                return Redirect("/Menu");

            }
            return View(addMenuItemViewModel);

        }
    }
}
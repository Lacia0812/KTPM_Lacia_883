using Microsoft.AspNetCore.Mvc;

namespace Lab1_THKTPM.Navigation
{
    [ViewComponent(Name = "Lab1_THKTPM.Navigation.LeftNavigation")]
    public class LeftNavigationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(NavigationMenu menu)
        {
            menu.MenuItems = menu.MenuItems.OrderBy(p => p.Sequence).ToList();
            return View(menu);
        }
    }
}
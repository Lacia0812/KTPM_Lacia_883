using ASC.Web.Controllers;
using Lab1_THKTPM.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Lab1_THKTPM.Areas.ServiceRequests.Controllers
{
    [Area("ServiceRequests")] // Xác định Area
    public class DashboardController : BaseController
    {
        private readonly IOptions<ApplicationSettings> _settings;

        public DashboardController(IOptions<ApplicationSettings> settings)
        {
            _settings = settings;
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}

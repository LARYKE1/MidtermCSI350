using Microsoft.AspNetCore.Mvc;
using MidtermCSI350.Data;
using System.Linq;

namespace MidtermCSI350.Controllers
{
    public class SettingsController : Controller
    {

        private readonly MyDbContext _context;

        public SettingsController(MyDbContext context)
        {
            _context = context;
        }

        [Route("~/")]
        public IActionResult Index()
        {
            int managerCount = _context.Manager.Count();
            int employeesCount= _context.Employees.Count();

            TempData["ManagerCount"] = managerCount;
            TempData["EmployeeCount"] = employeesCount;


            return View();
        }
        [Route("MyBrand[action]")]

        public IActionResult ContactUs()
        {
            return View("~/Views/Settings/Privacy.cshtml");
        }
    }
}

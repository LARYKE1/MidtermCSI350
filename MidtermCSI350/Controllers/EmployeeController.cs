using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MidtermCSI350.Data;
using MidtermCSI350.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MidtermCSI350.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly MyDbContext _context;

        public EmployeeController(MyDbContext context)
        {
            _context = context;
        }

        //NORMAL
        [HttpGet]
        [Route("Employees")]
        public IActionResult GetAll()
        {
            var employees= _context.Employees.Include(e=>e.Manager).ToList();
            return PartialView("EmployeesPartialView", employees);
        }
        //NORMAL
        [HttpGet("{id}")]
        [Route("Employees/{id}")]
        [Route("[controller]/[action]/{id?}")]
        public IActionResult GetEmployee(int id)
        {
            var employee = _context.Employees.Include(r => r.Manager).FirstOrDefault(item => item.Id == id);

            return PartialView("SingleEmployee", employee);
        }


        //JSON
        [HttpGet("{id?}/{format?}")]
        [FormatFilter]
        [Produces("application/json", "application/xml")]
        [Route("EmployeesJSON/{id?}/{format?}")]
        [Route("EmployeesJSON/{format?}")]
        [Route("[controller]/[action]/{id?}/{format?}")]
        [Route("[controller]/[action]/{format?}")]
        public IActionResult ShowAllJson(int? id)
        {

            

            if (id == null)
            {
                var allEmployees=_context.Employees.ToList();


                return Ok(allEmployees);
            }
            else
            {
                var employee = _context.Employees.Find(id);
                if (employee == null) 
                {
                    return NotFound(); 
                }

                return Ok(employee);
            }
    
        }


        //SimpleTextFile
        [HttpGet("{id?}")]
        [Route("EmployeesTxt/{id?}")]
        public IActionResult GetTextFile(int? id)
        {
            if (id == null)
            {
                List<Employee> employees = _context.Employees.Include(e => e.Manager).ToList();

                if (employees == null || employees.Count == 0)
                {
                    return NotFound();
                }

                StringBuilder sb = new StringBuilder();

                foreach (var employee in employees)
                {
                    sb.AppendLine($"Id: {employee.Id}");
                    sb.AppendLine($"Name: {employee.EmployeerName}");
                    sb.AppendLine($"Phone Number: {employee.PhoneNumber}");
                    sb.AppendLine($"Manager: {employee.Manager.ManagerName}");
                    sb.AppendLine();
                }

                byte[] fileBytes = Encoding.UTF8.GetBytes(sb.ToString());
                return File(fileBytes, "text/plain", "employees.txt");
            }
            else
            {
                Employee employee = _context.Employees.Include(e => e.Manager)
                                 .FirstOrDefault(item => item.Id == id);
                if (employee == null)
                {
                    return NotFound();
                }

                string textFile = $"Id: {employee.Id}\nName:{employee.EmployeerName}\nPhone Number:{employee.PhoneNumber}\nManager:{employee.Manager.ManagerName}";

                byte[] fileBytes = Encoding.UTF8.GetBytes(textFile);
                return File(fileBytes, "text/plain", "employees.txt");

            }
        }



        //CREATE NEW EMPLOYEE
        [Route("[controller]/[action]")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Employees=new SelectList(_context.Manager,"id", "ManagerName");
            return View();
        }
        [Route("[controller]/[action]")]
        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if(ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();

                return RedirectToAction("GetAll");
            }
            else
            {
                return RedirectToPage("~/Views/Settings/Index.cshtml");
            }
        }
    }
}

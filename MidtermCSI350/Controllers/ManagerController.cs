using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MidtermCSI350.Data;
using MidtermCSI350.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MidtermCSI350.Infrastructure;

namespace MidtermCSI350.Controllers
{
    public class ManagerController : Controller
    {
        private readonly MyDbContext _context;

        public ManagerController(MyDbContext context)
        {
            _context = context;
        }

        //HTML form
        [HttpGet("{id?}")]
        [Route("[controller]/[action]/{id?}")]
        [Route("ManagerFromHtml/{id?}")]
        public IActionResult ManagerFromHtml(int? id)
        {

            if (id == null)
            {
                var manager= _context.Manager.ToList();

                return View("Index", manager);
            }
            else
            {

                return View("SingleManager", _context.Manager.Find(id));
            }
   
        }


        //All Managers, format JSON OR XML
        [HttpGet("{format?}")]
        [FormatFilter]
        [Produces("application/json", "application/xml")]
        [Route("ManagerJSON/{format?}")]
        public IEnumerable<Manager> ManagerJson()
        {
            var manager=_context.Manager.ToList();

            return manager;
        }

        //GetIdManager, format JSON or XML
        [HttpGet("{id}/{format?}")]
        [FormatFilter]
        [Produces("application/json", "application/xml")]
        [Route("ManagerWithId/{id}/{format?}")]
        public Manager GetIdManager (int id)
        {
            
                var manager = _context.Manager.FirstOrDefault(item => item.id == id);

                return manager;
            
        }


        //SimpleTextFile
        [HttpGet("{id?}")]
        [Route("ManagerTxt/{id?}")]
        public IActionResult GetTextFile(int? id)
        {
            if (id == null)
            {
                List<Manager> managers = _context.Manager.ToList();

                

                StringBuilder sb = new StringBuilder();

                foreach (var manager in managers)
                {
                    sb.AppendLine($"Id: {manager.id}");
                    sb.AppendLine($"Name: {manager.ManagerName}");
                    sb.AppendLine($"Function: {manager.ManagerRank}");
                    sb.AppendLine();
                }

                byte[] fileBytes = Encoding.UTF8.GetBytes(sb.ToString());
                return File(fileBytes, "text/plain", "managers.txt");
            }
            else
            {
                Manager manager = _context.Manager.FirstOrDefault(item => item.id == id);
                if (manager == null)
                {
                    return NotFound();
                }

                string textFile = $"Id: {manager.id}\nName:{manager.ManagerName}\nFunction:{manager.ManagerRank}\n";

                byte[] fileBytes = Encoding.UTF8.GetBytes(textFile);
                return File(fileBytes, "text/plain", "manager{id}.txt");

            }
        }



        //CREATE NEW MANAGER
        [HttpGet]
        [Route("[controller]/[action]")]
        [Route("CreateNewManager")]
        public IActionResult Create()
        {
            ViewBag.ManagerRankList = Ranks.GetManagerRankList();
            return View();
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        [Route("CreateNewManager")]

        public IActionResult Create(Manager manager) 
        {
            if (ModelState.IsValid)
            {
                _context.Manager.Add(manager);
                _context.SaveChanges();
                return RedirectToAction("ManagerFromHtml");
            }
            else
            {
                return RedirectToAction("Index","Settings");
            }
        }


        //Delete manager, no need for a view
        [HttpDelete("{id}")]
        [Route("[controller]/[action]/{id}")]
        [Route("YoureFired/{id}")]
        public IActionResult Delete(int id)
        {

            var manager = _context.Manager.Find(id);
            
            if(manager == null)
            {
                return NotFound();
            }

            _context.Manager.Remove(manager);
            _context.SaveChanges(true);

            return RedirectToAction("ManagerFromHtml");
        }
    }
}

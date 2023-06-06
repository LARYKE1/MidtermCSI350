using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MidtermCSI350.Models
{
    public class Manager
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(200)]
        public string ManagerName { get; set; }
        public string ManagerRank { get; set;}

        public List<Employee> Employees { get; set;}

    }
}

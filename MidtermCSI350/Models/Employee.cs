using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MidtermCSI350.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        
        public int ManagerId { get; set; }
        [Required]
        public string EmployeerName { get; set; }
        [PersonalData]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public Manager Manager { get; set; }
    }
}

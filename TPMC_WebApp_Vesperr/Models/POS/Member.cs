using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TIPMC_WebApp_Vesperr.Models.POS
{
    public class Member
    {
        public Guid MemberId { get; set; }
        public string EmpNumber { get; set; }
        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Display(Name = "Member Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Designation { get; set; }
        public string CreditLimits { get; set; }
        public string AccessLevel { get; set; }
        public string DateJoined { get; set; }

      
    }
}

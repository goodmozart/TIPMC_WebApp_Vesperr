using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPMC_WebApp_Vesperr.Models.TPMC
{
    public class MemberShare
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid MemberId { get; set; }
        [Required]
        [Display(Name = "Employee Number")]
        public string EmpNumber { get; set; }
        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        [Display(Name = "Full Name")]
        public string Name { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        [Display(Name = "Date Posted")]
        public string DatePosted { get; set; }
        [Display(Name = "Number of Share")]
        public int NumberShare { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        [Display(Name = "Date Update")]
        public string UpdateDate { get; set; }
        public string Status { get; set; }
    }
}

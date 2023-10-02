using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace TPMC_WebApp_Vesperr.Models.TPMC
{
    public class MemberPayment
    {
        [Key]
        public Guid PaymentId { get; set; }
        public string MemberId { get; set; }
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
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        [Display(Name = "Date Paid")]
        public string DatePaid { get; set; }
        [Display(Name = "Principal")]
        public decimal? Principal { get; set; }
        [Display(Name = "Interest")]
        public decimal? Interest { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }

    }
}

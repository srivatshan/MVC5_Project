using MVCProject.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCProject.Models
{
    public class HouseHoldModel
    {
        public HouseHoldMemberModel HouseHoldMemberModel { get; set; }
        public List<HouseHoldMemberModel> MembersList { get; set; }
    }

    public class HouseHoldMemberModel
    {
        public int MemberID { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [MaxLength(32, ErrorMessage = "{0} can have a max of {1} characters")]
        [RegularExpression("^[-'*A-Za-z0-9]*", ErrorMessage = "Speccial char Not allowed")]
        public string FirstName { get; set; }

        public string Ml { get; set; }

        [Required]
        [MaxLength(32, ErrorMessage = "{0} can have a max of {1} characters")]
        [RegularExpression("^[-'*A-Za-z0-9]*", ErrorMessage = "Speccial char Not allowed")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public SuffixList Suffix { get; set; }

        [Display(Name = "Date of Birth (mm/dd/yyyy)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DateRenge]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        public int ApplicationID { get; set; }

    }

    public enum SuffixList
    {
        Mr = 1,
        Miss = 2
    }
}
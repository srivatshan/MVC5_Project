using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.Data.Models
{
    public class MemberDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ApplicationId { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string ML { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Suffix { get; set; }

        [Required]
        public string Dob { get; set; }

        [Required]
        public string Gender { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCProject.Data.Models
{
    public class RelationshipDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ApplicationId { get; set; }

        [Required]
        public int FromMemberId { get; set; }

        [Required]
        public string FromMemberName { get; set; }

        [Required]
        public int Relationship { get; set; }

        [Required]
        public int Tomemberid { get; set; }

        [Required]
        public string ToMemberName { get; set; }
    }



    
    
  


}

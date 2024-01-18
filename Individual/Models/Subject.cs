using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Individual.Models
{
    public class Subject
    {

        [Key, Column(Order = 0)]
        [Required]
        [StringLength(15)]
        [DisplayName("Subject Code")]
        public string? SUBJCODE { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [StringLength(10)]
        [DisplayName("Refer Course Code")]
        public string? SUBJCOURSECODE { get; set; }

        [Key, Column(Order = 2)]
        [Required]
        [StringLength(3)]
        [DisplayName("Requisite Code")]
        public string? SUBJPREQ { get; set; }

        [Key, Column(Order = 3)]
        [Required]
        [StringLength(15)]
        [DisplayName("Refer Subject Code")]
        public string? SUBJCODEPREQ { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Subject Description")]
        public string SUBJDESC { get; set; } = string.Empty;

        [Required]
        [DisplayName("Subject Units")]
        public int SUBJUNITS { get; set; }

        [Required]
        [DisplayName("Subject Offering")]
        public int SUBJREGOFRNG { get; set; }

        [Required]
        [StringLength(3)]
        [DisplayName("Subject Category")]
        public string? SUBJCATEGORY { get; set; }

        [Required]
        [StringLength(2)]
        [DisplayName("Status")]
        public string? SUBJSTATUS { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Curriculum Code")]
        public string? SUBJCURRCODE { get; set; }
    }

}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Individual.Models
{
    public class Student
    {
        [Key, Column(Order = 0)]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Student ID")]
        public long STFSTUDID { get; set; }

        //CHANGES HERE IN STUDENT THE 2 BELOW VARIABLES STFSTUDPICPATH AND STFSTUDPIC
        //SINCE IFORMFILE IS NOT SUPPORTED IN THE DATABASE IT NEEDS TO BE SET AS
        //A NOTMAPPED ANNOTATION AND ONLY PASS THE STRING OR GENERATED PATH OF THE IMAGE
        public string? STFSTUDPICPATH { get; set; }

        [NotMapped]
        public IFormFile? STFSTUDPIC { get; set; }

        [DisplayName("Last Name")]
        [Required]
        [StringLength(15)]
        public string? STFSTUDLNAME { get; set; }

        [DisplayName("First Name")]
        [Required]
        [StringLength(15)]
        public string? STFSTUDFNAME { get; set; }

        [DisplayName("Middle Name")]
        [Required]
        [StringLength(15)]
        public string? STFSTUDMNAME { get; set; }

        [DisplayName("Student Course")]
        [Required]
        [StringLength(10)]
        public string? STFSTUDCOURSE { get; set; }

        [Required]
        [DisplayName("Year")]
        public int STFSTUDYEAR { get; set; }

        [DisplayName("Student Remarks")]
        [Required]
        [StringLength(15)]
        public string? STFSTUDREMARKS { get; set; }

        [Required]
        [StringLength(2)]
        [DisplayName("Status")]
        public string? STFSTUDSTATUS { get; set; }
    }
}

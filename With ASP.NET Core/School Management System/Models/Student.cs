using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace School_Management_System.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        [Required,DisplayName("Student Name")]
        public string StudentName { get; set; } = default!;
        [Required,DisplayName("Date of Birth"),DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MMM-dd}",ApplyFormatInEditMode = true)]
        public DateTime DateofBirth { get; set; }
        [Required]
        public string Class {  get; set; }= default!;
        public string? Image {  get; set; } = default!;
        [Required,DisplayName("IsRegular?")]
        public bool IsRegular { get; set; }
        public virtual ICollection<StudentSubject> StudentSubjects { get; set; }=new List<StudentSubject>();
    }
}

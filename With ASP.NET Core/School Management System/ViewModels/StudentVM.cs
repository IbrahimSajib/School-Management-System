using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace School_Management_System.ViewModels
{
    public class StudentVM
    {
        public StudentVM()
        {
            this.SubjectList = new List<int>();
        }
        public int StudentId { get; set; }
        [Required, DisplayName("Student Name")]
        public string StudentName { get; set; } = default!;
        [Required, DisplayName("Date of Birth"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateofBirth { get; set; }
        [Required]
        public string Class { get; set; } = default!;
        public string? Image { get; set; } = default!;
        [Display(Name = "Image")]
        public IFormFile? ImagePath { get; set; }
        [Required,DisplayName("IsRegular?")]
        public bool IsRegular { get; set; }
        public List<int> SubjectList { get; set; }
    }
}

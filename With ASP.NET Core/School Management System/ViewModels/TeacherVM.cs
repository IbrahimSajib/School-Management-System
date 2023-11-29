using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace School_Management_System.ViewModels
{
    public class TeacherVM
    {
        public TeacherVM()
        {
            this.SubjectList = new List<int>();
        }
        public int TeacherId { get; set; }
        [Required, DisplayName("Teacher Name")]
        public string TeacherName { get; set; } = default!;
        [Required, DisplayName("Join Date"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime JoinDate { get; set; }
        public decimal Salary { get; set; }
        [Required,MaxLength(20)]
        public string Phone { get; set; } = default!;
        public List<int> SubjectList { get; set; }
    }
}

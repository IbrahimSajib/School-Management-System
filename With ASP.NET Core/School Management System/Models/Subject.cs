using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace School_Management_System.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        [Required,DisplayName("Subject Name")]
        public string SubjectName { get; set; } = default!;
        public virtual ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
        public virtual ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();

    }
}

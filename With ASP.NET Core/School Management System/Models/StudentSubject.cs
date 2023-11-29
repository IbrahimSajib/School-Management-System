using System.ComponentModel.DataAnnotations.Schema;

namespace School_Management_System.Models
{
    public class StudentSubject
    {
        public int StudentSubjectId { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        [ForeignKey("Subject")]
        public int SubjectId { get; set; }
        public virtual Student Student { get; set; } = default!;
        public virtual Subject Subject { get; set; } = default!;
    }
}

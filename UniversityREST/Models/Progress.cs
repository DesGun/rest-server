using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityREST.Models
{
    public class Progress
    {
        public int Id { get; set; }
        public int SemesterNumber { get; set; }
        public string Subject { get; set; }
        public int Mark { get; set; }
        public string ExamDate { get; set; }
        public string TeacherFIO { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UniversityREST.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FIO { get; set; }
        public string AdmissionDate { get; set; }
        public string Department { get; set; }
        public string Speciality { get; set; }
        public int Course { get; set; }
        public int GroupNumber { get; set; }
    }
}
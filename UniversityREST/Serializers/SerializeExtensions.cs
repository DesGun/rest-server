using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UniversityREST.Models;
using System.Data;

namespace UniversityREST.Serializers
{
    public static class SerializeExtensions
    {
        public static List<Department> ToDepartmentsList(this DataTable table)
        {
            return table.AsEnumerable().Select(m => new Department()
            {
                Id = m.Field<int>("Id"),
                DepartmentName = m.Field<string>("DepartmentName")
            }).ToList();
        }

        public static List<Progress> ToProgressesList(this DataTable table)
        {
            return table.AsEnumerable().Select(m => new Progress()
            {
                Id = m.Field<int>("Id"),
                SemesterNumber = m.Field<int>("SemesterNumber"),
                Subject = m.Field<string>("Subject"),
                Mark = m.Field<int>("Mark"),
                ExamDate = m.Field<string>("ExamDate"),
                TeacherFIO = m.Field<string>("TeacherFIO")
            }).ToList();
        }

        public static List<Speciality> ToSpecialitiesList(this DataTable table)
        {
            return table.AsEnumerable().Select(m => new Speciality()
            {
                Id = m.Field<int>("Id"),
                SpecialityName = m.Field<string>("SpecialityName")
            }).ToList();
        }

        public static List<Student> ToStudentsList(this DataTable table)
        {
            return table.AsEnumerable().Select(m => new Student()
            {
                Id = m.Field<int>("Id"),
                FIO = m.Field<string>("FIO"),
                AdmissionDate = m.Field<string>("AdmissionDate"),
                Department = m.Field<string>("Department"),
                Speciality = m.Field<string>("Speciality"),
                Course = m.Field<int>("Course"),
                GroupNumber = m.Field<int>("GroupNumber")
            }).ToList();
        }

        public static List<Subject> ToSubjectsList(this DataTable table)
        {
            return table.AsEnumerable().Select(m => new Subject()
            {
                Id = m.Field<int>("Id"),
                SubjectName = m.Field<string>("SubjectName")
            }).ToList();
        }
    }
}
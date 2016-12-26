using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using UniversityREST.Models;
using System.Data;
using Npgsql;
using UniversityREST.Serializers;

namespace UniversityREST.Controllers
{
    public class StudentsController : ApiController
    {
        DataTable table;
        NpgsqlConnectionStringBuilder constr;
        NpgsqlDataAdapter adapter;
        NpgsqlConnection connection;
        NpgsqlCommand command;

        public StudentsController()
        {
            table = new DataTable();
            constr = new NpgsqlConnectionStringBuilder()
            {
                Host = "localhost",
                Port = 4879,
                Database = "UniversityDB",
                Username = "postgres",
                Password = "123456",
                Pooling = true
            };
            connection = new NpgsqlConnection(constr);
            command = new NpgsqlCommand()
            {
                Connection = connection
            };
        }

        // GET: api/students
        public IEnumerable<Student> Get()
        {
            using (adapter = new NpgsqlDataAdapter("SELECT * FROM \"Students\";", constr.ConnectionString))
                adapter.Fill(table);

            return table.ToStudentsList();
        }

        // GET: api/students/id
        public HttpResponseMessage Get(int id)
        {
            using (adapter = new NpgsqlDataAdapter(String.Format("SELECT * FROM \"Students\" WHERE \"Id\"={0};", id), constr.ConnectionString))
                adapter.Fill(table);

            return table.Rows.Count != 0 ? Request.CreateResponse(HttpStatusCode.OK, table.ToStudentsList()) :
                                           Request.CreateResponse(HttpStatusCode.NotFound, "Элемент не найден");
        }

        // POST: api/students
        public HttpResponseMessage Post([FromBody]Student item)
        {
            using (connection)
            {
                connection.Open();
                command.CommandText = String.Format("INSERT INTO \"Students\" VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}');", item.Id, item.FIO, item.AdmissionDate, item.Department, item.Speciality, item.Course, item.GroupNumber);
                command.ExecuteNonQuery();
            }

            HttpResponseMessage msg = Request.CreateResponse(HttpStatusCode.Created, item);
            msg.Headers.Location = new Uri(Request.RequestUri + "/" + item.Id);
            return msg;
        }

        // PUT: api/students/id
        public HttpResponseMessage Put(int id, [FromBody]Student item)
        {
            int rowAffected;
            using (connection)
            {
                connection.Open();
                command.CommandText = String.Format("UPDATE \"Students\" SET \"Id\"={0}, \"FIO\"='{1}', \"AdmissionDate\"='{2}', \"Department\"='{3}', \"Speciality\"='{4}', \"Course\"='{5}', \"GroupNumber\"='{6}' WHERE \"Id\"='{7}';",
                                                    item.Id, item.FIO, item.AdmissionDate, item.Department, item.Speciality, item.Course, item.GroupNumber, id);
                rowAffected = command.ExecuteNonQuery();
            }

            return rowAffected != 1 ? Request.CreateResponse(HttpStatusCode.NotFound) :
                                      Request.CreateResponse(HttpStatusCode.OK, "Элемент " + id + " изменён");
        }

        // DELETE: api/students/id
        public HttpResponseMessage Delete(int id)
        {
            int rowAffected;
            using (connection)
            {
                connection.Open();
                command.CommandText = String.Format("DELETE FROM \"Students\" WHERE \"Id\"='{0}';", id);
                rowAffected = command.ExecuteNonQuery();
            }

            return rowAffected != 1 ? Request.CreateResponse(HttpStatusCode.NotFound) :
                                      Request.CreateResponse(HttpStatusCode.OK, "Элемент " + id + " удалён");
        }
    }
}
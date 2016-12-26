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
    public class ProgressesController : ApiController
    {
        DataTable table;
        NpgsqlConnectionStringBuilder constr;
        NpgsqlDataAdapter adapter;
        NpgsqlConnection connection;
        NpgsqlCommand command;

        public ProgressesController()
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

        // GET: api/progresses
        public IEnumerable<Progress> Get()
        {
            using (adapter = new NpgsqlDataAdapter("SELECT * FROM \"Progresses\";", constr.ConnectionString))
                adapter.Fill(table);

            return table.ToProgressesList();
        }

        // GET: api/progresses/id
        public HttpResponseMessage Get(int id)
        {
            using (adapter = new NpgsqlDataAdapter(String.Format("SELECT * FROM \"Progresses\" WHERE \"Id\"={0};", id), constr.ConnectionString))
                adapter.Fill(table);

            return table.Rows.Count != 0 ? Request.CreateResponse(HttpStatusCode.OK, table.ToProgressesList()) :
                                           Request.CreateResponse(HttpStatusCode.NotFound, "Элемент не найден");
        }

        // POST: api/progresses
        public HttpResponseMessage Post([FromBody]Progress item)
        {
            using (connection)
            {
                connection.Open();
                command.CommandText = String.Format("INSERT INTO \"Progresses\" VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', {5});", item.Id, item.SemesterNumber, item.Subject, item.Mark, item.ExamDate, item.TeacherFIO);
                command.ExecuteNonQuery();
            }

            HttpResponseMessage msg = Request.CreateResponse(HttpStatusCode.Created, item);
            msg.Headers.Location = new Uri(Request.RequestUri + "/" + item.Id);
            return msg;
        }

        // PUT: api/progresses/id
        public HttpResponseMessage Put(int id, [FromBody]Progress item)
        {
            int rowAffected;
            using (connection)
            {
                connection.Open();
                command.CommandText = String.Format("UPDATE \"Progresses\" SET \"Id\"={0}, \"SemesterNumber\"='{1}', \"Subject\"='{2}', \"Mark\"='{3}', \"ExamDate\"='{4}', \"TeacherFIO\"='{5}' WHERE \"Id\"='{6}';",
                                                    item.Id, item.SemesterNumber, item.Subject, item.Mark, item.ExamDate, item.TeacherFIO, id);
                rowAffected = command.ExecuteNonQuery();
            }

            return rowAffected != 1 ? Request.CreateResponse(HttpStatusCode.NotFound) :
                                      Request.CreateResponse(HttpStatusCode.OK, "Элемент " + id + " изменён"); 
        }

        // DELETE: api/progresses/id
        public HttpResponseMessage Delete(int id)
        {
            int rowAffected;
            using (connection)
            {
                connection.Open();
                command.CommandText = String.Format("DELETE FROM \"Progresses\" WHERE \"Id\"='{0}';", id);
                rowAffected = command.ExecuteNonQuery();
            }

            return rowAffected != 1 ? Request.CreateResponse(HttpStatusCode.NotFound) :
                                      Request.CreateResponse(HttpStatusCode.OK, "Элемент " + id + " удалён");
        }
    }
}
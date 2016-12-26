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
    public class SubjectsController : ApiController
    {
        DataTable table;
        NpgsqlConnectionStringBuilder constr;
        NpgsqlDataAdapter adapter;
        NpgsqlConnection connection;
        NpgsqlCommand command;

        public SubjectsController()
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

        // GET: api/subjects
        public IEnumerable<Subject> Get()
        {
            using (adapter = new NpgsqlDataAdapter("SELECT * FROM \"Subjects\";", constr.ConnectionString))
                adapter.Fill(table);

            return table.ToSubjectsList();
        }

        // GET: api/subjects/id
        public HttpResponseMessage Get(int id)
        {
            using (adapter = new NpgsqlDataAdapter(String.Format("SELECT * FROM \"Subjects\" WHERE \"Id\"={0};", id), constr.ConnectionString))
                adapter.Fill(table);

            return table.Rows.Count != 0 ? Request.CreateResponse(HttpStatusCode.OK, table.ToSubjectsList()) :
                                           Request.CreateResponse(HttpStatusCode.NotFound, "Элемент не найден");
        }

        // POST: api/subjects
        public HttpResponseMessage Post([FromBody]Subject item)
        {
            using (connection)
            {
                connection.Open();
                command.CommandText = String.Format("INSERT INTO \"Subjects\" VALUES ('{0}', '{1}');", item.Id, item.SubjectName);
                command.ExecuteNonQuery();
            }

            HttpResponseMessage msg = Request.CreateResponse(HttpStatusCode.Created, item);
            msg.Headers.Location = new Uri(Request.RequestUri + "/" + item.Id);
            return msg;
        }

        // PUT: api/subjects/id
        public HttpResponseMessage Put(int id, [FromBody]Subject item)
        {
            int rowAffected;
            using (connection)
            {
                connection.Open();
                command.CommandText = String.Format("UPDATE \"Subjects\" SET \"Id\"={0}, \"SubjectName\"='{1}' WHERE \"Id\"='{2}';",
                                                    item.Id, item.SubjectName, id);
                rowAffected = command.ExecuteNonQuery();
            }

            return rowAffected != 1 ? Request.CreateResponse(HttpStatusCode.NotFound) :
                                      Request.CreateResponse(HttpStatusCode.OK, "Элемент " + id + " изменён");                                      
        }

        // DELETE: api/subjects/id
        public HttpResponseMessage Delete(int id)
        {
            int rowAffected;
            using (connection)
            {
                connection.Open();
                command.CommandText = String.Format("DELETE FROM \"Subjects\" WHERE \"Id\"='{0}';", id);
                rowAffected = command.ExecuteNonQuery();
            }

            return rowAffected != 1 ? Request.CreateResponse(HttpStatusCode.NotFound) :
                                      Request.CreateResponse(HttpStatusCode.OK, "Элемент " + id + " удалён");
        }
    }
}
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
    public class DepartmentsController : ApiController
    {
        DataTable table;
        NpgsqlConnectionStringBuilder constr;
        NpgsqlDataAdapter adapter;
        NpgsqlConnection connection;
        NpgsqlCommand command;

        public DepartmentsController()
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

        // GET: api/departments
        public IEnumerable<Department> Get()
        {
            using (adapter = new NpgsqlDataAdapter("SELECT * FROM \"Departments\";", constr.ConnectionString))
                adapter.Fill(table);

            return table.ToDepartmentsList();
        }

        // GET: api/departments/id
        public HttpResponseMessage Get(int id)
        {
            using (adapter = new NpgsqlDataAdapter(String.Format("SELECT * FROM \"Departments\" WHERE \"Id\"={0};", id), constr.ConnectionString))
                adapter.Fill(table);

            return table.Rows.Count != 0 ? Request.CreateResponse(HttpStatusCode.OK, table.ToDepartmentsList()) :
                                           Request.CreateResponse(HttpStatusCode.NotFound, "Элемент не найден");
        }

        // POST: api/departments
        public HttpResponseMessage Post([FromBody]Department item)
        {
            using (connection)
            {
                connection.Open();
                command.CommandText = String.Format("INSERT INTO \"Departments\" VALUES ('{0}', '{1}');", item.Id, item.DepartmentName);
                command.ExecuteNonQuery();
            }

            HttpResponseMessage msg = Request.CreateResponse(HttpStatusCode.Created, item);
            msg.Headers.Location = new Uri(Request.RequestUri + "/" + item.Id);
            return msg;
        }

        // PUT: api/departments/id
        public HttpResponseMessage Put(int id, [FromBody]Department item)
        {
            int rowAffected;
            using (connection)
            {
                connection.Open();
                command.CommandText = String.Format("UPDATE \"Departments\" SET \"Id\"={0}, \"DepartmentName\"='{1}' WHERE \"Id\"='{2}';",
                                                    item.Id, item.DepartmentName, id);
                rowAffected = command.ExecuteNonQuery();
            }

            return rowAffected != 1 ? Request.CreateResponse(HttpStatusCode.NotFound) :
                                      Request.CreateResponse(HttpStatusCode.OK, "Элемент " + id + " изменён");
        }

        // DELETE: api/departments/id
        public HttpResponseMessage Delete(int id)
        {
            int rowAffected;
            using (connection)
            {
                connection.Open();
                command.CommandText = String.Format("DELETE FROM \"Departments\" WHERE \"Id\"='{0}';", id);
                rowAffected = command.ExecuteNonQuery();
            }

            return rowAffected != 1 ? Request.CreateResponse(HttpStatusCode.NotFound) :
                                      Request.CreateResponse(HttpStatusCode.OK, "Элемент " + id + " удалён");
        }
    }
}
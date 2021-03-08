using System.Collections.Generic;
using System.Data;  

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

using MessageLibrary;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NotificationAppAPI
{
    
    [Route("api/[controller]")]
    public class UsersController : Controller
    {    
        static SqlConnection conn = new SqlConnection("Server=tcp:nblydbserver.database.windows.net,1433;Database=coreDB;User ID=sqladmin;Password=SQLsrad#3;");

        // GET: api/values
        [HttpGet]
        public IActionResult GetUsers()
        {
            List<User> userList = new List<User>();
            SqlCommand GetAllUsers = new SqlCommand("GetUsers", conn);
            conn.Open();
			userList = ReadSqlData(GetAllUsers, userList);
			conn.Close();
			return Ok(userList);
		}

        // GET api/values/5
        [HttpGet("{id}")]
        public User GetUser(int id)
        {
            User user = null;
            SqlCommand GetUserById = new SqlCommand("GetUser", conn);
			GetUserById.CommandType = CommandType.StoredProcedure;
            GetUserById.Parameters.Add(new SqlParameter("@user_id", id));
			conn.Open();
			user = ReadSqlData(GetUserById, user);
			conn.Close();
			return user;
        }

        // POST api/values
        [HttpPost]
        public void PostUser([FromBody] User user)
        {
			SqlCommand InsertUser = new SqlCommand("InsertUser", conn);
            AddUserSqlParameters(InsertUser, user);
            conn.Open();            
			InsertUser.ExecuteNonQuery();
            conn.Close();
			Ok("User Created");
		}

        // PUT api/values/5
        [HttpPut("{id}")]
        public User PutUser(int id, [FromBody] User updatedUser)
        {
            SqlCommand UpdateUser = new SqlCommand("UpdateUser", conn);
            updatedUser.UserId = id;
            AddUserSqlParameters(UpdateUser, updatedUser);
            conn.Open();
            UpdateUser.ExecuteNonQuery();
            User user = ReadSqlData(UpdateUser, updatedUser);
            conn.Close();
			Ok("User Created");
			return user;
		}

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
			SqlCommand DeleteUser = new SqlCommand("DeleteUser", conn);
            DeleteUser.CommandType = CommandType.StoredProcedure;
            DeleteUser.Parameters.Add(new SqlParameter("@UserId", id));
			conn.Open();
			DeleteUser.ExecuteNonQuery();
			conn.Close();
			Ok("User Deleted");
		}

        private static void AddUserSqlParameters(SqlCommand command, User user)
        {
            command.CommandType = CommandType.StoredProcedure;
			InsertParam(command, "@FirstName", user.FirstName);
			InsertParam(command, "@LastName", user.LastName);
			InsertParam(command, "@Email", user.Email);
		}
        private static void InsertParam(SqlCommand command, string name, dynamic value) {
			command
                .Parameters
                .Add(new SqlParameter(name, SqlDbType.VarChar))
                .Value = value;
		}

        private static List<User> ReadSqlData(SqlCommand command, List<User> users) 
        {
            User user = null;
            
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
				user = new User((SqlDataReader)reader);
				users.Add(user);
			}
			return users;
		}
        private static User ReadSqlData(SqlCommand command, User user) 
        {
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
				user = new User((SqlDataReader)reader);
			}
			return user;
		}
    }
}

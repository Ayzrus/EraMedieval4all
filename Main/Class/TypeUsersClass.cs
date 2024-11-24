using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Main.Class
{
    public class TypeUsersClass
    {
        #region Constants

        // The name of the table in the database
        public const string Table = "tipo_utilizadores";

        #endregion Constants

        #region Properties

        // Properties representing the columns of the 'tipo_utilizadores' table in the database
        public int Id { get; set; }
        public string Descricao { get; set; }

        #endregion Properties

        #region Static Methods

        /// <summary>
        /// Method to get all users (Read operation)
        /// </summary>
        public static List<TypeUsersClass> GetUsers()
        {
            var users = new List<TypeUsersClass>();
            using (var connection = new Connection())
            {
                // SQL query to fetch all users from the database
                var query = $"SELECT * FROM {Table}";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                using var reader = command.ExecuteReader();
                // Read each record from the database and add to the users list
                while (reader.Read())
                {
                    users.Add(MapFromReader(reader));
                }
            }
            return users;
        }

        /// <summary>
        /// Method to insert a new user (Create operation)
        /// </summary>
        public static bool InsertUser(TypeUsersClass user)
        {
            try
            {
                using var connection = new Connection();
                // SQL query to insert a new user into the database
                var query = $"INSERT INTO {Table} (Descricao) VALUES (@Descricao)";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                // Add parameters to the SQL query to prevent SQL injection
                command.Parameters.AddWithValue("@Descricao", user.Descricao);
                // Execute the query
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Method to update an existing user (Update operation)
        /// </summary>
        public static bool UpdateUser(TypeUsersClass user)
        {
            using var connection = new Connection();
            // SQL query to update an existing user's details
            var query = $"UPDATE {Table} SET Descricao = @Descricao WHERE Id = @Id";
            using var command = new MySqlCommand(query, connection.GetMySqlConnection());
            // Add parameters to the SQL query
            command.Parameters.AddWithValue("@Id", user.Id);
            command.Parameters.AddWithValue("@Descricao", user.Descricao);

            // Execute the query and check how many rows were affected
            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        /// <summary>
        /// Method to delete a user (Delete operation)
        /// </summary>
        public static bool DeleteUser(int id)
        {
            try
            {
                using var connection = new Connection();
                // SQL query to delete a user by its ID
                var query = $"DELETE FROM {Table} WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                // Add user ID as a parameter to the SQL query
                command.Parameters.AddWithValue("@Id", id);
                // Execute the delete query
                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Method to get a specific user by its ID (Read operation)
        /// </summary>
        public static TypeUsersClass GetUser(int id)
        {
            TypeUsersClass user = new();
            using (var connection = new Connection())
            {
                // SQL query to fetch a specific user by its ID
                var query = $"SELECT * FROM {Table} WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                // Add user ID as a parameter to the SQL query
                command.Parameters.AddWithValue("@Id", id);
                using var reader = command.ExecuteReader();
                // If the record exists, map it to a TypeUsersClass object
                if (reader.Read())
                {
                    user = MapFromReader(reader);
                }
            }
            return user;
        }

        /// <summary>
        /// Helper method to map the database record to a TypeUsersClass object
        /// </summary>
        private static TypeUsersClass MapFromReader(MySqlDataReader reader)
        {
            return new TypeUsersClass
            {
                Id = reader.Cast<int>("Id"),
                Descricao = reader.Cast<string>("Descricao")
            };
        }

        #endregion Static Methods
    }
}

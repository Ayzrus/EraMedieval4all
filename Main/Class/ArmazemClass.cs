using MySql.Data.MySqlClient;

namespace Main.Class
{
    public class ArmazemClass
    {
        #region Constants

        // The name of the table in the database
        public const string Table = "armazems";

        #endregion Constants

        #region Properties

        // Properties representing the columns of the 'armazems' table in the database
        public int Id { get; set; }
        public string? Nome { get; set; }

        #endregion Properties

        #region Static Methods

        /// <summary>
        /// Method to get all warehouses (Read operation)
        /// </summary>
        public static List<ArmazemClass> GetWarehouses()
        {
            var warehouses = new List<ArmazemClass>();
            using (var connection = new Connection())
            {
                // SQL query to fetch all warehouses from the database
                var query = $"SELECT * FROM {Table}";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                using var reader = command.ExecuteReader();

                // Read each record from the database and add to the warehouses list
                while (reader.Read())
                {
                    warehouses.Add(MapFromReader(reader));
                }
            }
            return warehouses;
        }

        /// <summary>
        /// Method to insert a new warehouse (Create operation)
        /// </summary>
        public static bool InsertWarehouse(ArmazemClass warehouse)
        {
            try
            {
                using var connection = new Connection();
                // SQL query to insert a new warehouse into the database
                var query = $"INSERT INTO {Table} (Nome) VALUES (@Nome)";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());

                // Add parameters to the SQL query to prevent SQL injection
                command.Parameters.AddWithValue("@Nome", warehouse.Nome);

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
        /// Method to update an existing warehouse (Update operation)
        /// </summary>
        public static bool UpdateWarehouse(ArmazemClass warehouse)
        {
            using var connection = new Connection();
            // SQL query to update an existing warehouse's details
            var query = $"UPDATE {Table} SET Nome = @Nome WHERE Id = @Id";
            using var command = new MySqlCommand(query, connection.GetMySqlConnection());

            // Add parameters to the SQL query
            command.Parameters.AddWithValue("@Id", warehouse.Id);
            command.Parameters.AddWithValue("@Nome", warehouse.Nome);

            // Execute the query and check how many rows were affected
            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        /// <summary>
        /// Method to delete a warehouse (Delete operation)
        /// </summary>
        public static bool DeleteWarehouse(int id)
        {
            try
            {
                using var connection = new Connection();
                // SQL query to delete a warehouse by its ID
                var query = $"DELETE FROM {Table} WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());

                // Add warehouse ID as a parameter to the SQL query
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
        /// Method to get a specific warehouse by its ID (Read operation)
        /// </summary>
        public static ArmazemClass GetWarehouse(int id)
        {
            ArmazemClass warehouse = new();
            using (var connection = new Connection())
            {
                // SQL query to fetch a specific warehouse by its ID
                var query = $"SELECT * FROM {Table} WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());

                // Add warehouse ID as a parameter to the SQL query
                command.Parameters.AddWithValue("@Id", id);
                using var reader = command.ExecuteReader();

                // If the record exists, map it to an ArmazemClass object
                if (reader.Read())
                {
                    warehouse = MapFromReader(reader);
                }
            }
            return warehouse;
        }

        /// <summary>
        /// Helper method to map the database record to an ArmazemClass object
        /// </summary>
        private static ArmazemClass MapFromReader(MySqlDataReader reader)
        {
            return new ArmazemClass
            {
                Id = reader.Cast<int>("Id"),
                Nome = reader.Cast<string>("Nome")
            };
        }

        #endregion Static Methods
    }
}

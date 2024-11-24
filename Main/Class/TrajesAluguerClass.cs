using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Main.Class
{
    public class TrajesAluguerClass
    {
        #region Constants

        // The name of the table in the database
        public const string Table = "trajes_alugar";

        #endregion Constants

        #region Properties

        // Properties representing the columns of the 'trajes_alugar' table in the database
        public int Id { get; set; }
        public int Traje { get; set; }  // Represents the outfit ID
        public int Alugar { get; set; } // Represents the rental ID or status

        #endregion Properties

        #region Static Methods

        /// <summary>
        /// Method to get all rental outfits (Read operation)
        /// </summary>
        public static List<TrajesAluguerClass> GetAllRentals()
        {
            var rentals = new List<TrajesAluguerClass>();
            using (var connection = new Connection())
            {
                // SQL query to fetch all rental outfits from the database
                var query = $"SELECT * FROM {Table}";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                using var reader = command.ExecuteReader();
                // Read each record from the database and add to the rentals list
                while (reader.Read())
                {
                    rentals.Add(MapFromReader(reader));
                }
            }
            return rentals;
        }

        /// <summary>
        /// Method to insert a new rental outfit (Create operation)
        /// </summary>
        public static bool InsertRental(TrajesAluguerClass rental)
        {
            try
            {
                using var connection = new Connection();
                // SQL query to insert a new rental outfit into the database
                var query = $"INSERT INTO {Table} (Traje, Alugar) VALUES (@Traje, @Alugar)";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                // Add parameters to the SQL query to prevent SQL injection
                command.Parameters.AddWithValue("@Traje", rental.Traje);
                command.Parameters.AddWithValue("@Alugar", rental.Alugar);
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
        /// Method to update an existing rental outfit (Update operation)
        /// </summary>
        public static bool UpdateRental(TrajesAluguerClass rental)
        {
            using var connection = new Connection();
            // SQL query to update an existing rental outfit's details
            var query = $"UPDATE {Table} SET Traje = @Traje, Alugar = @Alugar WHERE Id = @Id";
            using var command = new MySqlCommand(query, connection.GetMySqlConnection());
            // Add parameters to the SQL query
            command.Parameters.AddWithValue("@Id", rental.Id);
            command.Parameters.AddWithValue("@Traje", rental.Traje);
            command.Parameters.AddWithValue("@Alugar", rental.Alugar);

            // Execute the query and check how many rows were affected
            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        /// <summary>
        /// Method to delete a rental outfit (Delete operation)
        /// </summary>
        public static bool DeleteRental(int id)
        {
            try
            {
                using var connection = new Connection();
                // SQL query to delete a rental outfit by its ID
                var query = $"DELETE FROM {Table} WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                // Add rental ID as a parameter to the SQL query
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
        /// Method to get a specific rental outfit by its ID (Read operation)
        /// </summary>
        public static TrajesAluguerClass GetRentalById(int id)
        {
            TrajesAluguerClass rental = new();
            using (var connection = new Connection())
            {
                // SQL query to fetch a specific rental outfit by its ID
                var query = $"SELECT * FROM {Table} WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                // Add rental ID as a parameter to the SQL query
                command.Parameters.AddWithValue("@Id", id);
                using var reader = command.ExecuteReader();
                // If the record exists, map it to a TrajesAluguerClass object
                if (reader.Read())
                {
                    rental = MapFromReader(reader);
                }
            }
            return rental;
        }


        public static bool DeleteTrajesByAluguerId(int id)
        {
            try
            {
                using var connection = new Connection();
                // SQL query to delete a rental outfit by its ID
                var query = $"DELETE FROM {Table} WHERE Alugar = @Id";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                // Add rental ID as a parameter to the SQL query
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
        /// Helper method to map the database record to a TrajesAluguerClass object
        /// </summary>
        private static TrajesAluguerClass MapFromReader(MySqlDataReader reader)
        {
            return new TrajesAluguerClass
            {
                Id = reader.Cast<int>("Id"),
                Traje = reader.Cast<int>("Traje"),
                Alugar = reader.Cast<int>("Alugar")
            };
        }

        #endregion Static Methods
    }
}

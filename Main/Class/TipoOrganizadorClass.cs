using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Main.Class
{
    public class TipoOrganizadorClass
    {
        #region Constants

        // The name of the table in the database
        public const string Table = "tipo_organizador";

        #endregion Constants

        #region Properties

        // Properties representing the columns of the 'tipo_organizador' table in the database
        public int Id { get; set; }
        public string? Descricao { get; set; }

        #endregion Properties

        #region Static Methods

        /// <summary>
        /// Method to get all tipo organizador (Read operation)
        /// </summary>
        public static List<TipoOrganizadorClass> GetAllTipoOrganizadores()
        {
            var tipoOrganizadores = new List<TipoOrganizadorClass>();
            using (var connection = new Connection())
            {
                // SQL query to fetch all records from the 'tipo_organizador' table
                var query = $"SELECT * FROM {Table}";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                using var reader = command.ExecuteReader();
                // Read each record from the database and add to the tipoOrganizadores list
                while (reader.Read())
                {
                    tipoOrganizadores.Add(MapFromReader(reader));
                }
            }
            return tipoOrganizadores;
        }

        /// <summary>
        /// Method to insert a new tipo organizador (Create operation)
        /// </summary>
        public static bool InsertTipoOrganizador(TipoOrganizadorClass tipoOrganizador)
        {
            try
            {
                using var connection = new Connection();
                // SQL query to insert a new tipo organizador into the database
                var query = $"INSERT INTO {Table} (Descricao) VALUES (@Descricao)";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                // Add parameters to the SQL query to prevent SQL injection
                command.Parameters.AddWithValue("@Descricao", tipoOrganizador.Descricao);
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
        /// Method to update an existing tipo organizador (Update operation)
        /// </summary>
        public static bool UpdateTipoOrganizador(TipoOrganizadorClass tipoOrganizador)
        {
            using var connection = new Connection();
            // SQL query to update an existing tipo organizador's details
            var query = $"UPDATE {Table} SET Descricao = @Descricao WHERE Id = @Id";
            using var command = new MySqlCommand(query, connection.GetMySqlConnection());
            // Add parameters to the SQL query
            command.Parameters.AddWithValue("@Id", tipoOrganizador.Id);
            command.Parameters.AddWithValue("@Descricao", tipoOrganizador.Descricao);

            // Execute the query and check how many rows were affected
            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        /// <summary>
        /// Method to delete a tipo organizador (Delete operation)
        /// </summary>
        public static bool DeleteTipoOrganizador(int id)
        {
            try
            {
                using var connection = new Connection();
                // SQL query to delete a tipo organizador by its ID
                var query = $"DELETE FROM {Table} WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                // Add tipo organizador ID as a parameter to the SQL query
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
        /// Method to get a specific tipo organizador by its ID (Read operation)
        /// </summary>
        public static TipoOrganizadorClass GetTipoOrganizador(int id)
        {
            TipoOrganizadorClass tipoOrganizador = new();
            using (var connection = new Connection())
            {
                // SQL query to fetch a specific tipo organizador by its ID
                var query = $"SELECT * FROM {Table} WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                // Add tipo organizador ID as a parameter to the SQL query
                command.Parameters.AddWithValue("@Id", id);
                using var reader = command.ExecuteReader();
                // If the record exists, map it to a TipoOrganizadorClass object
                if (reader.Read())
                {
                    tipoOrganizador = MapFromReader(reader);
                }
            }
            return tipoOrganizador;
        }

        /// <summary>
        /// Helper method to map the database record to a TipoOrganizadorClass object
        /// </summary>
        private static TipoOrganizadorClass MapFromReader(MySqlDataReader reader)
        {
            return new TipoOrganizadorClass
            {
                Id = reader.GetInt32("Id"),
                Descricao = reader.GetString("Descricao")
            };
        }

        #endregion Static Methods
    }
}

using MySql.Data.MySqlClient;

namespace Main.Class
{
    public class OrganizadorClass
    {
        #region Constants

        // The name of the table in the database
        public const string Table = "organizadores";

        #endregion Constants

        #region Properties

        // Properties representing the columns of the 'organizadores' table in the database
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Localidade { get; set; }
        public string? Nif { get; set; }
        public int IdTipo { get; set; }

        #endregion Properties

        #region Static Methods

        /// <summary>
        /// Method to get all organizers (Read operation)
        /// </summary>
        public static List<OrganizadorClass> GetOrganizadores()
        {
            var organizadores = new List<OrganizadorClass>();
            using (var connection = new Connection())
            {
                // SQL query to fetch all organizers from the database
                var query = $"SELECT * FROM {Table}";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                using var reader = command.ExecuteReader();
                // Read each record from the database and add to the organizadores list
                while (reader.Read())
                {
                    organizadores.Add(MapFromReader(reader));
                }
            }
            return organizadores;
        }

        /// <summary>
        /// Method to insert a new organizer (Create operation)
        /// </summary>
        public static bool InsertOrganizador(OrganizadorClass organizador)
        {
            try
            {
                using var connection = new Connection();
                // SQL query to insert a new organizer into the database
                var query = $"INSERT INTO {Table} (Nome, Localidade, Nif, IdTipo) VALUES (@Nome, @Localidade, @Nif, @IdTipo)";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                // Add parameters to the SQL query to prevent SQL injection
                command.Parameters.AddWithValue("@Nome", organizador.Nome);
                command.Parameters.AddWithValue("@Localidade", organizador.Localidade);
                command.Parameters.AddWithValue("@Nif", organizador.Nif);
                command.Parameters.AddWithValue("@IdTipo", organizador.IdTipo);
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
        /// Method to update an existing organizer (Update operation)
        /// </summary>
        public static bool UpdateOrganizador(OrganizadorClass organizador)
        {
            using var connection = new Connection();
            // SQL query to update an existing organizer's details
            var query = $"UPDATE {Table} SET Nome = @Nome, Localidade = @Localidade, Nif = @Nif, IdTipo = @IdTipo WHERE Id = @Id";
            using var command = new MySqlCommand(query, connection.GetMySqlConnection());
            // Add parameters to the SQL query
            command.Parameters.AddWithValue("@Id", organizador.Id);
            command.Parameters.AddWithValue("@Nome", organizador.Nome);
            command.Parameters.AddWithValue("@Localidade", organizador.Localidade);
            command.Parameters.AddWithValue("@Nif", organizador.Nif);
            command.Parameters.AddWithValue("@IdTipo", organizador.IdTipo);

            // Execute the query and check how many rows were affected
            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        /// <summary>
        /// Method to delete an organizer (Delete operation)
        /// </summary>
        public static bool DeleteOrganizador(int id)
        {
            try
            {
                using var connection = new Connection();
                // SQL query to delete an organizer by its ID
                var query = $"DELETE FROM {Table} WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                // Add organizer ID as a parameter to the SQL query
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
        /// Method to get a specific organizer by its ID (Read operation)
        /// </summary>
        public static OrganizadorClass GetOrganizador(int id)
        {
            OrganizadorClass organizador = new();
            using (var connection = new Connection())
            {
                // SQL query to fetch a specific organizer by its ID
                var query = $"SELECT * FROM {Table} WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                // Add organizer ID as a parameter to the SQL query
                command.Parameters.AddWithValue("@Id", id);
                using var reader = command.ExecuteReader();
                // If the record exists, map it to an OrganizadorClass object
                if (reader.Read())
                {
                    organizador = MapFromReader(reader);
                }
            }
            return organizador;
        }

        /// <summary>
        /// Helper method to map the database record to an OrganizadorClass object
        /// </summary>
        private static OrganizadorClass MapFromReader(MySqlDataReader reader)
        {
            return new OrganizadorClass
            {
                Id = reader.Cast<int>("Id"),
                Nome = reader.Cast<string>("Nome"),
                Localidade = reader.Cast<string>("Localidade"),
                Nif = reader.Cast<string>("Nif"),
                IdTipo = reader.Cast<int>("IdTipo")
            };
        }

        #endregion Static Methods
    }
}

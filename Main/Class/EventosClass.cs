using MySql.Data.MySqlClient;

namespace Main.Class
{
    public class EventosClass
    {
        #region Constants
        public const string Table = "eventos"; // The name of the table in the database
        #endregion

        #region Properties
        public int Id { get; set; } // Event ID
        public string? Localidade { get; set; } // Event location
        public string? Descricao { get; set; } // Event description
        public string? Titulo { get; set; } // Event title
        public DateTime DataInicio { get; set; } // Event start date
        public DateTime DataFim { get; set; } // Event end date
        public string? Facebook { get; set; } // Event's Facebook link
        public string? Instagram { get; set; } // Event's Instagram link
        public string? TikTok { get; set; } // Event's TikTok link
        public int Organizador { get; set; } // Event organizer ID
        public string? OrganizadorNome { get; set; } // Event organizer ID

        #endregion

        #region Static Methods

        /// <summary>
        /// Method to get all events from the database
        /// </summary>
        public static List<EventosClass> GetEventos()
        {
            var eventos = new List<EventosClass>(); // List to store events
            using (var connection = new Connection()) // Open database connection
            {
                var query = @$"SELECT E.*, O.Nome AS OrganizadorNome FROM {Table} E
                    LEFT JOIN {OrganizadorClass.Table} O ON O.Id = E.Organizador
                "; // SQL query to get all events
                using var command = new MySqlCommand(query, connection.GetMySqlConnection()); // Execute query
                using var reader = command.ExecuteReader(); // Read query results
                while (reader.Read())
                {
                    eventos.Add(MapFromReader(reader)); // Map reader data to event object
                }
            }
            return eventos; // Return list of events
        }

        /// <summary>
        /// Method to insert a new event into the database
        /// </summary>
        public static bool InsertEvento(EventosClass evento)
        {
            try
            {
                using var connection = new Connection(); // Open database connection
                var query = $"INSERT INTO {Table} (Localidade, Descricao, Titulo, DataInicio, DataFim, Facebook, Instagram, TikTok, Organizador) " +
                            "VALUES (@Localidade, @Descricao, @Titulo, @DataInicio, @DataFim, @Facebook, @Instagram, @TikTok, @Organizador)"; // SQL insert query
                using var command = new MySqlCommand(query, connection.GetMySqlConnection()); // Prepare command
                // Add parameters to the command to prevent SQL injection
                command.Parameters.AddWithValue("@Localidade", evento.Localidade);
                command.Parameters.AddWithValue("@Descricao", evento.Descricao);
                command.Parameters.AddWithValue("@Titulo", evento.Titulo);
                command.Parameters.AddWithValue("@DataInicio", evento.DataInicio);
                command.Parameters.AddWithValue("@DataFim", evento.DataFim);
                command.Parameters.AddWithValue("@Facebook", evento.Facebook);
                command.Parameters.AddWithValue("@Instagram", evento.Instagram);
                command.Parameters.AddWithValue("@TikTok", evento.TikTok);
                command.Parameters.AddWithValue("@Organizador", evento.Organizador);
                command.ExecuteNonQuery(); // Execute the query
                return true; // Return true if the event is inserted
            }
            catch (Exception)
            {
                return false; // Return false if an error occurs
            }
        }

        /// <summary>
        /// Method to update an existing event in the database
        /// </summary>
        public static bool UpdateEvento(EventosClass evento)
        {
            using var connection = new Connection(); // Open database connection
            var query = $"UPDATE {Table} SET Localidade = @Localidade, Descricao = @Descricao, Titulo = @Titulo, DataInicio = @DataInicio, " +
                        "DataFim = @DataFim, Facebook = @Facebook, Instagram = @Instagram, TikTok = @TikTok, Organizador = @Organizador " +
                        "WHERE Id = @Id"; // SQL update query
            using var command = new MySqlCommand(query, connection.GetMySqlConnection()); // Prepare command
            // Add parameters to the command to prevent SQL injection
            command.Parameters.AddWithValue("@Id", evento.Id);
            command.Parameters.AddWithValue("@Localidade", evento.Localidade);
            command.Parameters.AddWithValue("@Descricao", evento.Descricao);
            command.Parameters.AddWithValue("@Titulo", evento.Titulo);
            command.Parameters.AddWithValue("@DataInicio", evento.DataInicio);
            command.Parameters.AddWithValue("@DataFim", evento.DataFim);
            command.Parameters.AddWithValue("@Facebook", evento.Facebook);
            command.Parameters.AddWithValue("@Instagram", evento.Instagram);
            command.Parameters.AddWithValue("@TikTok", evento.TikTok);
            command.Parameters.AddWithValue("@Organizador", evento.Organizador);

            int rowsAffected = command.ExecuteNonQuery(); // Execute the update query
            return rowsAffected > 0; // Return true if any rows are affected
        }

        /// <summary>
        /// Method to delete an event from the database
        /// </summary>
        public static bool DeleteEvento(int id)
        {
            try
            {
                using var connection = new Connection(); // Open database connection
                var query = $"DELETE FROM {Table} WHERE Id = @Id"; // SQL delete query
                using var command = new MySqlCommand(query, connection.GetMySqlConnection()); // Prepare command
                command.Parameters.AddWithValue("@Id", id); // Add event ID as parameter
                command.ExecuteNonQuery(); // Execute the delete query
                return true; // Return true if the event is deleted
            }
            catch (Exception)
            {
                return false; // Return false if an error occurs
            }
        }

        /// <summary>
        /// Method to get a specific event by its ID from the database
        /// </summary>
        public static EventosClass GetEvento(int id)
        {
            EventosClass evento = new(); // Create new EventosClass object
            using (var connection = new Connection()) // Open database connection
            {
                var query = @$"SELECT E.*, O.Nome AS OrganizadorNome FROM {Table} E
                    LEFT JOIN {OrganizadorClass.Table} O ON O.Id = E.Organizador
                    WHERE E.Id = @Id
                "; // SQL query to get specific event
                using var command = new MySqlCommand(query, connection.GetMySqlConnection()); // Prepare command
                command.Parameters.AddWithValue("@Id", id); // Add event ID as parameter
                using var reader = command.ExecuteReader(); // Execute query
                if (reader.Read())
                {
                    evento = MapFromReader(reader); // Map data to EventosClass object
                }
            }
            return evento; // Return the event object
        }

        /// <summary>
        /// Helper method to map data from a MySQLDataReader to an EventosClass object
        /// </summary>
        private static EventosClass MapFromReader(MySqlDataReader reader)
        {
            return new EventosClass
            {
                Id = reader.Cast<int>("Id"),
                Localidade = reader.Cast<string>("Localidade"),
                Descricao = reader.Cast<string>("Descricao"),
                Titulo = reader.Cast<string>("Titulo"),
                DataInicio = reader.Cast<DateTime>("DataInicio"),
                DataFim = reader.Cast<DateTime>("DataFim"),
                Facebook = reader.Cast<string>("Facebook"),
                Instagram = reader.Cast<string>("Instagram"),
                TikTok = reader.Cast<string>("TikTok"),
                Organizador = reader.Cast<int>("Organizador"),
                OrganizadorNome = reader.Cast<string>("OrganizadorNome"),
            }; // Map data from reader to EventosClass object
        }

        #endregion
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Main.Class
{
    public class AluguerClass
    {
        #region Constants

        // The name of the table in the database
        public const string Table = "alugar";

        #endregion Constants

        #region Properties

        // Properties representing the columns of the 'alugar' table in the database
        public int Id { get; set; }
        public DateTime DataAlugou { get; set; }
        public int Cliente { get; set; }
        public string? Estado { get; set; }
        public DateTime DataEntrega { get; set; }
        public int Evento { get; set; }
        public string? ClienteNome { get; set; }
        public string? EventoNome { get; set; }
        public int Ativos { get; set; }

        #endregion Properties

        #region Static Methods

        /// <summary>
        /// Method to get all rentals (Read operation)
        /// </summary>
        public static List<AluguerClass> GetAlugueres()
        {
            var alugueres = new List<AluguerClass>();
            using (var connection = new Connection())
            {
                // SQL query to fetch all rentals from the database
                var query = @$"SELECT A.*, 0 AS Ativos, 1 AS TotalValor, C.Nome AS ClienteNome, E.Titulo AS EventoNome FROM {Table} A
                    LEFT JOIN {ClientesClass.Table} C ON C.Id = A.Cliente
                    LEFT JOIN {EventosClass.Table} E ON E.Id = A.Evento
                ";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
					alugueres.Add(MapFromReader(reader));
                }
            }
            return alugueres;
        }

		/// <summary>
		/// Method to get the count of active rentals
		/// </summary>
		public static int GetAlugueresActiveCount()
		{
			int activeCount = 0;
			using (var connection = new Connection())
			{
				// SQL query to count active rentals
				var query = @$"
            SELECT COUNT(*) AS Ativos, 0 AS TotalValor
            FROM {Table} A
            WHERE A.Estado = 'Decorrer';
        ";
				using var command = new MySqlCommand(query, connection.GetMySqlConnection());
				activeCount = Convert.ToInt32(command.ExecuteScalar());
			}
			return activeCount;
		}

		public static decimal GetTotalValorAnoAtual()
		{
			decimal totalValor = 0;
			using (var connection = new Connection())
			{
				// SQL query to calculate the total value of rentals for the current year
				var query = @$"
            SELECT SUM(T.Valor) AS TotalValor, 0 AS Ativos
            FROM {TrajesAluguerClass.Table} TA
            LEFT JOIN {Table} A ON TA.Alugar = A.Id
            LEFT JOIN {TrajesClass.Table} T ON TA.Traje = T.Id
            WHERE YEAR(A.DataAlugou) = YEAR(CURDATE());
        ";
				using var command = new MySqlCommand(query, connection.GetMySqlConnection());
				var result = command.ExecuteScalar();
				totalValor = result != DBNull.Value ? Convert.ToDecimal(result) : 0;
			}
			return totalValor;
		}


		/// <summary>
		/// Method to insert a new rental (Create operation)
		/// </summary>
		/// <returns>Tuple containing a boolean indicating success or failure and the last inserted ID</returns>
		public static (bool success, int? lastInsertedId) InsertAluguer(AluguerClass aluguer)
        {
            try
            {
                using var connection = new Connection();
                var query = $"INSERT INTO {Table} (Cliente, DataEntrega, Evento) VALUES (@Cliente, @DataEntrega, @Evento)";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());

                command.Parameters.AddWithValue("@Cliente", aluguer.Cliente);
                command.Parameters.AddWithValue("@DataEntrega", aluguer.DataEntrega);
                command.Parameters.AddWithValue("@Evento", aluguer.Evento);

                // Execute the insertion
                command.ExecuteNonQuery();

                // Retrieve the last inserted ID
                var lastInsertedId = Convert.ToInt32(command.LastInsertedId);

                return (true, lastInsertedId); // Return true along with the last inserted ID
            }
            catch (Exception)
            {
                return (false, null); // Return false if there's an error
            }
        }


        /// <summary>
        /// Method to update an existing rental (Update operation)
        /// </summary>
        public static bool UpdateAluguer(AluguerClass aluguer)
        {
            using var connection = new Connection();
            var query = $"UPDATE {Table} SET Cliente = @Cliente, DataEntrega = @DataEntrega, Evento = @Evento WHERE Id = @Id";
            using var command = new MySqlCommand(query, connection.GetMySqlConnection());

            command.Parameters.AddWithValue("@Id", aluguer.Id);
            command.Parameters.AddWithValue("@Cliente", aluguer.Cliente);
            command.Parameters.AddWithValue("@DataEntrega", aluguer.DataEntrega);
            command.Parameters.AddWithValue("@Evento", aluguer.Evento);

            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        /// <summary>
        /// Method to copy an existing rental (Create new rental based on existing one)
        /// </summary>
        public static bool CopyAluguer(AluguerClass aluguer)
        {
            using var connection = new Connection();

            var querySelect = $"SELECT DataAlugou, Cliente, Evento FROM {Table} WHERE Id = @Id";
            using var selectCommand = new MySqlCommand(querySelect, connection.GetMySqlConnection());
            selectCommand.Parameters.AddWithValue("@Id", aluguer.Id);

            var reader = selectCommand.ExecuteReader();
            if (reader.Read())
            {
                var cliente = reader.GetInt32("Cliente");
                var evento = reader.GetInt32("Evento");
                var dataAlugou = reader.GetDateTime("DataAlugou");
                reader.Close();
                var queryInsert = $"INSERT INTO {Table} (Cliente, DataAlugou, DataEntrega, Evento) VALUES (@Cliente, @DataAlugou, @DataEntrega, @Evento)";
                using var insertCommand = new MySqlCommand(queryInsert, connection.GetMySqlConnection());

                insertCommand.Parameters.AddWithValue("@Cliente", cliente);
                insertCommand.Parameters.AddWithValue("@DataEntrega", aluguer.DataEntrega);
                insertCommand.Parameters.AddWithValue("@DataAlugou", dataAlugou);
                insertCommand.Parameters.AddWithValue("@Evento", evento);

                int rowsAffected = insertCommand.ExecuteNonQuery();
                return rowsAffected > 0;
            }

            return false;
        }

        /// <summary>
        /// Method to delete a rental (Delete operation)
        /// </summary>
        public static bool DeleteAluguer(int id)
        {
            try
            {
                using var connection = new Connection();
                var query = $"DELETE FROM {Table} WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());

                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Method to get a specific rental by its ID (Read operation)
        /// </summary>
        public static AluguerClass GetAluguer(int id)
        {
            AluguerClass aluguer = new();
            using (var connection = new Connection())
            {
                var query = @$"SELECT A.*, 0 AS Ativos, 1 AS TotalValor, C.Nome AS ClienteNome, E.Titulo AS EventoNome FROM {Table} A
                    LEFT JOIN {ClientesClass.Table} C ON C.Id = A.Cliente
                    LEFT JOIN {EventosClass.Table} E ON E.Id = A.Evento
                    WHERE A.Id = @Id
                ";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                command.Parameters.AddWithValue("@Id", id);
                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    aluguer = MapFromReader(reader);
                }
            }
            return aluguer;
        }

        /// <summary>
        /// Inative by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool Inative(int id)
        {
            try
            {
                using var connection = new Connection();
                var query = $"UPDATE {Table} SET Estado = 'Finalizado' WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Helper method to map the database record to an AluguerClass object
        /// </summary>
        private static AluguerClass MapFromReader(MySqlDataReader reader)
        {
            return new AluguerClass
            {
                Id = reader.Cast<int>("Id"),
                DataAlugou = reader.Cast<DateTime>("DataAlugou"),
                Cliente = reader.Cast<int>("Cliente"),
                Estado = reader.Cast<string>("Estado"),
                DataEntrega = reader.Cast<DateTime>("DataEntrega"),
                Evento = reader.Cast<int>("Evento"),
                ClienteNome = reader.Cast<string>("ClienteNome"),
                EventoNome = reader.Cast<string>("EventoNome"),
                Ativos = reader.Cast<int>("Ativos"),
            };
        }

        #endregion Static Methods
    }
}

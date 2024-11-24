using MySql.Data.MySqlClient;

namespace Main.Class
{
    public class TrajesClass
    {
        #region Constants

        // The name of the table in the database
        public const string Table = "trajes";

        #endregion Constants

        #region Properties

        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Estado { get; set; }
        public double Valor { get; set; }
        public int Armazem { get; set; }
        public string? Foto { get; set; }
        public int Quantidade { get; set; }
        public string? Especificacao { get; set; }
        public string? Tipo { get; set; }
        public string? ArmazemNome { get; set; }
        public string? Ref { get; set; }
        public long TrajesAlugados { get; set; }
        public long TrajesRegistados { get; set; }
        public DateTime DataAlugou { get; set; }
        public DateTime DataEntrega { get; set; }

        #endregion Properties

        #region Static Methods

        /// <summary>
        /// Method to get all costumes (Read operation)
        /// </summary>
        public static List<TrajesClass> GetTrajes()
        {
            var trajes = new List<TrajesClass>();
            using (var connection = new Connection())
            {
                var query = @$"SELECT 
                        T.*, 
                        0 AS TrajesAlugados, 
                        1 AS TrajesRegistados, 
                        AL.DataAlugou AS DataAlugou, 
                        AL.DataEntrega AS DataEntrega, 
                        A.Nome AS ArmazemNome
                    FROM {Table} T
                    LEFT JOIN {ArmazemClass.Table} A ON A.Id = T.Armazem
                    LEFT JOIN {TrajesAluguerClass.Table} TA ON TA.Traje = T.Id
                    LEFT JOIN {AluguerClass.Table} AL ON AL.Id = TA.Alugar
                    GROUP BY T.Id, A.Nome;
                ";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    trajes.Add(MapFromReader(reader));
                }
            }
            return trajes;
        }        
        
        public static List<TrajesClass> GetLastTenTrajes()
        {
            var trajes = new List<TrajesClass>();
            using (var connection = new Connection())
            {
                var query = @$"SELECT 
                    T.*, 
                    1 AS TrajesAlugados, 
                    0 AS TrajesRegistados, 
                    AL.DataAlugou AS DataAlugou, 
                    AL.DataEntrega AS DataEntrega, 
                    A.Nome AS ArmazemNome
                FROM {Table} T
                LEFT JOIN {ArmazemClass.Table} A ON A.Id = T.Armazem
                LEFT JOIN {TrajesAluguerClass.Table} TA ON TA.Traje = T.Id
                LEFT JOIN {AluguerClass.Table} AL ON AL.Id = TA.Alugar
                ORDER BY AL.DataAlugou DESC
                LIMIT 10;
                ";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    trajes.Add(MapFromReader(reader));
                }
            }
            return trajes;
        }

		public static int GetTrajesAlugadosCount()
		{
			int totalTrajes = 0;
			using (var connection = new Connection())
			{
				// SQL query to count the total rented costumes
				var query = @$"
            SELECT COUNT(*) AS TrajesAlugados, AL.DataAlugou, AL.DataEntrega
            FROM {TrajesAluguerClass.Table} TA
            LEFT JOIN {Table} T ON TA.Traje = T.Id
            LEFT JOIN {AluguerClass.Table} AL ON AL.Id = TA.Alugar
            WHERE AL.Estado = 'Decorrer'
        ";
				using var command = new MySqlCommand(query, connection.GetMySqlConnection());
				totalTrajes = Convert.ToInt32(command.ExecuteScalar());
			}
			return totalTrajes;
		}

		public static int GetTotalTrajesRegistrados()
		{
			int totalTrajes = 0;
			using (var connection = new Connection())
			{
				// SQL query to count the total registered costumes
				var query = @$"
            SELECT COUNT(*) AS TrajesRegistados
            FROM {Table};
        ";
				using var command = new MySqlCommand(query, connection.GetMySqlConnection());
				totalTrajes = Convert.ToInt32(command.ExecuteScalar());
			}
			return totalTrajes;
		}

		public static List<TrajesClass> GetTrajesAluguer(int idalg)
        {
            var trajes = new List<TrajesClass>();
            using (var connection = new Connection())
            {
                var query = @$"SELECT 
                        T.*, 
                        0 AS TrajesAlugados, 
                        1 AS TrajesRegistados, 
                        AL.DataAlugou AS DataAlugou, 
                        AL.DataEntrega AS DataEntrega, 
                        A.Nome AS ArmazemNome
                    FROM {Table} T
                    LEFT JOIN {ArmazemClass.Table} A ON A.Id = T.Armazem
                    LEFT JOIN {TrajesAluguerClass.Table} TA ON TA.Traje = T.Id
                    LEFT JOIN {AluguerClass.Table} AL ON AL.Id = TA.Alugar
                    WHERE TA.Alugar = @Id
                    GROUP BY T.Id, A.Nome;
                ";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                command.Parameters.AddWithValue("@Id", idalg);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    trajes.Add(MapFromReader(reader));
                }
            }
            return trajes;
        }

        /// <summary>
        /// Method to insert a new costume (Create operation)
        /// </summary>
        public static (bool success, string message) InsertTraje(TrajesClass traje)
        {
            try
            {
                using var connection = new Connection();

                // Verificar se a referência já existe
                var checkQuery = $"SELECT COUNT(1) FROM {Table} WHERE Ref = @Ref";
                using var checkCommand = new MySqlCommand(checkQuery, connection.GetMySqlConnection());
                checkCommand.Parameters.AddWithValue("@Ref", traje.Ref);

                // Execute the check query to see if the reference already exists
                var exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
                if (exists)
                {
                    return (false, "A referência já existe.");
                }

                // Inserir o novo traje
                var query = $"INSERT INTO {Table} (Nome, Valor, Armazem, Foto, Quantidade, Especificacao, Tipo, Ref) VALUES (@Nome, @Valor, @Armazem, @Foto, @Quantidade, @Especificacao, @Tipo, @Ref)";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());

                command.Parameters.AddWithValue("@Nome", traje.Nome);
                command.Parameters.AddWithValue("@Valor", traje.Valor);
                command.Parameters.AddWithValue("@Armazem", traje.Armazem);
                command.Parameters.AddWithValue("@Foto", traje.Foto);
                command.Parameters.AddWithValue("@Quantidade", traje.Quantidade);
                command.Parameters.AddWithValue("@Especificacao", traje.Especificacao);
                command.Parameters.AddWithValue("@Tipo", traje.Tipo);
                command.Parameters.AddWithValue("@Ref", traje.Ref);

                command.ExecuteNonQuery();
                return (true, "Traje inserido com sucesso.");
            }
            catch (Exception ex)
            {
                // Retorna false com a mensagem de erro em caso de exceção
                return (false, $"Erro ao inserir o traje: {ex.Message}");
            }
        }


        /// <summary>
        /// Method to update an existing costume (Update operation)
        /// </summary>
        public static (bool success, string message) UpdateTraje(TrajesClass traje)
        {
            using var connection = new Connection();

            // Verificar se a referência já existe em outro traje
            var checkQuery = $"SELECT COUNT(1) FROM {Table} WHERE Ref = @Ref AND Id != @Id";
            using var checkCommand = new MySqlCommand(checkQuery, connection.GetMySqlConnection());
            checkCommand.Parameters.AddWithValue("@Ref", traje.Ref);
            checkCommand.Parameters.AddWithValue("@Id", traje.Id);

            var exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
            if (exists)
            {
                return (false, "A referência já existe.");
            }

            var query = $"UPDATE {Table} SET Nome = @Nome, Valor = @Valor, Armazem = @Armazem, Foto = @Foto, Quantidade = @Quantidade, Especificacao = @Especificacao, Tipo = @Tipo, Ref = @Ref WHERE Id = @Id";
            using var command = new MySqlCommand(query, connection.GetMySqlConnection());

            command.Parameters.AddWithValue("@Id", traje.Id);
            command.Parameters.AddWithValue("@Nome", traje.Nome);
            command.Parameters.AddWithValue("@Valor", traje.Valor);
            command.Parameters.AddWithValue("@Armazem", traje.Armazem);
            command.Parameters.AddWithValue("@Foto", traje.Foto);
            command.Parameters.AddWithValue("@Quantidade", traje.Quantidade);
            command.Parameters.AddWithValue("@Especificacao", traje.Especificacao);
            command.Parameters.AddWithValue("@Tipo", traje.Tipo);
            command.Parameters.AddWithValue("@Ref", traje.Ref);

            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                return (true, "Traje atualizado com sucesso.");
            }
            else
            {
                return (false, "Erro ao atualizar o traje ou nenhum dado alterado.");
            }
        }


        /// <summary>
        /// Method to delete a costume (Delete operation)
        /// </summary>
        public static bool DeleteTraje(int id)
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
        /// Method to retrieve a specific costume by ID (Read operation)
        /// </summary>
        public static TrajesClass GetTraje(int id)
        {
            TrajesClass traje = new();
            using (var connection = new Connection())
            {
                var query = @$"SELECT 
    T.*, 
    0 AS TrajesAlugados, 
    1 AS TrajesRegistados, 
    AL.DataAlugou AS DataAlugou, 
    AL.DataEntrega AS DataEntrega, 
    A.Nome AS ArmazemNome
FROM {Table} T
LEFT JOIN {ArmazemClass.Table} A ON A.Id = T.Armazem
LEFT JOIN {TrajesAluguerClass.Table} TA ON TA.Traje = T.Id
LEFT JOIN {AluguerClass.Table} AL ON AL.Id = TA.Alugar
WHERE T.Id = @Id
GROUP BY T.Id, A.Nome
";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());

                command.Parameters.AddWithValue("@Id", id);
                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    traje = MapFromReader(reader);
                }
            }
            return traje;
        }

        public static bool Inative(int id)
        {
            try
            {
                using var connection = new Connection();
                var query = $"UPDATE {Table} SET Estado = 'Inativo' WHERE Id = @Id";
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

		public static List<TrajesClass> GetTop10MostRentedTrajes()
		{
			var trajes = new List<TrajesClass>();
			using (var connection = new Connection())
			{
				// SQL query to get the top 10 most rented costumes ordered by rental count
				var query = @$"SELECT 
                        T.*, 
                        COUNT(AL.Id) AS TrajesAlugados, 
                        0 AS TrajesRegistados, 
                        AL.DataAlugou AS DataAlugou, 
                        AL.DataEntrega AS DataEntrega, 
                        A.Nome AS ArmazemNome
                    FROM {Table} T
                    LEFT JOIN {ArmazemClass.Table} A ON A.Id = T.Armazem
                    LEFT JOIN {TrajesAluguerClass.Table} TA ON TA.Traje = T.Id
                    LEFT JOIN {AluguerClass.Table} AL ON AL.Id = TA.Alugar
                    GROUP BY T.Id, A.Nome
                    ORDER BY TrajesAlugados DESC
                    LIMIT 10;
                ";
				using var command = new MySqlCommand(query, connection.GetMySqlConnection());
				using var reader = command.ExecuteReader();

				while (reader.Read())
				{
					trajes.Add(MapFromReader(reader));
				}
			}
			return trajes;
		}


		/// <summary>
		/// Helper method to map a database record to a TrajesClass object
		/// </summary>
		private static TrajesClass MapFromReader(MySqlDataReader reader)
        {
            return new TrajesClass
            {
                Id = reader.Cast<int>("Id"),
                Nome = reader.Cast<string>("Nome"),
                Estado = reader.Cast<string>("Estado"),
                Valor = reader.Cast<double>("Valor"),
                Armazem = reader.Cast<int>("Armazem"),
                Foto = reader.Cast<string>("Foto"),
                Quantidade = reader.Cast<int>("Quantidade"),
                Especificacao = reader.Cast<string>("Especificacao"),
                Tipo = reader.Cast<string>("Tipo"),
                ArmazemNome = reader.Cast<string>("ArmazemNome"),
                Ref = reader.Cast<string>("Ref"),
				TrajesAlugados = reader.IsDBNull(reader.GetOrdinal("TrajesAlugados")) ? 0 : reader.GetInt64(reader.GetOrdinal("TrajesAlugados")),
				TrajesRegistados = reader.IsDBNull(reader.GetOrdinal("TrajesRegistados")) ? 0 : reader.GetInt64(reader.GetOrdinal("TrajesRegistados")),
				DataAlugou = reader.Cast<DateTime>("DataAlugou"),
				DataEntrega = reader.Cast<DateTime>("DataEntrega"),
            };
        }

        #endregion Static Methods
    }
}

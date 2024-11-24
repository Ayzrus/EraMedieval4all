using MySql.Data.MySqlClient;

namespace Main.Class
{
    public class ClientesClass
    {
        #region Constants

        // The name of the table in the database
        public const string Table = "clientes";

        #endregion Constants

        #region Properties

        // Properties representing the columns of the 'clientes' table in the database
        public int Id { get; set; }
        public string? Nif { get; set; }
        public string? Nome { get; set; }
        public string? Morada { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public string? Estado { get; set; }
        public int User { get; set; }
        public long Quantidade { get; set; }
        public long Valor { get; set; }

        #endregion Properties

        #region Static Methods

        /// <summary>
        /// Method to get all clients (Read operation)
        /// </summary>
        public static List<ClientesClass> GetClients()
        {
            var clients = new List<ClientesClass>();
            using (var connection = new Connection())
            {
                // SQL query to fetch all clients from the database
                var query = $"SELECT *, CAST(0 AS SIGNED) AS Quantidade, CAST(1 AS SIGNED) AS Valor FROM {Table}";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                using var reader = command.ExecuteReader();
                // Read each record from the database and add to the clients list
                while (reader.Read())
                {
                    clients.Add(MapFromReader(reader));
                }
            }
            return clients;
        }

		public static List<ClientesClass> GetLastFiveClient()
		{
			var registros = new List<ClientesClass>();
			using (var connection = new Connection())
			{
				var query = @$"
            SELECT *, CAST(0 AS SIGNED) AS Quantidade, CAST(1 AS SIGNED) AS Valor
            FROM {Table}
            ORDER BY Id DESC
            LIMIT 5;
        ";
				using var command = new MySqlCommand(query, connection.GetMySqlConnection());
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					registros.Add(MapFromReader(reader));
				}
			}
			return registros;
		}		
        
        public static List<ClientesClass> GetClientsFilter()
		{
			var registros = new List<ClientesClass>();
			using (var connection = new Connection())
			{
				var query = @$"
            SELECT 
                C.*, 
                COUNT(TA.Id) AS Quantidade, CAST(1 AS SIGNED) AS Valor
            FROM 
                {Table} C
            LEFT JOIN 
                {AluguerClass.Table} A ON A.Cliente = C.Id
            LEFT JOIN 
                {TrajesAluguerClass.Table} TA ON TA.Alugar = A.Id
            LEFT JOIN 
                {TrajesClass.Table} T ON T.Id = TA.Traje
            GROUP BY 
                C.Id
            ORDER BY 
                Quantidade DESC;
        ";
				using var command = new MySqlCommand(query, connection.GetMySqlConnection());
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					registros.Add(MapFromReader(reader));
				}
			}
			return registros;
		}        
        
        public static List<ClientesClass> GetClientsHigh()
		{
			var registros = new List<ClientesClass>();
			using (var connection = new Connection())
			{
				var query = @$"
                SELECT 
                    C.*, 
                    SUM(T.Valor) AS Valor,
                    CAST(0 AS SIGNED) AS Quantidade
                FROM 
                    {Table} C
                LEFT JOIN 
                    {AluguerClass.Table} A ON A.Cliente = C.Id
                LEFT JOIN 
                    {TrajesAluguerClass.Table} TA ON TA.Alugar = A.Id
                LEFT JOIN 
                    {TrajesClass.Table} T ON T.Id = TA.Traje
                GROUP BY 
                    C.Id
                ORDER BY 
                    Valor DESC
                LIMIT 25;
        ";
				using var command = new MySqlCommand(query, connection.GetMySqlConnection());
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					registros.Add(MapFromReader(reader));
				}
			}
			return registros;
		}

		public static List<ClientesClass> GetClientN()
		{
			var registros = new List<ClientesClass>();
			using (var connection = new Connection())
			{
				var query = @$"
            SELECT *, CAST(0 AS SIGNED) AS Quantidade, CAST(1 AS SIGNED) AS Valor
            FROM {Table}
            WHERE User = 0
            ORDER BY Id DESC
        ";
				using var command = new MySqlCommand(query, connection.GetMySqlConnection());
				using var reader = command.ExecuteReader();
				while (reader.Read())
				{
					registros.Add(MapFromReader(reader));
				}
			}
			return registros;
		}

		/// <summary>
		/// Method to insert a new client (Create operation)
		/// </summary>
		public static bool InsertClient(ClientesClass client, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using var connection = new Connection();

                // Verifica se já existe um cliente com o mesmo NIF
                var checkQuery = $"SELECT COUNT(*) FROM {Table} WHERE Nif = @Nif";
                using var checkCommand = new MySqlCommand(checkQuery, connection.GetMySqlConnection());
                checkCommand.Parameters.AddWithValue("@Nif", client.Nif);

                // Verifica se algum registro com o NIF fornecido já existe
                var existingClientCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (existingClientCount > 0)
                {
                    errorMessage = "Já existe um cliente com esse NIF.";
                    return false;
                }

                // SQL query to insert a new client into the database
                var query = $"INSERT INTO {Table} (Nif, Nome, Morada, Email, Telefone) VALUES (@Nif, @Nome, @Morada, @Email, @Telefone)";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());

                // Add parameters to the SQL query to prevent SQL injection
                command.Parameters.AddWithValue("@Nif", client.Nif);
                command.Parameters.AddWithValue("@Nome", client.Nome);
                command.Parameters.AddWithValue("@Morada", client.Morada);
                command.Parameters.AddWithValue("@Email", client.Email);
                command.Parameters.AddWithValue("@Telefone", client.Telefone);

                // Execute the query
                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Erro ao tentar adicionar o cliente: {ex.Message}";
                return false;
            }
        }


        /// <summary>
        /// Method to update an existing client (Update operation)
        /// </summary>
        public static (bool success, string message) UpdateClient(ClientesClass client)
        {
            using var connection = new Connection();

            var checkQuery = $"SELECT COUNT(1) FROM {Table} WHERE Nif = @Nif AND Id != @Id";
            using var checkCommand = new MySqlCommand(checkQuery, connection.GetMySqlConnection());
            checkCommand.Parameters.AddWithValue("@Nif", client.Nif);
            checkCommand.Parameters.AddWithValue("@Id", client.Id);

            var exists = Convert.ToInt32(checkCommand.ExecuteScalar()) > 0;
            if (exists)
            {
                return (false, "Já existe um cliente com o mesmo NIF.");
            }

            // SQL query to update an existing client's details
            var query = $"UPDATE {Table} SET Nif = @Nif, Nome = @Nome, Morada = @Morada, Email = @Email, Telefone = @Telefone WHERE Id = @Id";
            using var command = new MySqlCommand(query, connection.GetMySqlConnection());

            // Add parameters to the SQL query
            command.Parameters.AddWithValue("@Id", client.Id);
            command.Parameters.AddWithValue("@Nif", client.Nif);
            command.Parameters.AddWithValue("@Nome", client.Nome);
            command.Parameters.AddWithValue("@Morada", client.Morada);
            command.Parameters.AddWithValue("@Email", client.Email);
            command.Parameters.AddWithValue("@Telefone", client.Telefone);

            // Execute the query and check how many rows were affected
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                return (true, "Cliente atualizado com sucesso.");
            }
            else
            {
                return (false, "Erro ao atualizar o cliente ou nenhum dado alterado.");
            }
        }


        /// <summary>
        /// Method to delete a client (Delete operation)
        /// </summary>
        public static bool DeleteClient(int id)
        {
            try
            {
                using var connection = new Connection();
                // SQL query to delete a client by its ID
                var query = $"DELETE FROM {Table} WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                // Add client ID as a parameter to the SQL query
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
        /// Method to get a specific client by its ID (Read operation)
        /// </summary>
        public static ClientesClass GetClient(int id)
        {
            ClientesClass client = new();
            using (var connection = new Connection())
            {
                // SQL query to fetch a specific client by its ID
                var query = $"SELECT *, CAST(0 AS SIGNED) AS Quantidade, CAST(1 AS SIGNED) AS Valor FROM {Table} WHERE Id = @Id";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                // Add client ID as a parameter to the SQL query
                command.Parameters.AddWithValue("@Id", id);
                using var reader = command.ExecuteReader();
                // If the record exists, map it to a ClientesClass object
                if (reader.Read())
                {
                    client = MapFromReader(reader);
                }
            }
            return client;
        }

        /// <summary>
        /// Helper method to map the database record to a ClientesClass object
        /// </summary>
        private static ClientesClass MapFromReader(MySqlDataReader reader)
        {
            return new ClientesClass
            {
                Id = reader.Cast<int>("Id"),
                Nif = reader.Cast<string>("Nif"),
                Nome = reader.Cast<string>("Nome"),
                Morada = reader.Cast<string>("Morada"),
                Email = reader.Cast<string>("Email"),
                Telefone = reader.Cast<string>("Telefone"),
                Estado = reader.Cast<string>("Estado"),
				User = reader.Cast<int>("User"),
				Quantidade = reader.IsDBNull(reader.GetOrdinal("Quantidade")) ? 0 : reader.GetInt64(reader.GetOrdinal("Quantidade")),
				Valor = reader.IsDBNull(reader.GetOrdinal("Valor")) ? 0 : reader.GetInt64(reader.GetOrdinal("Valor"))
			};
        }

        #endregion Static Methods
    }
}

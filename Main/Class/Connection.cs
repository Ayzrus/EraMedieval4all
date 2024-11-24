using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace Main.Class
{
    internal class Connection : IDisposable
    {
        #region Propriedades

        private readonly string MySqlConnectionString;

        private MySqlConnection? _mysqlConnection = null;

        // Construtor para inicializar a string de conexão
        public Connection()
        {
            var mysqluser = "root";
            var mysqlserver = "localhost";
            var pwd = "";
            var database = "eramedieval4all";

            MySqlConnectionString = new MySqlConnectionStringBuilder
            {
                Server = mysqlserver,
                UserID = mysqluser,
                Password = pwd,
                Database = database
            }.ToString();
        }

        // Método para obter a conexão MySQL de forma assíncrona
        public async Task<MySqlConnection> GetMySqlConnectionAsync()
        {
            if (_mysqlConnection == null)
            {
                _mysqlConnection = new MySqlConnection(MySqlConnectionString);
                try
                {
                    await _mysqlConnection.OpenAsync(); // Abre a conexão de forma assíncrona
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Erro ao conectar ao banco de dados: {ex.Message}");
                    throw; // Lança novamente a exceção para ser tratada no nível superior
                }
            }
            return _mysqlConnection;
        }

        // Método síncrono para abrir a conexão, se necessário
        public MySqlConnection GetMySqlConnection()
        {
            if (_mysqlConnection == null)
            {
                _mysqlConnection = new MySqlConnection(MySqlConnectionString);
                try
                {
                    _mysqlConnection.Open(); // Abre a conexão de forma síncrona
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Erro ao conectar ao banco de dados: {ex.Message}");
                    throw; // Lança novamente a exceção para ser tratada no nível superior
                }
            }
            return _mysqlConnection;
        }

        // Método Dispose para fechar a conexão quando o objeto for descartado
        public void Dispose()
        {
            if (_mysqlConnection != null)
            {
                _mysqlConnection.Close();
                _mysqlConnection.Dispose();
                _mysqlConnection = null;
            }
        }

        #endregion Propriedades
    }
}

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Main.Class
{
    public class LogsUserClass
    {
        #region Constants

        // Name of the table in the database
        public const string Table = "logs_login";

        #endregion Constants

        #region Properties

        // Properties that represent the columns of the 'logs_login' table in the database
        public int Id { get; set; }
        public DateTime Entrou { get; set; }
        public DateTime Saiu { get; set; }
        public string? Email { get; set; }
        public string? Acao { get; set; }
        public string? Ip { get; set; }
        public string? Motivo { get; set; }
        public DateTime DataCriacao { get; set; }
        public string? Nome { get; set; }

        #endregion Properties

        #region Static Methods

        /// <summary>
        /// Method to get all login log records.
        /// </summary>
        public static List<LogsUserClass> GetAllLogs()
        {
            var logs = new List<LogsUserClass>();
            using (var connection = new Connection())
            {
                var query = @$"SELECT l.*, u.Nome
                    FROM {Table} l
                    INNER JOIN (
                        SELECT Email
                        FROM {Table}
                        WHERE Acao = 'Sucesso'
                        GROUP BY Email
                    ) latest ON l.Email = latest.Email
                    INNER JOIN {UsersClass.Table} u ON l.Email = u.Email
                    WHERE l.Acao = 'Sucesso'
                    ORDER BY l.DataCriacao DESC;
                    ;
                ";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    logs.Add(MapFromReader(reader));
                }
            }
            return logs;
        }

        /// <summary>
        /// Method to insert a login log.
        /// </summary>
        public static bool InsertLog(LogsUserClass log)
        {
            try
            {
                using var connection = new Connection();
                var query = $"INSERT INTO {Table} (Email, Entrou, Saiu, Ip, Acao, Motivo, DataCriacao) VALUES (@Email, @Entrou, @Saiu, @Ip, @Acao, @Motivo, @DataCriacao)";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());

                command.Parameters.AddWithValue("@Email", log.Email);
                command.Parameters.AddWithValue("@Entrou", log.Entrou);
                command.Parameters.AddWithValue("@Saiu", log.Saiu);
                command.Parameters.AddWithValue("@Ip", log.Ip);
                command.Parameters.AddWithValue("@Acao", log.Acao);
                command.Parameters.AddWithValue("@Motivo", log.Motivo);
                command.Parameters.AddWithValue("@DataCriacao", log.DataCriacao);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// update exit from user
        /// </summary>
        /// <param name="email"></param>
        public static void UpdateExit(string email)
        {
            using var connection = new Connection();
            var updateQuery = $@"
                UPDATE {Table} 
                SET Saiu = @Saiu 
                WHERE Email = @Email 
                AND Acao = 'Sucesso' 
                AND DataCriacao >= DATE_SUB(NOW(), INTERVAL 1 DAY)
                ORDER BY DataCriacao DESC
                LIMIT 1;
            ";

            using var updateCommand = new MySqlCommand(updateQuery, connection.GetMySqlConnection());
            updateCommand.Parameters.AddWithValue("@Saiu", DateTime.Now);
            updateCommand.Parameters.AddWithValue("@Email", email);

            updateCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Helper method to map the database result to a LogsUserClass object.
        /// </summary>
        private static LogsUserClass MapFromReader(MySqlDataReader reader)
        {
            return new LogsUserClass
            {
                Id = reader.Cast<int>("Id"),
                Email = reader.Cast<string>("Email"),
                Entrou = reader.Cast<DateTime>("Entrou"),
                Saiu = reader.Cast<DateTime>("Saiu"),
                Ip = reader.Cast<string>("Ip"),
                Acao = reader.Cast<string>("Acao"),
                Motivo = reader.Cast<string>("Motivo"),
                DataCriacao = reader.Cast<DateTime>("DataCriacao"),
                Nome = reader.Cast<string>("Nome"),
            };
        }

        #endregion Static Methods
    }
}

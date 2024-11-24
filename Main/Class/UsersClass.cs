using Konscious.Security.Cryptography;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using static Mysqlx.Expect.Open.Types.Condition.Types;

namespace Main.Class
{
    public class UsersClass
    {
        #region Constants

        // Table name constant
        public const string Table = "utilizadores";

        #endregion Constants

        #region Properties

        // Properties related to user details
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Morada { get; set; }
        public int Telefone { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Pin { get; set; }
        public int TipoUser { get; set; }
        public string? Status { get; set; }
        public DateTime DataCricao { get; set; }
        public DateTime UltimoAcesso { get; set; }
        public string? Tipo { get; set; }
        public int Cliente { get; set; }

        private const int MaxFailedAttempts = 3; // Maximum allowed failed login attempts

        #endregion Properties

        #region Static Methods

        // Retrieves all users from the database
        public static List<UsersClass> GetAllUsers()
        {
            var users = new List<UsersClass>();
            using var connection = new Connection();
            var query = @$"SELECT U.*, T.Descricao AS Tipo FROM {Table} U
                LEFT JOIN {TypeUsersClass.Table} T ON T.Id = U.TipoUser
            ";
            using var command = new MySqlCommand(query, connection.GetMySqlConnection());
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(MapFromReader(reader)); // Maps each row to a UserClass object
            }
            return users;
        }        
        
        // Retrieves all users from the database
        public static List<UsersClass> GetAllUsersBlocked()
        {
            var users = new List<UsersClass>();
            using var connection = new Connection();
            var query = @$"SELECT U.*, T.Descricao AS Tipo FROM {Table} U
                LEFT JOIN {TypeUsersClass.Table} T ON T.Id = U.TipoUser
                WHERE U.Status = 'Bloqueado'
            ";
            using var command = new MySqlCommand(query, connection.GetMySqlConnection());
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(MapFromReader(reader)); // Maps each row to a UserClass object
            }
            return users;
        }

        /// <summary>
        /// Inserts a new user into the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static (bool success, string message) InsertUser(UsersClass user)
        {
            try
            {
                using var connection = new Connection();

                // Verificar se o e-mail já está registrado
                var checkEmailQuery = $"SELECT COUNT(1) FROM {Table} WHERE Email = @Email";
                using var checkEmailCommand = new MySqlCommand(checkEmailQuery, connection.GetMySqlConnection());
                checkEmailCommand.Parameters.AddWithValue("@Email", user.Email);

                var emailExists = Convert.ToInt32(checkEmailCommand.ExecuteScalar()) > 0;
                if (emailExists)
                {
                    return (false, "Já existe um usuário com esse e-mail.");
                }

                var query = $@"
            INSERT INTO {Table} 
            (Nome, Morada, Telefone, Email, Password, Pin, TipoUser) 
            VALUES 
            (@Nome, @Morada, @Telefone, @Email, @Password, @Pin, @TipoUser)";

                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                command.Parameters.AddWithValue("@Nome", user.Nome);
                command.Parameters.AddWithValue("@Morada", user.Morada);
                command.Parameters.AddWithValue("@Telefone", user.Telefone);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", HashPassword(user.Password!));
                command.Parameters.AddWithValue("@Pin", HashPassword(user.Pin!));
                command.Parameters.AddWithValue("@TipoUser", user.TipoUser);

                command.ExecuteNonQuery();
                return (true, "Usuário inserido com sucesso.");
            }
            catch (Exception)
            {
                return (false, "Erro ao inserir o usuário.");
            }
        }


        /// <summary>
        /// Update user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static (bool success, string message) UpdateUser(UsersClass user)
        {
            try
            {
                using var connection = new Connection();

                // Verificar se o e-mail já está registrado (e não é o e-mail do próprio usuário que está sendo atualizado)
                var checkEmailQuery = $@"
            SELECT COUNT(1) FROM {Table} 
            WHERE Email = @Email AND Id != @Id";
                using var checkEmailCommand = new MySqlCommand(checkEmailQuery, connection.GetMySqlConnection());
                checkEmailCommand.Parameters.AddWithValue("@Email", user.Email);
                checkEmailCommand.Parameters.AddWithValue("@Id", user.Id);

                var emailExists = Convert.ToInt32(checkEmailCommand.ExecuteScalar()) > 0;
                if (emailExists)
                {
                    return (false, "Já existe um usuário com esse e-mail.");
                }

                var query = $@"
            UPDATE {Table} 
            SET Nome = @Nome, Morada = @Morada, Telefone = @Telefone, 
                Email = @Email, Password = @Password,
                Pin = @Pin, TipoUser = @TipoUser
            WHERE Id = @Id";

                using var command = new MySqlCommand(query, connection.GetMySqlConnection());
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@Nome", user.Nome);
                command.Parameters.AddWithValue("@Morada", user.Morada);
                command.Parameters.AddWithValue("@Telefone", user.Telefone);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", HashPassword(user.Password!));
                command.Parameters.AddWithValue("@Pin", HashPassword(user.Pin!));
                command.Parameters.AddWithValue("@TipoUser", user.TipoUser);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return (true, "Usuário atualizado com sucesso.");
                }
                else
                {
                    return (false, "Erro ao atualizar o usuário ou nenhum dado alterado.");
                }
            }
            catch (Exception)
            {
                return (false, "Erro ao atualizar o usuário.");
            }
        }



        // Deletes a user by ID
        public static bool DeleteUser(int id)
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

        // Retrieves a single user by ID
        public static UsersClass GetUser(int id)
        {
            UsersClass user = new();
            using var connection = new Connection();
            var query = @$"SELECT U.*, T.Descricao AS Tipo FROM {Table} U
                LEFT JOIN {TypeUsersClass.Table} T ON T.Id = U.TipoUser
                WHERE Id = @Id
            ";
            using var command = new MySqlCommand(query, connection.GetMySqlConnection());
            command.Parameters.AddWithValue("@Id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                user = MapFromReader(reader); // Maps the result to a user object
            }
            return user;
        }

        /// <summary>
        /// Method to register a login attempt.
        /// </summary>
        public static void RegisterLoginAttempt(string email, bool success, string ip)
        {
            string action = success ? "Sucesso" : "Falha";
            if (!success && HasExceededAttempts(email))
            {
                action = "Bloqueado";
                LockAccount(email); // Lock the account after too many failed attempts
            }

            LogsUserClass.InsertLog(new LogsUserClass
            {
                Email = email,
                DataCriacao = DateTime.Now,
                Ip = ip,
                Acao = action // Register the action (success, failure, or blocked)
            });
        }

		/// <summary>
		/// Checks if the user has exceeded the maximum number of failed login attempts.
		/// </summary>
		private static bool HasExceededAttempts(string email)
		{
			using var connection = new Connection();

			var limitDate = DateTime.Now.AddMinutes(-1);

			var query = $@"
        SELECT COUNT(*) 
        FROM {LogsUserClass.Table} 
        WHERE Email = @Email 
          AND Acao = 'Falha' 
          AND DataCriacao >= @LimitDate";

			using var command = new MySqlCommand(query, connection.GetMySqlConnection());
			command.Parameters.AddWithValue("@Email", email);
			command.Parameters.AddWithValue("@LimitDate", limitDate);

			var failedAttempts = Convert.ToInt32(command.ExecuteScalar());

			return failedAttempts >= MaxFailedAttempts;
		}

		/// <summary>
		/// Locks the account of a user after exceeding failed attempts.
		/// </summary>
		private static void LockAccount(string email)
        {
            try
            {
                using var connection = new Connection();
                var query = $"UPDATE {Table} SET Status = 'Bloqueado' WHERE Email = @Email";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());

                command.Parameters.AddWithValue("@Email", email);

                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Verifies the user's account status.
        /// </summary>
        public static void Verify(string email)
        {
            try
            {
                using var connection = new Connection();
                var query = $"UPDATE {Table} SET Status = 'Verificado' WHERE Email = @Email";
                using var command = new MySqlCommand(query, connection.GetMySqlConnection());

                command.Parameters.AddWithValue("@Email", email);

                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// Checks if the user's account is locked.
        /// </summary>
        public static bool IsAccountLocked(string email)
        {
            using var connection = new Connection();
            var query = @$"SELECT U.*, T.Descricao AS Tipo FROM {Table} U
                LEFT JOIN {TypeUsersClass.Table} T ON T.Id = U.TipoUser 
                WHERE U.Email = @Email";
            using var command = new MySqlCommand(query, connection.GetMySqlConnection());

            command.Parameters.AddWithValue("@Email", email);
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                var status = reader.Cast<string>("Status");
                return status == "Bloqueado"; // Checks if the status is "Blocked"
            }

            return false; // Account not found or not blocked
        }

        /// <summary>
        /// update last acess from user
        /// </summary>
        /// <param name="userId"></param>
		private static void UpdateLastAcess(int userId)
		{
			using var connection = new Connection();
			var updateQuery = $"UPDATE {Table} SET UltimoAcesso = @UltimoAcesso WHERE Id = @Id";
			using var updateCommand = new MySqlCommand(updateQuery, connection.GetMySqlConnection());
			updateCommand.Parameters.AddWithValue("@UltimoAcesso", DateTime.Now);
			updateCommand.Parameters.AddWithValue("@Id", userId);
			updateCommand.ExecuteNonQuery();
		}

		/// <summary>
		/// Marks users as inactive if they haven't logged in for over 6 months.
		/// </summary>
		public static void MarkInactiveUsers()
        {
            using var connection = new Connection();
            var query = $"UPDATE {Table} SET Status = 'Inativo' WHERE UltimoAcesso < @DateLimit AND Status != 'Inativo'";
            using var command = new MySqlCommand(query, connection.GetMySqlConnection());

            // Setting the date limit to 6 months ago
            DateTime dateLimit = DateTime.Now.AddMonths(-6);
            command.Parameters.AddWithValue("@DateLimit", dateLimit);

            command.ExecuteNonQuery();
        }

        public static (bool success, string message, string email, bool firstJoin, string nome, string tipo, int cliente) ValidateLogin(string login, string password, string ip)
        {

            UsersClass user;

            using var connection = new Connection();
            var query = @$"SELECT U.*, T.Descricao AS Tipo FROM {Table} U
                LEFT JOIN {TypeUsersClass.Table} T ON T.Id = U.TipoUser WHERE U.Email = @Login";
            using var command = new MySqlCommand(query, connection.GetMySqlConnection());
            command.Parameters.AddWithValue("@Login", login);
            using var reader = command.ExecuteReader();

            if (!reader.Read())
            {
                RegisterFailedLog(login, "User not found", ip);
                return (false, "User not found.", "", false, "", "", 0);
            }

            user = MapFromReader(reader);

			if (!VerifyPassword(password, user.Password!))
            {
				RegisterLoginAttempt(login, false, ip);

				RegisterFailedLog(user.Email!, "Incorrect password", ip);
                return (false, "Incorrect password.", "", false, "", "", 0);
            }

			if (IsAccountLocked(user.Email!))
			{
				RegisterFailedLog(user.Email!, "Account locked", ip);
				return (false, "Account locked.", "", false, "", "", 0);
			}

            if (user.Status == "Inativo")
            {
                RegisterFailedLog(user.Email!, "Account Inative", ip);
                return (false, "Account Inative.", "", false, "", "", 0);
            }

			RegisterSuccessfulLog(user.Email!, ip);
            
            if (user.UltimoAcesso == DateTime.MinValue)
            {
				UpdateLastAcess(user.Id);
				return (true, string.Empty, user.Email!, true, "", "", 0);
            }
            else
            {
                UpdateLastAcess(user.Id);
				return (true, string.Empty, user.Email!, false, user.Nome!, user.Tipo!, user.Cliente);
			}
        }

		public static (bool success, string message) ValidatePin(string pin, string email, string ip)
		{
			UsersClass user;

			using var connection = new Connection();
			var query = @$"SELECT U.*, T.Descricao AS Tipo FROM {Table} U
                LEFT JOIN {TypeUsersClass.Table} T ON T.Id = U.TipoUser WHERE U.Email = @Email";
			using var command = new MySqlCommand(query, connection.GetMySqlConnection());
			command.Parameters.AddWithValue("@Email", email);

			using var reader = command.ExecuteReader();

			if (!reader.Read())
			{
				RegisterFailedLog(email, "User not found", ip);
				return (false, "User not found.");
			}

			user = MapFromReader(reader);

			if (!VerifyPassword(pin, user.Pin!))
			{
				RegisterFailedLog(email, "Incorrect PIN", ip);
				return (false, "Incorrect PIN.");
			}
            UnlockAccount(email);
			return (true, "");
		}

		public static (bool success, string message) UpdatePassword(string newPassword, string email, string ip)
		{
			UsersClass user;

			using var connection = new Connection();
			var query = @$"SELECT U.*, T.Descricao AS Tipo FROM {Table} U
                LEFT JOIN {TypeUsersClass.Table} T ON T.Id = U.TipoUser WHERE U.Email = @Email";
			using var command = new MySqlCommand(query, connection.GetMySqlConnection());
			command.Parameters.AddWithValue("@Email", email);

			using var reader = command.ExecuteReader();

			if (!reader.Read())
			{
				RegisterFailedLog(email, "User not found", ip);
				return (false, "User not found.");
			}

			user = MapFromReader(reader);

			reader.Close();

			string hashedPassword = HashPassword(newPassword);

			using var updateCommand = new MySqlCommand($"UPDATE {Table} SET Password = @Password WHERE Email = @Email", connection.GetMySqlConnection());
			updateCommand.Parameters.AddWithValue("@Password", hashedPassword);
			updateCommand.Parameters.AddWithValue("@Email", email);
			int rowsAffected = updateCommand.ExecuteNonQuery();

			if (rowsAffected == 0)
			{
				return (false, "Failed to update password.");
			}

			RegisterSuccessfulLog(email, ip);
			return (true, "Password updated successfully.");
		}


		private static void UnlockAccount(string email)
		{
			using var connection = new Connection();
			var query = $"UPDATE {Table} SET Status = 'Ativo' WHERE Email = @Email";
			using var command = new MySqlCommand(query, connection.GetMySqlConnection());
			command.Parameters.AddWithValue("@Email", email);
			command.ExecuteNonQuery();
		}


		private static void RegisterFailedLog(string email, string reason, string ip)
        {
            LogsUserClass log = new()
            {
                Email = email,
                Acao = "Falha",
                Motivo = reason,
                DataCriacao = DateTime.Now,
                Ip = ip
            };
            LogsUserClass.InsertLog(log);
        }

        private static void RegisterSuccessfulLog(string email, string ip)
        {
            LogsUserClass log = new()
            {
                Email = email,
                Entrou = DateTime.Now,
                Acao = "Sucesso",
                Motivo = "Login sucesso",
                DataCriacao = DateTime.Now,
                Ip = ip
            };
            LogsUserClass.InsertLog(log);
        }

        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return salt;
        }

        private static string HashPassword(string password)
        {
            byte[] salt = GenerateSalt();
            using var hasher = new Argon2i(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8,
                Iterations = 4,
                MemorySize = 65536
            };

            byte[] hash = hasher.GetBytes(32);
            byte[] hashBytes = new byte[salt.Length + hash.Length];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, salt.Length);
            Buffer.BlockCopy(hash, 0, hashBytes, salt.Length, hash.Length);
            return Convert.ToBase64String(hashBytes);
        }

        private static bool VerifyPassword(string password, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[16];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, salt.Length);

            using var hasher = new Argon2i(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8,
                Iterations = 4,
                MemorySize = 65536
            };

            byte[] hash = hasher.GetBytes(32);
            for (int i = 0; i < hash.Length; i++)
            {
                if (hash[i] != hashBytes[salt.Length + i])
                {
                    return false;
                }
            }
            return true;
        }


        public static bool Inative(int id)
        {
            try
            {
                using var connection = new Connection();
                var query = $"UPDATE {Table} SET Status = 'Inativo' WHERE Id = @Id";
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

        /// Maps the data from a database reader to a UsersClass object
        private static UsersClass MapFromReader(MySqlDataReader reader)
        {
            return new UsersClass
            {
                Id = reader.Cast<int>("Id"),
                Nome = reader.Cast<string>("Nome"),
                Morada = reader.Cast<string>("Morada"),
                Telefone = reader.Cast<int>("Telefone"),
                Email = reader.Cast<string>("Email"),
                Password = reader.Cast<string>("Password"),
                Pin = reader.Cast<string>("Pin"),
                TipoUser = reader.Cast<int>("TipoUser"),
                Status = reader.Cast<string>("Status"),
                DataCricao = reader.Cast<DateTime>("DataCriacao"),
				UltimoAcesso = reader.Cast<DateTime>("UltimoAcesso"),
                Tipo = reader.Cast<string>("Tipo"),
                Cliente = reader.Cast<int>("Cliente"),
            };
        }

        #endregion Static Methods
    }
}

using System;
using System.Configuration;
using System.Data.SqlClient;
using Gavrilov_PhotoCenterRegistration.Models;
using PasswordHashing;

namespace Gavrilov_PhotoCenterRegistration
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Создание новой учетной записи для пользователя");
            Console.WriteLine();

            UserAccount account = new UserAccount();

            account.FirstName = ConsoleInput.ReadValue("Введите имя пользователя: ", 50);
            account.LastName = ConsoleInput.ReadValue("Введите фамилию пользователя: ", 50);

            Console.Write("Введите пол пользователя (1 - мужской, 2 - женский): ");
            account.GenderId = Convert.ToInt32(Console.ReadLine());

            account.Login = ConsoleInput.ReadValue("Введите логин пользователя: ", 50);
            account.Password = ConsoleInput.ReadPassword("Введите пароль пользователя: ");

            PasswordHasher hasher = new PasswordHasher();
            account.Salt = Guid.NewGuid().ToString("N").ToUpper();
            account.Hash = hasher.GetPasswordHash(account.Password, account.Salt);

            Console.WriteLine("Хешированный пароль пользователя: " + account.Hash);
            Console.WriteLine();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["PhotoCenterDb"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
INSERT INTO Clients (LastName, FirstName, GenderID) VALUES (@LastName, @FirstName, @GenderID);
DECLARE @ClientID INT = SCOPE_IDENTITY();

INSERT INTO Users (Login, PasswordHash, Salt, Role) VALUES (@Login, @Hash, @Salt, @Role);
DECLARE @UserID INT = SCOPE_IDENTITY();

UPDATE Clients SET UserID = @UserID WHERE ClientID = @ClientID;";

                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@LastName", account.LastName);
                    command.Parameters.AddWithValue("@FirstName", account.FirstName);
                    command.Parameters.AddWithValue("@GenderID", account.GenderId);
                    command.Parameters.AddWithValue("@Login", account.Login);
                    command.Parameters.AddWithValue("@Hash", account.Hash);
                    command.Parameters.AddWithValue("@Salt", account.Salt);
                    command.Parameters.AddWithValue("@Role", "Client");   

                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        command.Transaction = transaction;

                        try
                        {
                            command.ExecuteNonQuery();
                            transaction.Commit();
                            Console.WriteLine("Учетная запись добавлена");
                        }
                        catch (Exception error)
                        {
                            transaction.Rollback();
                            Console.WriteLine("Не удалось добавить учетную запись: " + error.Message);
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Ошибка подключения к базе данных: " + error.Message);
            }

            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
using System;
using System.Data.SQLite;
using System.IO;

namespace EC.Core
{
    internal class UsersRepository
    { 
        private readonly string AppDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ECData");
        
        private string AppDbFullPath => Path.Combine(AppDirectory, "appData.db");

        private string ConnectionStr => $"DataSource={AppDbFullPath}; Version=3;";

        public UsersRepository() 
        {
            CreateLocalStorage();
        }

        private void CreateLocalStorage()
        {
            if (!Directory.Exists(AppDirectory))
            {
                Directory.CreateDirectory(AppDirectory);
            }
            if (!File.Exists(AppDbFullPath))
            {
                SQLiteConnection.CreateFile(AppDbFullPath);
                CreateTables();
            }

            void CreateTables()
            {
                using SQLiteConnection connection = new(ConnectionStr);
                string createUsers = "CREATE TABLE users" +
                        "(id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                        "name TEXT NOT NULL, password TEXT NOT NULL)";
                SQLiteCommand createUsersCommand = new(createUsers, connection);
                connection.Open();
                createUsersCommand.ExecuteNonQuery();
                connection.Close();
            }
        }

        internal void CreateNewUser(User user)
        {
            using SQLiteConnection connection = new(ConnectionStr);
            string createUser = "INSERT INTO [users] ([name], [password]) VALUES (@name, @password)";
            SQLiteCommand createUserCommand = new(createUser, connection);
            connection.Open();
            createUserCommand.Parameters.AddWithValue("@name", user.Name);
            createUserCommand.Parameters.AddWithValue("@password", user.Password);
            createUserCommand.ExecuteNonQuery();
            connection.Close();
        }

        internal bool UserExist(string login)
        {
            using SQLiteConnection connection = new(ConnectionStr);
            SQLiteCommand userExist = new()
            {
                Connection = connection,
                CommandText = $"SELECT EXISTS(SELECT * FROM [users] WHERE [name] == '{login}')"
            };
            connection.Open();
            var exist = (long)userExist.ExecuteScalar() == 1;
            connection.Close();
            return exist;
        }

        internal bool UserLoginDataCorrect(string login, string? password)
        {
            var result = false;
            using SQLiteConnection connection = new(ConnectionStr);
            SQLiteCommand readUser = new()
            {
                CommandText = $"SELECT * FROM [users] WHERE [name] = '{login}' LIMIT 1",
                Connection = connection,
            };
            connection.Open();
            var reader = readUser.ExecuteReader();
            if (reader.Read() && reader["password"].Equals(password))
            {
                result = true;
            }
            connection.Close();
            return result;
        }

    }
}

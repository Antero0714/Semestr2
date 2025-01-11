using System.Data;
using System.Data.SqlClient;

namespace MyORMLibrary
{
    public class ORMContext<T> where T : class, new()
    {
        private readonly IDbConnection _dbConnection;

        public ORMContext(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Получение записи по ID
        public T GetById(int id, string tableName)
        {
            string query = $"SELECT * FROM {tableName} WHERE Id = @id";

            using (var command = _dbConnection.CreateCommand())
            {
                command.CommandText = query;

                var parameter = command.CreateParameter();
                parameter.ParameterName = "@id";
                parameter.Value = id;
                command.Parameters.Add(parameter);

                _dbConnection.Open();
                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return Map(reader);
                        }
                    }
                }
                finally
                {
                    _dbConnection.Close();
                }
            }

            return null;
        }

        // Создание записи
        public void Create(T entity, string tableName)
        {
            var query = GenerateInsertQuery(tableName);

            using (var command = _dbConnection.CreateCommand())
            {
                command.CommandText = query;

                var properties = typeof(T).GetProperties()
                    .Where(p => p.Name != "Id"); // Исключаем Id

                foreach (var property in properties)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = $"@{property.Name}";
                    parameter.Value = property.GetValue(entity) ?? DBNull.Value; // Если null, передаём DBNull.Value
                    command.Parameters.Add(parameter);
                }

                _dbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                finally
                {
                    _dbConnection.Close();
                }
            }
        }

        // Генерация SQL для INSERT
        private string GenerateInsertQuery(string tableName)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.Name != "Id") // Исключаем Id
                .ToArray();

            var columns = string.Join(", ", properties.Select(p => $"{p.Name}"));
            var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));
            return $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
        }

        // Обновление записи
        public void Update(int id, T entity, string tableName)
        {
            var properties = typeof(T).GetProperties();
            var setClause = string.Join(", ", properties.Select(p => $"{p.Name} = @{p.Name}"));

            string query = $"UPDATE {tableName} SET {setClause} WHERE Id = @id";

            using (var command = _dbConnection.CreateCommand())
            {
                command.CommandText = query;

                foreach (var property in properties)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = $"@{property.Name}";
                    parameter.Value = property.GetValue(entity) ?? DBNull.Value;
                    command.Parameters.Add(parameter);
                }

                var idParameter = command.CreateParameter();
                idParameter.ParameterName = "@id";
                idParameter.Value = id;
                command.Parameters.Add(idParameter);

                _dbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                finally
                {
                    _dbConnection.Close();
                }
            }
        }

        // Удаление записи
        public void Delete(int id, string tableName)
        {
            string query = $"DELETE FROM {tableName} WHERE Id = @id";

            using (var command = _dbConnection.CreateCommand())
            {
                command.CommandText = query;

                var parameter = command.CreateParameter();
                parameter.ParameterName = "@id";
                parameter.Value = id;
                command.Parameters.Add(parameter);

                _dbConnection.Open();
                try
                {
                    command.ExecuteNonQuery();
                }
                finally
                {
                    _dbConnection.Close();
                }
            }
        }

        // Получение всех записей
        public List<T> GetByAll(string tableName)
        {
            var resultList = new List<T>();

            using (var command = _dbConnection.CreateCommand())
            {
                command.CommandText = $"SELECT * FROM {tableName}";

                _dbConnection.Open();
                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultList.Add(Map(reader));
                        }
                    }
                }
                finally
                {
                    _dbConnection.Close();
                }
            }

            return resultList;
        }

        // Маппинг данных из базы в объект
        private T Map(IDataReader reader)
        {
            var entity = new T();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (reader[property.Name] != DBNull.Value)
                {
                    property.SetValue(entity, reader[property.Name]);
                }
            }

            return entity;
        }
    }
}

using Dapper;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace AircraftCarrierSlotSolverKai.Models
{
    public class SQLRecords<T>
    {
        protected const string DataSource = @"solver.db";

        public ObservableCollection<T> Records { get; set; } = new ObservableCollection<T>();

        protected static SQLiteConnection GetConnection()
        {
            var config = new SQLiteConnectionStringBuilder()
            {
                DataSource = DataSource
            };
            return new SQLiteConnection(config.ToString());
        }

        protected SQLRecords()
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            using (var connection = GetConnection())
            {
                connection.Open();
                foreach (var record in connection.Query<T>($"select * from {typeof(T).Name.ToLower()}"))
                {
                    Records.Add(record);
                }
            }
        }
    }
}
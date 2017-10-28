using Dapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;

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

        protected string TableName => typeof(T).Name.ToLower();

        protected SQLRecords(Func<IEnumerable<T>, IEnumerable<T>> func = null)
        {
            DefaultTypeMap.MatchNamesWithUnderscores = true;

            using (var connection = GetConnection())
            {
                connection.Open();

                var query = connection.Query<T>($"select * from {TableName}");

                var result = func == null ? query : func(query);

                foreach (var record in result)
                {
                    Records.Add(record);
                }
            }
        }
    }
}
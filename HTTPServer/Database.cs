using System.Data;
using LinqToDB.Data;
using LinqToDB.DataProvider;

namespace HTTPServer
{
    /**
     * Класс, который нужен для предоставления прочим классам подключения к базе данных.
     * Параметры подключения передаются в файле Startup.cs.
     */
    public class Database : DataConnection
    {
        public Database(IDataProvider provider, IDbConnection connection)
            : base(provider, connection)
        {
        }
    }
}

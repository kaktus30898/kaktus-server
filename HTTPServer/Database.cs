using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.DataLoader;
using HTTPServer.Model;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.Mapping;

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

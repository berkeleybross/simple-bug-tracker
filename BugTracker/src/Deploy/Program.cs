
using System.Linq;
using Milvus.Api.Deploy.PgSqlDatabase;

namespace Deploy
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Contains("--drop"))
            {
                PgDatabaseUpgrader.DropDatabase(args[0]);
            }

            PgDatabaseUpgrader.CreateAndUpgradeDatabase(args[0]);
        }
    }
}

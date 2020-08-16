// <copyright file="PgDatabaseUpgrader.cs" company="Content Catalyst Limited">
//     Copyright (c) Content Catalyst Limited. All rights reserved.
// </copyright>

using Deploy.PgSqlDatabase;

namespace Milvus.Api.Deploy.PgSqlDatabase
{
    using System;
    using System.Linq;
    using Dapper;
    using DbUp;
    using Npgsql;

    public static class PgDatabaseUpgrader
    {
        public static void DropDatabase(string connectionString)
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            var database = builder.Database;
            builder.Database = "postgres";

            using var con = OpenDatabase(builder.ToString());

            foreach (var databaseName in con.Query<string>("SELECT datname FROM pg_database")
                .Where(d => d == database))
            {
                con.Execute(
                    $@"
SELECT pg_terminate_backend(pid)
FROM pg_stat_activity
WHERE datname = @databaseName;

DROP DATABASE {databaseName};",
                    new {databaseName});
            }

            foreach (var roleName in con.Query<string>("SELECT rolname FROM pg_roles")
                .Where(r => r.StartsWith(database)))
            {
                con.Execute($@"DROP ROLE {roleName};");
            }
        }

        public static void CreateAndUpgradeDatabase(string connectionDetails)
        {
            EnsureDatabaseExists(connectionDetails);
            var builder = new NpgsqlConnectionStringBuilder(connectionDetails);

            var connectionString = connectionDetails;

            var upgrader = DeployChanges.To.PostgresqlDatabase(connectionString)
                .WithScripts(new PgScriptProvider())
                .WithoutTransaction()
                .WithVariable("databaseName", builder.Database)
                .Build();

            var result = upgrader.PerformUpgrade();
            if (!result.Successful)
            {
                throw new Exception("Could not upgrade database", result.Error);
            }
        }

        private static void EnsureDatabaseExists(string connectionDetails)
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionDetails);

            var database = builder.Database;
            var password = builder.Password;

            builder.Database = "postgres";

            using var connection = OpenDatabase(builder.ToString());

            var exists = connection.ExecuteScalar<bool?>("SELECT true FROM pg_database WHERE datname = @databaseName", new { databaseName = database });
            if (exists != true)
            {
                connection.Execute($@"CREATE DATABASE {database};", commandTimeout: 120);
            }

            connection.Execute(
                $@"
DO
$do$
BEGIN
    IF NOT EXISTS (SELECT * FROM pg_catalog.pg_roles WHERE rolname = '{database}_app_role') THEN
        CREATE ROLE {database}_app_role;
    END IF;

    IF NOT EXISTS (SELECT * FROM pg_catalog.pg_roles WHERE rolname = '{database}_read_role') THEN
        CREATE ROLE {database}_read_role;
    END IF;

    IF NOT EXISTS (SELECT * FROM pg_catalog.pg_roles WHERE rolname = '{database}_app') THEN
        CREATE ROLE {database}_app WITH LOGIN PASSWORD '{password}';
    ELSE
        ALTER ROLE {database}_app WITH PASSWORD '{password}';  
    END IF;

    IF NOT EXISTS (SELECT * FROM pg_catalog.pg_roles WHERE rolname = '{database}_read') THEN
        CREATE ROLE {database}_read WITH LOGIN PASSWORD '{password}';
    ELSE
        ALTER ROLE {database}_read WITH PASSWORD '{password}';  
    END IF;

    GRANT {database}_app_role TO {database}_app;
    GRANT {database}_read_role TO {database}_read;
END
$do$;");
        }

        private static NpgsqlConnection OpenDatabase(string connectionString)
        {
            NpgsqlConnection connection = null;
            try
            {
                connection = new NpgsqlConnection(connectionString);
                connection.Open();

                return connection;
            }
            catch
            {
                connection?.Dispose();
                throw;
            }
        }
    }
}

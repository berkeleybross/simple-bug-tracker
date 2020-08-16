// <copyright file="PgScriptProvider.cs" company="Content Catalyst Limited">
//     Copyright (c) Content Catalyst Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using DbUp.Engine;
using DbUp.Engine.Transactions;

namespace Deploy.PgSqlDatabase
{
    public class PgScriptProvider
        : IScriptProvider
    {
        private const string NamespacePrefix = "Deploy.PgSqlDatabase.";

        public IEnumerable<SqlScript> GetScripts(IConnectionManager connectionManager)
        {
            var assembly = typeof(PgScriptProvider).Assembly;

            return assembly.GetManifestResourceNames()
                           .Where(s => s.EndsWith(".pgsql", StringComparison.InvariantCultureIgnoreCase))
                           .Where(s => s.StartsWith(NamespacePrefix))
                           .Select(s => LoadScript(assembly, s, s[NamespacePrefix.Length..^".pgsql".Length]))
                           .OrderBy(s => s.Name)
                           .ToList();
        }

        internal static SqlScript LoadScript(Assembly assembly, string resourceName, string scriptName)
        {
            using var resourceStream = assembly.GetManifestResourceStream(resourceName);
            if (resourceStream == null)
            {
                throw new InvalidOperationException("Resource stream " + resourceName + " is null");
            }

            resourceStream.Position = 0;

            using var streamReader = new StreamReader(resourceStream, Encoding.UTF8, false);
            return new SqlScript(scriptName, streamReader.ReadToEnd());
        }
    }
}

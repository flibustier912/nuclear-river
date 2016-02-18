﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

using NuClear.CustomerIntelligence.Replication.StateInitialization.Tests.Identitites.Connections;
using NuClear.CustomerIntelligence.Storage.Identitites.Connections;
using NuClear.Storage.API.ConnectionStrings;

namespace NuClear.CustomerIntelligence.Replication.StateInitialization.Tests
{
    public sealed class RunnerConnectionStringSettings : ConnectionStringSettingsAspect
    {
        private static readonly string DatabaseNamePostfix = Guid.NewGuid().ToString("N");

        public RunnerConnectionStringSettings()
            : base(CreateConnectionStringMappings(GetTestAssemblyConnectionStrings()))
        {
        }

        private static ConnectionStringSettingsCollection GetTestAssemblyConnectionStrings()
        {
            var assemblyLocation = typeof(RunnerConnectionStringSettings).Assembly.Location;
            return ConfigurationManager.OpenExeConfiguration(assemblyLocation).ConnectionStrings.ConnectionStrings;
        }

        private static IReadOnlyDictionary<IConnectionStringIdentity, string> CreateConnectionStringMappings(ConnectionStringSettingsCollection configuration) =>
            new Dictionary<IConnectionStringIdentity, string>
            {
                { ErmConnectionStringIdentity.Instance, MakeUniqueSqlConnectionString(configuration[ConnectionStringName.Erm].ConnectionString) },
                { FactsConnectionStringIdentity.Instance, MakeUniqueSqlConnectionString(configuration[ConnectionStringName.Facts].ConnectionString) },
                { CustomerIntelligenceConnectionStringIdentity.Instance, MakeUniqueSqlConnectionString(configuration[ConnectionStringName.CustomerIntelligence].ConnectionString) },
                { BitConnectionStringIdentity.Instance, MakeUniqueSqlConnectionString(configuration[ConnectionStringName.Bit].ConnectionString) },
                { StatisticsConnectionStringIdentity.Instance, MakeUniqueSqlConnectionString(configuration[ConnectionStringName.Statistics].ConnectionString) },

                { ErmMasterConnectionStringIdentity.Instance, configuration[ConnectionStringName.ErmMaster].ConnectionString },
            };

        private static string MakeUniqueSqlConnectionString(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            builder.InitialCatalog = builder.InitialCatalog + DatabaseNamePostfix;
            return builder.ToString();
        }
    }
}
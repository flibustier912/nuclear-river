﻿using System;
using System.Collections.Generic;
using System.Linq;

using NuClear.AdvancedSearch.Common.Metadata.Identities;
using NuClear.CustomerIntelligence.Domain;
using NuClear.CustomerIntelligence.Domain.Contexts;
using NuClear.CustomerIntelligence.Storage;
using NuClear.Metamodeling.Elements;
using NuClear.Metamodeling.Provider.Sources;
using NuClear.Replication.Bulk.Metadata;

namespace NuClear.CustomerIntelligence.StateInitialization
{
    public sealed class BulkReplicationMetadataSource : MetadataSourceBase<BulkReplicationMetadataKindIdentity>
    {
        private static readonly IReadOnlyDictionary<Uri, IMetadataElement> Elements =
            new BulkReplicationMetadataElement[]
            {
                BulkReplicationMetadataElement.Config
                                              .CommandlineKey("-fact")
                                              .From(ConnectionString.Erm, Schema.Erm, EntityTypeMap.CreateErmContext())
                                              .To(ConnectionString.Facts, Schema.Facts)
                                              .UsingMetadataOfKind<ReplicationMetadataIdentity>(ReplicationMetadataName.Facts),

                BulkReplicationMetadataElement.Config
                                              .CommandlineKey("-ci")
                                              .From(ConnectionString.Facts, Schema.Facts, EntityTypeMap.CreateFactsContext())
                                              .To(ConnectionString.CustomerIntelligence, Schema.CustomerIntelligence)
                                              .UsingMetadataOfKind<ReplicationMetadataIdentity>(ReplicationMetadataName.Aggregates),

                BulkReplicationMetadataElement.Config
                                              .CommandlineKey("-statistics")
                                              .From(ConnectionString.Facts, Schema.Facts, EntityTypeMap.CreateFactsContext())
                                              .From(ConnectionString.Facts, Schema.Bit, EntityTypeMap.CreateBitFactsContext())
                                              .To(ConnectionString.CustomerIntelligence, Schema.CustomerIntelligence)
                                              .UsingMetadataOfKind<StatisticsRecalculationMetadataIdentity>()
            }.ToDictionary(x => x.Identity.Id, x => (IMetadataElement)x);

        public override IReadOnlyDictionary<Uri, IMetadataElement> Metadata => Elements;
    }
}
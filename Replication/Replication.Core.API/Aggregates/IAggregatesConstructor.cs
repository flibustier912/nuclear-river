﻿using System.Collections.Generic;

using NuClear.River.Common.Metadata.Model.Operations;

namespace NuClear.Replication.Core.API.Aggregates
{
    public interface IAggregatesConstructor
    {
        void Execute(IEnumerable<AggregateOperation> commands);
    }
}
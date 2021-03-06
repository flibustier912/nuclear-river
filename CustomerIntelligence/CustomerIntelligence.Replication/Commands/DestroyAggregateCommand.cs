﻿using System;

using NuClear.Replication.Core.Commands;

namespace NuClear.CustomerIntelligence.Replication.Commands
{
    public class DestroyAggregateCommand : IAggregateCommand
    {
        public DestroyAggregateCommand(Type aggregateRootType, long aggregateRootId)
        {
            AggregateRootType = aggregateRootType;
            AggregateRootId = aggregateRootId;
        }

        public Type AggregateRootType { get; }

        public long AggregateRootId { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((DestroyAggregateCommand)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((AggregateRootType?.GetHashCode() ?? 0) * 397) ^ AggregateRootId.GetHashCode();
            }
        }

        private bool Equals(DestroyAggregateCommand other)
        {
            return AggregateRootType == other.AggregateRootType && AggregateRootId == other.AggregateRootId;
        }
    }
}
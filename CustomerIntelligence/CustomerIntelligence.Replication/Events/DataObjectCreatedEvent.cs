﻿using System;

using NuClear.Replication.Core;

namespace NuClear.CustomerIntelligence.Replication.Events
{
    public class DataObjectCreatedEvent : IEvent
    {
        public DataObjectCreatedEvent(Type dataObjectType, long dataObjectId)
            : this(dataObjectType, dataObjectId, DateTime.Now)
        {
        }

        public DataObjectCreatedEvent(Type dataObjectType, long dataObjectId, DateTime eventTime)
        {
            DataObjectType = dataObjectType;
            DataObjectId = dataObjectId;
            Time = eventTime;
        }

        public Type DataObjectType { get; }
        public long DataObjectId { get; }
        public DateTime Time { get; }

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

            return Equals((DataObjectCreatedEvent)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((DataObjectType?.GetHashCode() ?? 0) * 397) ^ DataObjectId.GetHashCode();
            }
        }

        private bool Equals(DataObjectCreatedEvent other)
        {
            return DataObjectType == other.DataObjectType && DataObjectId == other.DataObjectId;
        }
    }
}
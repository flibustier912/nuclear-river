using NuClear.Telemetry;

namespace NuClear.Replication.OperationsProcessing.Performance
{
    public sealed class AggregateOperationProcessedCountIdentity : TelemetryIdentityBase<AggregateOperationProcessedCountIdentity>
    {
        public override int Id
        {
            get { return 0; }
        }

        public override string Description
        {
            get { return "���������� ������������ �������� �� ����� final"; }
        }
    }
}
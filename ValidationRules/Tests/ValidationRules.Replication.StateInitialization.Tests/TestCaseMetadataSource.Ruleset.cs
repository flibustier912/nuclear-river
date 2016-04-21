﻿using NuClear.DataTest.Metamodel.Dsl;

namespace NuClear.ValidationRules.Replication.StateInitialization.Tests
{
    using Facts = Domain.Model.Facts;
    using Aggs = Domain.Model.Aggregates;

    public sealed partial class TestCaseMetadataSource
    {
        // ReSharper disable once UnusedMember.Local
        private static ArrangeMetadataElement RulesetRulesTests
        => ArrangeMetadataElement.Config
        .Name(nameof(RulesetRulesTests))
        .Fact(
            new Facts::RulesetRule { Id = 1, Priority = 1 },
            new Facts::RulesetRule { Id = 2, PrincipalPositionId = 1, DependentPositionId = 2, ObjectBindingType = 3, Priority = 2 }
            )
        .Aggregate(
            new Aggs::RulesetRule { RulesetId = 2, PrincipalPositionId = 1, DependentPositionId = 2, ObjectBindingType = 3 },
            new Aggs::Ruleset { Id = 2 }
            );
    }
}
﻿// ReSharper disable once CheckNamespace
namespace NuClear.AdvancedSearch.EntityDataModel.Metadata
{
    partial class AdvancedSearchMetadataSource
    {
        private readonly BoundedContextElement _customerIntelligence =
            BoundedContextElement.Config
                .Name("CustomerIntelligence")
                .Elements(
                    EntityElement.Config
                        .Name("Firm")
                        .Property(EntityPropertyElement.Config.Name("Id").OfType(EntityPropertyType.Int64).NotNull())
                        .Property(EntityPropertyElement.Config.Name("OrganizationUnitId").OfType(EntityPropertyType.Int64))
                        .Property(EntityPropertyElement.Config.Name("TerritoryId").OfType(EntityPropertyType.Int64))
                        .Property(EntityPropertyElement.Config.Name("CreatedOn").OfType(EntityPropertyType.DateTime))
                        .Property(EntityPropertyElement.Config.Name("LastQualifiedOn").OfType(EntityPropertyType.DateTime))
                        .Property(EntityPropertyElement.Config.Name("LastDistributedOn").OfType(EntityPropertyType.DateTime))
                        .Property(EntityPropertyElement.Config.Name("HasWebsite").OfType(EntityPropertyType.Boolean))
                        .Property(EntityPropertyElement.Config.Name("HasPhone").OfType(EntityPropertyType.Boolean))
                        .Property(EntityPropertyElement.Config.Name("CategoryGroup").OfType(EntityPropertyType.Byte))
                        .Property(EntityPropertyElement.Config.Name("AddressCount").OfType(EntityPropertyType.Int32))
                        .IdentifyBy("Id")
                        .Relation(EntityRelationElement.Config
                            .Name("Categories")
                            .DirectTo(
                                EntityElement.Config.Name("Category")
                                    .Property(EntityPropertyElement.Config.Name("Id").OfType(EntityPropertyType.Int64).NotNull())
                                    .Property(EntityPropertyElement.Config.Name("Name").OfType(EntityPropertyType.String))
                                    .Property(EntityPropertyElement.Config.Name("CategoryGroup").OfType(EntityPropertyType.Byte))
                                .IdentifyBy("Id")
                            )
                            .AsMany())
                        .Relation(EntityRelationElement.Config
                            .Name("Client")
                            .DirectTo(
                                EntityElement.Config.Name("Client")
                                    .Property(EntityPropertyElement.Config.Name("Id").OfType(EntityPropertyType.Int64))
                                    .Property(EntityPropertyElement.Config.Name("CategoryGroup").OfType(EntityPropertyType.Byte))
                                    .Relation(
                                        EntityRelationElement.Config
                                            .Name("Accounts")
                                            .DirectTo(
                                                EntityElement.Config
                                                    .Name("Account")
                                                    .Property(EntityPropertyElement.Config.Name("Balance").OfType(EntityPropertyType.Decimal))
                                            )
                                            .AsMany()
                                    )
                                    .Relation(
                                        EntityRelationElement.Config
                                            .Name("Contacts")
                                            .DirectTo(
                                                EntityElement.Config.Name("Contact")
                                                    .Property(EntityPropertyElement.Config.Name("Id").OfType(EntityPropertyType.Int64))
                                                    .Property(EntityPropertyElement.Config.Name("Role")
                                                        .UsingEnum()
                                                        .WithMember("Employee", 200000)
                                                        .WithMember("InfluenceDecisions", 200001)
                                                        .WithMember("MakingDecisions", 200002)
                                                        )
                                                    .Property(EntityPropertyElement.Config.Name("IsFired").OfType(EntityPropertyType.Boolean))
                                            )
                                            .AsMany()
                                    )
                            )
                            .AsOneOptionally())
                );
    }
}
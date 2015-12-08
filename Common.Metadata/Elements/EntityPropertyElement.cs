﻿using System;
using System.Collections.Generic;

using NuClear.AdvancedSearch.Common.Metadata.Builders;
using NuClear.AdvancedSearch.Common.Metadata.Features;
using NuClear.Metamodeling.Elements.Aspects.Features;
using NuClear.Metamodeling.Elements.Identities;

namespace NuClear.AdvancedSearch.Common.Metadata.Elements
{
    public sealed class EntityPropertyElement : BaseMetadataElement<EntityPropertyElement, EntityPropertyElementBuilder>
    {
        private readonly IStructuralModelTypeElement _typeElement;

        internal EntityPropertyElement(IMetadataElementIdentity identity, IStructuralModelTypeElement typeElement, IEnumerable<IMetadataFeature> features)
            : base(identity, features)
        {
            if (typeElement == null)
            {
                throw new ArgumentNullException("typeElement");
            }
            _typeElement = typeElement;
        }

        public IStructuralModelTypeElement PropertyType
        {
            get
            {
                return _typeElement;
            }
        }

        public bool IsNullable
        {
            get
            {
                return ResolveFeature<EntityPropertyNullableFeature, bool>(f => f.IsNullable);
            }
        }
   }
}
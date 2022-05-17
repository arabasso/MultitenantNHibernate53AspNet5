using System;
using Microsoft.AspNetCore.Identity;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MultitenantNHibernate53AspNet5.Models.Mappings
{
    public class IdentityRoleMapping :
        IdentityRoleMapping<long>
    {
        public IdentityRoleMapping()
        {
        }

        public IdentityRoleMapping(
            IGeneratorDef generatorDef) :
            base(generatorDef)
        {
        }

        public IdentityRoleMapping(
            string tableName,
            IGeneratorDef generatorDef) :
            base(tableName, generatorDef)
        {
        }
    }

    public class IdentityRoleMapping<TKey>
        : IdentityRoleMapping<IdentityRole<TKey>, TKey>
        where TKey : IEquatable<TKey>
    {
        public IdentityRoleMapping()
        {
        }

        public IdentityRoleMapping(
            IGeneratorDef generatorDef) :
            base(generatorDef)
        {
        }

        public IdentityRoleMapping(
            string tableName,
            IGeneratorDef generatorDef) :
            base(tableName, generatorDef)
        {
        }
    }

    public class IdentityRoleMapping<TRole, TKey> :
        ClassMapping<TRole>
        where TKey : IEquatable<TKey>
        where TRole : IdentityRole<TKey>
    {
        public IdentityRoleMapping() :
            this("aspnetroles", Generators.Identity)
        {
        }

        public IdentityRoleMapping(
            IGeneratorDef generatorDef) :
            this("aspnetroles", generatorDef)
        {
        }

        public IdentityRoleMapping(
            string tableName,
            IGeneratorDef generatorDef)
        {
            Table(tableName);
            Id(e => e.Id, id =>
            {
                id.Generator(generatorDef);
            });
            Property(e => e.Name, prop =>
            {
                prop.Length(64);
                prop.NotNullable(true);
                prop.Unique(true);
            });
            Property(e => e.NormalizedName, prop =>
            {
                prop.Length(64);
                prop.NotNullable(true);
                prop.Unique(true);
            });
            Property(e => e.ConcurrencyStamp, prop =>
            {
                prop.Length(36);
                prop.NotNullable(false);
            });
        }
    }
}

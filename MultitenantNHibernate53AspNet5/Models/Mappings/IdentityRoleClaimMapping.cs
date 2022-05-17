using System;
using Microsoft.AspNetCore.Identity;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MultitenantNHibernate53AspNet5.Models.Mappings
{
    public class IdentityRoleClaimMapping :
        IdentityRoleClaimMapping<long>
    {
        public IdentityRoleClaimMapping()
        {
        }

        public IdentityRoleClaimMapping(
            IGeneratorDef generatorDef) :
            base(generatorDef)
        {
        }

        public IdentityRoleClaimMapping(
            string tableName,
            IGeneratorDef generatorDef) :
            base(tableName, generatorDef)
        {
        }
    }

    public class IdentityRoleClaimMapping<TKey> :
        IdentityRoleClaimMapping<IdentityRoleClaim<TKey>, TKey>
        where TKey : IEquatable<TKey>
    {
        public IdentityRoleClaimMapping()
        {
        }

        public IdentityRoleClaimMapping(
            IGeneratorDef generatorDef) :
            base(generatorDef)
        {
        }

        public IdentityRoleClaimMapping(
            string tableName,
            IGeneratorDef generatorDef) :
            base(tableName, generatorDef)
        {
        }
    }

    public class IdentityRoleClaimMapping<TRoleClaim, TKey> :
        ClassMapping<TRoleClaim>
        where TKey : IEquatable<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
    {
        public IdentityRoleClaimMapping() :
            this("aspnetroleclaims", Generators.Identity)
        {
        }

        public IdentityRoleClaimMapping(
            IGeneratorDef generatorDef) :
            this("aspnetroleclaims", generatorDef)
        {
        }

        public IdentityRoleClaimMapping(
            string tableName,
            IGeneratorDef generatorDef)
        {
            Table(tableName);
            Id(e => e.Id, id =>
            {
                id.Generator(generatorDef);
            });
            Property(e => e.ClaimType, prop =>
            {
                prop.Length(1024);
                prop.NotNullable(true);
            });
            Property(e => e.ClaimValue, prop =>
            {
                prop.Length(1024);
                prop.NotNullable(true);
            });
            Property(e => e.RoleId, prop =>
            {
                prop.NotNullable(true);
            });
        }
    }
}

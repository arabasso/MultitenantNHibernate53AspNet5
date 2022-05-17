using System;
using Microsoft.AspNetCore.Identity;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MultitenantNHibernate53AspNet5.Models.Mappings
{
    public class IdentityUserClaimMapping :
        IdentityUserClaimMapping<long>
    {
        public IdentityUserClaimMapping()
        {
        }

        public IdentityUserClaimMapping(
            IGeneratorDef generatorDef) :
            base(generatorDef)
        {
        }

        public IdentityUserClaimMapping(
            string tableName,
            IGeneratorDef generatorDef) :
            base(tableName, generatorDef)
        {
        }
    }

    public class IdentityUserClaimMapping<TKey> :
        IdentityUserClaimMapping<IdentityUserClaim<TKey>, TKey>
        where TKey : IEquatable<TKey>
    {
        public IdentityUserClaimMapping()
        {
        }

        public IdentityUserClaimMapping(
            IGeneratorDef generatorDef) :
            base(generatorDef)
        {
        }

        public IdentityUserClaimMapping(
            string tableName,
            IGeneratorDef generatorDef) :
            base(tableName, generatorDef)
        {
        }
    }

    public class IdentityUserClaimMapping<TUserClaim, TKey> :
        ClassMapping<TUserClaim>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
    {
        public IdentityUserClaimMapping() :
            this("aspnetuserclaims", Generators.Identity)
        {
        }

        public IdentityUserClaimMapping(
            IGeneratorDef generatorDef) :
            this("aspnetuserclaims", generatorDef)
        {
        }

        public IdentityUserClaimMapping(
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
            Property(e => e.UserId, prop =>
            {
                prop.Length(32);
                prop.NotNullable(true);
            });
        }

    }
}

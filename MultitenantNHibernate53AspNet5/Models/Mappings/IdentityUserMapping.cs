using System;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace MultitenantNHibernate53AspNet5.Models.Mappings
{
    public class IdentityUserMapping :
        IdentityUserMapping<long>
    {
        public IdentityUserMapping()
        {
        }

        public IdentityUserMapping(
            IGeneratorDef generatorDef) :
            base(generatorDef)
        {
        }

        public IdentityUserMapping(
            string tableName,
            IGeneratorDef generatorDef) :
            base(tableName, generatorDef)
        {
        }
    }

    public class IdentityUserMapping<TKey> :
        IdentityUserMapping<Microsoft.AspNetCore.Identity.IdentityUser<TKey>, TKey>
        where TKey : IEquatable<TKey>
    {
        public IdentityUserMapping()
        {
        }

        public IdentityUserMapping(
            IGeneratorDef generatorDef) :
            base(generatorDef)
        {
        }

        public IdentityUserMapping(
            string tableName,
            IGeneratorDef generatorDef) :
            base(tableName, generatorDef)
        {
        }
    }

    public class IdentityUserMapping<TUser, TKey> :
        ClassMapping<TUser>
        where TUser : Microsoft.AspNetCore.Identity.IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public IdentityUserMapping() :
            this("aspnetusers", Generators.Identity)
        {
        }

        public IdentityUserMapping(
            IGeneratorDef generatorDef) :
            this("aspnetusers", generatorDef)
        {
        }

        public IdentityUserMapping(
            string tableName,
            IGeneratorDef generatorDef)
        {
            Table(tableName);
            Id(e => e.Id, id =>
            {
                id.Generator(generatorDef);
            });
            Property(e => e.UserName, prop =>
            {
                prop.Length(64);
                prop.NotNullable(true);
                prop.Unique(true);
            });
            Property(e => e.NormalizedUserName, prop =>
            {
                prop.Length(64);
                prop.NotNullable(true);
                prop.Unique(true);
            });
            Property(e => e.Email, prop =>
            {
                prop.Length(256);
                prop.NotNullable(true);
            });
            Property(e => e.NormalizedEmail, prop =>
            {
                prop.Length(256);
                prop.NotNullable(true);
            });
            Property(e => e.EmailConfirmed, prop =>
            {
                prop.NotNullable(true);
            });
            Property(e => e.PhoneNumber, prop =>
            {
                prop.Length(128);
                prop.NotNullable(false);
            });
            Property(e => e.PhoneNumberConfirmed, prop =>
            {
                prop.NotNullable(true);
            });
            Property(e => e.LockoutEnabled, prop =>
            {
                prop.NotNullable(true);
            });
            Property(e => e.AccessFailedCount, prop =>
            {
                prop.NotNullable(true);
            });
            Property(e => e.ConcurrencyStamp, prop =>
            {
                prop.Length(36);
                prop.NotNullable(false);
            });
            Property(e => e.PasswordHash, prop =>
            {
                prop.Length(256);
                prop.NotNullable(false);
            });
            Property(e => e.TwoFactorEnabled, prop =>
            {
                prop.NotNullable(true);
            });
            Property(e => e.SecurityStamp, prop =>
            {
                prop.Length(64);
                prop.NotNullable(false);
            });
        }
    }
}

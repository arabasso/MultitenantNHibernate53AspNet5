using System;
using NHibernate.Mapping.ByCode.Conformist;

namespace MultitenantNHibernate53AspNet5.Models.Mappings
{
    public class IdentityUserLoginMapping :
        IdentityUserLoginMapping<long>
    {
        public IdentityUserLoginMapping()
        {
        }

        public IdentityUserLoginMapping(
            string tableName) :
            base(tableName)
        {
        }
    }

    public class IdentityUserLoginMapping<TKey> :
        IdentityUserLoginMapping<IdentityUserLogin<TKey>, TKey>
        where TKey : IEquatable<TKey>
    {
        public IdentityUserLoginMapping()
        {
        }

        public IdentityUserLoginMapping(
            string tableName) :
            base(tableName)
        {
        }
    }

    public class IdentityUserLoginMapping<TUserLogin, TKey> :
        ClassMapping<TUserLogin>
        where TKey : IEquatable<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
    {
        public IdentityUserLoginMapping() :
            this("aspnetuserlogins")
        {
        }

        public IdentityUserLoginMapping(
            string tableName)
        {
            Table(tableName);
            ComposedId(id =>
            {
                id.Property(e => e.LoginProvider, prop =>
                {
                    prop.Length(32);
                });
                id.Property(e => e.ProviderKey, prop =>
                {
                    prop.Length(32);
                });
            });
            Property(e => e.ProviderDisplayName, prop =>
            {
                prop.Length(32);
                prop.NotNullable(true);
            });
            Property(e => e.UserId, prop =>
            {
                prop.NotNullable(true);
            });
        }

    }
}

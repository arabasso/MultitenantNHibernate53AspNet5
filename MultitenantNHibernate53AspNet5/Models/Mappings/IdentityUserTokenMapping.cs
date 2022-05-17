using System;
using NHibernate.Mapping.ByCode.Conformist;

namespace MultitenantNHibernate53AspNet5.Models.Mappings
{
    public class IdentityUserTokenMapping :
        IdentityUserTokenMapping<long>
    {
        public IdentityUserTokenMapping()
        {
        }

        public IdentityUserTokenMapping(
            string tableName) :
            base(tableName)
        {
        }
    }

    public class IdentityUserTokenMapping<TKey> :
        IdentityUserTokenMapping<IdentityUserToken<TKey>, TKey>
        where TKey : IEquatable<TKey>
    {
        public IdentityUserTokenMapping()
        {
        }

        public IdentityUserTokenMapping(
            string tableName) :
            base(tableName)
        {
        }
    }

    public class IdentityUserTokenMapping<TUserToken, TKey> :
        ClassMapping<TUserToken>
        where TKey : IEquatable<TKey>
        where TUserToken : IdentityUserToken<TKey>
    {
        public IdentityUserTokenMapping() :
            this("aspnetusertokens")
        {
        }

        public IdentityUserTokenMapping(
            string tableName)
        {
            Table(tableName);
            ComposedId(id =>
            {
                id.Property(e => e.UserId);
                id.Property(e => e.LoginProvider, prop =>
                {
                    prop.Length(32);
                });
                id.Property(e => e.Name, prop =>
                {
                    prop.Length(32);
                });
            });
            Property(e => e.Value, prop =>
            {
                prop.Length(256);
                prop.NotNullable(true);
                prop.Column("Value1");
            });
        }
    }
}

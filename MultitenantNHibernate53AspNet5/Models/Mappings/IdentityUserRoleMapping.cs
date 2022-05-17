using System;
using NHibernate.Mapping.ByCode.Conformist;

namespace MultitenantNHibernate53AspNet5.Models.Mappings
{
    public class IdentityUserRoleMapping :
        IdentityUserRoleMapping<long>
    {
        public IdentityUserRoleMapping()
        {
        }

        public IdentityUserRoleMapping(
            string tableName) :
            base(tableName)
        {
        }
    }

    public class IdentityUserRoleMapping<TKey> :
        IdentityUserRoleMapping<IdentityUserRole<TKey>, TKey>
        where TKey : IEquatable<TKey>
    {
        public IdentityUserRoleMapping()
        {
        }

        public IdentityUserRoleMapping(
            string tableName) :
            base(tableName)
        {
        }
    }

    public class IdentityUserRoleMapping<TUserRole, TKey> :
        ClassMapping<TUserRole>
        where TKey : IEquatable<TKey>
        where TUserRole : IdentityUserRole<TKey>
    {
        public IdentityUserRoleMapping() :
            this("aspnetuserroles")
        {
        }

        public IdentityUserRoleMapping(
            string tableName)
        {
            Table(tableName);
            ComposedId(id =>
            {
                id.Property(e => e.UserId);
                id.Property(e => e.RoleId);
            });
        }
    }
}

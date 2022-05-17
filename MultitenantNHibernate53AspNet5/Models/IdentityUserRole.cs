using System;

namespace MultitenantNHibernate53AspNet5.Models
{
    public class IdentityUserRole<TKey> :
        Microsoft.AspNetCore.Identity.IdentityUserRole<TKey>
        where TKey : IEquatable<TKey>
    {
        protected bool Equals(
            IdentityUserRole<TKey> other)
        {
            return Equals(RoleId, other.RoleId)
                && Equals(UserId, other.UserId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((IdentityUserRole<TKey>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 0;
                hashCode = RoleId.GetHashCode();
                hashCode = (hashCode * 397) ^ UserId.GetHashCode();
                return hashCode;
            }
        }
    }
}

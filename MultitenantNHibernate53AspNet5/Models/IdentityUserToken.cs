using System;

namespace MultitenantNHibernate53AspNet5.Models
{
    public class IdentityUserToken<TKey> :
        Microsoft.AspNetCore.Identity.IdentityUserToken<TKey>
        where TKey : IEquatable<TKey>
    {
        protected bool Equals(
            IdentityUserToken<TKey> other)
        {
            return Equals(UserId, other.UserId)
                && LoginProvider == other.LoginProvider
                && Name == other.Name;
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
            return Equals((IdentityUserToken<TKey>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 0;
                hashCode = UserId.GetHashCode();
                hashCode = (hashCode * 397) ^ LoginProvider.GetHashCode();
                hashCode = (hashCode * 397) ^ Name.GetHashCode();
                return hashCode;
            }
        }
    }
}

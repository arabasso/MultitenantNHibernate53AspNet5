using System;

namespace MultitenantNHibernate53AspNet5.Models
{
    public class IdentityUserLogin<TKey> :
        Microsoft.AspNetCore.Identity.IdentityUserLogin<TKey>
        where TKey : IEquatable<TKey>
    {
        protected bool Equals(IdentityUserLogin<TKey> other)
        {
            return LoginProvider == other.LoginProvider
                && ProviderKey == other.ProviderKey;
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
            return Equals((IdentityUserLogin<TKey>)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 0;
                hashCode = LoginProvider.GetHashCode();
                hashCode = (hashCode * 397) ^ ProviderKey.GetHashCode();
                return hashCode;
            }
        }
    }
}

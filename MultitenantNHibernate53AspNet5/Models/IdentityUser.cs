using System;

namespace MultitenantNHibernate53AspNet5.Models
{
    public class IdentityUser<TKey> :
        Microsoft.AspNetCore.Identity.IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        public virtual long? LockoutEndUnixTimeMilliseconds { get; set; }

        public override DateTimeOffset? LockoutEnd
        {
            get
            {
                if (!LockoutEndUnixTimeMilliseconds.HasValue)
                {
                    return null;
                }
                var offset = DateTimeOffset.FromUnixTimeMilliseconds(
                    LockoutEndUnixTimeMilliseconds.Value
                );
                return TimeZoneInfo.ConvertTime(offset, TimeZoneInfo.Local);
            }
            set
            {
                if (value.HasValue)
                {
                    LockoutEndUnixTimeMilliseconds = value.Value.ToUnixTimeMilliseconds();
                }
                else
                {
                    LockoutEndUnixTimeMilliseconds = null;
                }
            }
        }
    }
}

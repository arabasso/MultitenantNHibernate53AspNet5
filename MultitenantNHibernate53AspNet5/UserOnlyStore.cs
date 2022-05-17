using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;

namespace MultitenantNHibernate53AspNet5
{
    public class UserOnlyStore<TUser, TKey> :
        Microsoft.AspNetCore.Identity.UserStoreBase<TUser, TKey, Microsoft.AspNetCore.Identity.IdentityUserClaim<TKey>, Models.IdentityUserLogin<TKey>, Models.IdentityUserToken<TKey>>,
        Microsoft.AspNetCore.Identity.IProtectedUserStore<TUser>
        where TUser : Microsoft.AspNetCore.Identity.IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly ISession _session;

        public override IQueryable<TUser> Users => _session.Query<TUser>();

        private IQueryable<Microsoft.AspNetCore.Identity.IdentityUserClaim<TKey>> UserClaims => _session.Query<Microsoft.AspNetCore.Identity.IdentityUserClaim<TKey>>();

        private IQueryable<Models.IdentityUserLogin<TKey>> UserLogins => _session.Query<Models.IdentityUserLogin<TKey>>();

        private IQueryable<Models.IdentityUserToken<TKey>> UserTokens => _session.Query<Models.IdentityUserToken<TKey>>();


        public UserOnlyStore(
            ISession session,
            Microsoft.AspNetCore.Identity.IdentityErrorDescriber describer) :
            base(describer ?? new Microsoft.AspNetCore.Identity.IdentityErrorDescriber())
        {
            this._session = session ?? throw new ArgumentNullException(nameof(session));
        }

        public override async Task<Microsoft.AspNetCore.Identity.IdentityResult> CreateAsync(
            TUser user,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            await _session.SaveAsync(user, cancellationToken);
            await FlushChangesAsync(cancellationToken);
            return Microsoft.AspNetCore.Identity.IdentityResult.Success;
        }

        public override async Task<Microsoft.AspNetCore.Identity.IdentityResult> UpdateAsync(
            TUser user,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var exists = await Users.AnyAsync(
                u => u.Id.Equals(user.Id),
                cancellationToken
            );
            if (!exists)
            {
                return Microsoft.AspNetCore.Identity.IdentityResult.Failed(
                    new Microsoft.AspNetCore.Identity.IdentityError
                    {
                        Code = "UserNotExist",
                        Description = $"User with id {user.Id} does not exists!"
                    }
                );
            }
            user.ConcurrencyStamp = Guid.NewGuid().ToString("N");
            await _session.MergeAsync(user, cancellationToken);
            await FlushChangesAsync(cancellationToken);
            return Microsoft.AspNetCore.Identity.IdentityResult.Success;
        }

        public override async Task<Microsoft.AspNetCore.Identity.IdentityResult> DeleteAsync(
            TUser user,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            await _session.DeleteAsync(user, cancellationToken);
            await FlushChangesAsync(cancellationToken);
            return Microsoft.AspNetCore.Identity.IdentityResult.Success;
        }

        public override async Task<TUser> FindByIdAsync(
            string userId,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var id = ConvertIdFromString(userId);
            var user = await _session.GetAsync<TUser>(id, cancellationToken);
            return user;
        }

        public override async Task<TUser> FindByNameAsync(
            string normalizedUserName,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var user = await Users.FirstOrDefaultAsync(
                u => u.NormalizedUserName == normalizedUserName,
                cancellationToken
            );
            return user;
        }

        protected override async Task<TUser> FindUserAsync(
            TKey userId,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var user = await Users.FirstOrDefaultAsync(
                u => u.Id.Equals(userId),
                cancellationToken
            );
            return user;
        }

        protected override async Task<Models.IdentityUserLogin<TKey>> FindUserLoginAsync(
            TKey userId,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var userLogin = await UserLogins.FirstOrDefaultAsync(
                ul => ul.UserId.Equals(userId) && ul.LoginProvider == loginProvider
                    && ul.ProviderKey == providerKey,
                cancellationToken
            );
            return userLogin;
        }

        protected override async Task<Models.IdentityUserLogin<TKey>> FindUserLoginAsync(
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var userLogin = await UserLogins.FirstOrDefaultAsync(
                ul => ul.LoginProvider == loginProvider
                    && ul.ProviderKey == providerKey,
                cancellationToken
            );
            return userLogin;
        }

        public override async Task<IList<Claim>> GetClaimsAsync(
            TUser user,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var claims = await UserClaims.Where(
                    uc => uc.UserId.Equals(user.Id)
                )
                .Select(c => c.ToClaim())
                .ToListAsync(cancellationToken);
            return claims;
        }

        public override async Task AddClaimsAsync(
            TUser user,
            IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }
            foreach (var claim in claims)
            {
                await _session.SaveAsync(
                    CreateUserClaim(user, claim),
                    cancellationToken
                );
            }
            await FlushChangesAsync(cancellationToken);
        }

        public override async Task ReplaceClaimAsync(
            TUser user,
            Claim claim,
            Claim newClaim,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            if (newClaim == null)
            {
                throw new ArgumentNullException(nameof(newClaim));
            }
            var matchedClaims = await UserClaims.Where(
                    uc => uc.UserId.Equals(user.Id) &&
                        uc.ClaimValue == claim.Value
                        && uc.ClaimType == claim.Type
                )
                .ToListAsync(cancellationToken);
            foreach (var matchedClaim in matchedClaims)
            {
                matchedClaim.ClaimType = newClaim.Type;
                matchedClaim.ClaimValue = newClaim.Value;
                await _session.UpdateAsync(matchedClaim, cancellationToken);
            }
            await FlushChangesAsync(cancellationToken);
        }

        public override async Task RemoveClaimsAsync(
            TUser user,
            IEnumerable<Claim> claims,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }
            foreach (var claim in claims)
            {
                var matchedClaims = await UserClaims.Where(
                        uc => uc.UserId.Equals(user.Id) &&
                            uc.ClaimValue == claim.Value
                            && uc.ClaimType == claim.Type
                    )
                    .ToListAsync(cancellationToken);
                foreach (var matchedClaim in matchedClaims)
                {
                    await _session.DeleteAsync(matchedClaim, cancellationToken);
                }
            }
            await FlushChangesAsync(cancellationToken);
        }

        public override async Task<IList<TUser>> GetUsersForClaimAsync(
            Claim claim,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            var query = from userClaims in UserClaims
                        join user in Users on userClaims.UserId equals user.Id
                        where userClaims.ClaimValue == claim.Value
                            && userClaims.ClaimType == claim.Type
                        select user;
            return await query.ToListAsync(cancellationToken);
        }

        protected override async Task<Models.IdentityUserToken<TKey>> FindTokenAsync(
            TUser user,
            string loginProvider,
            string name,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var token = await UserTokens.FirstOrDefaultAsync(
                ut => ut.UserId.Equals(user.Id) &&
                    ut.LoginProvider == loginProvider
                    && ut.Name == name,
                cancellationToken);
            return token;
        }

        protected override async Task AddUserTokenAsync(
            Models.IdentityUserToken<TKey> token)
        {
            ThrowIfDisposed();
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }
            await _session.SaveAsync(token);
            await FlushChangesAsync();
        }

        protected override async Task RemoveUserTokenAsync(
            Models.IdentityUserToken<TKey> token)
        {
            ThrowIfDisposed();
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }
            await _session.DeleteAsync(token);
            await FlushChangesAsync();
        }

        public override async Task AddLoginAsync(
            TUser user,
            Microsoft.AspNetCore.Identity.UserLoginInfo login,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }
            await _session.SaveAsync(
                CreateUserLogin(user, login),
                cancellationToken
            );
            await FlushChangesAsync(cancellationToken);
        }

        public override async Task RemoveLoginAsync(
            TUser user,
            string loginProvider,
            string providerKey,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var login = await FindUserLoginAsync(
                user.Id,
                loginProvider,
                providerKey,
                cancellationToken
            );
            if (login != null)
            {
                await _session.DeleteAsync(login, cancellationToken);
            }
        }

        public override async Task<IList<Microsoft.AspNetCore.Identity.UserLoginInfo>> GetLoginsAsync(
            TUser user,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var userId = user.Id;
            var logins = await UserLogins.Where(l => l.UserId.Equals(userId))
                .Select(
                    l => new Microsoft.AspNetCore.Identity.UserLoginInfo(
                        l.LoginProvider,
                        l.ProviderKey,
                        l.ProviderDisplayName
                    )
                )
                .ToListAsync(cancellationToken);
            return logins;
        }

        public override async Task<TUser> FindByEmailAsync(
            string normalizedEmail,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return await Users.FirstOrDefaultAsync(
                u => u.NormalizedEmail == normalizedEmail,
                cancellationToken
            );
        }

        private async Task FlushChangesAsync(
            CancellationToken cancellationToken = default)
        {
            await _session.FlushAsync(cancellationToken);
            _session.Clear();
        }

    }
}

using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MultitenantNHibernate53AspNet5.Models.Mappings;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;

namespace MultitenantNHibernate53AspNet5
{
    public static class IdentityBuilderExtensions
    {
        public static Configuration AddIdentityMapping(
            this Configuration configuration)
        {
            return configuration.AddIdentityMapping<long>();
        }

        public static Configuration AddIdentityMapping<TKey>(
            this Configuration configuration)
            where TKey : IEquatable<TKey>
        {
            return configuration.AddIdentityMapping<IdentityUser<TKey>, IdentityRole<TKey>, TKey>();
        }

        public static Configuration AddIdentityMapping<TUser, TKey>(
            this Configuration configuration)
            where TKey : IEquatable<TKey>
            where TUser : IdentityUser<TKey>
        {
            return configuration.AddIdentityMapping<TUser, IdentityRole<TKey>, TKey>();
        }

        public static Configuration AddIdentityMapping<TUser, TKey>(
            this Configuration configuration,
            IGeneratorDef generatorDef)
            where TKey : IEquatable<TKey>
            where TUser : IdentityUser<TKey>
        {
            return configuration.AddIdentityMapping<TUser, IdentityRole<TKey>, TKey>(generatorDef);
        }

        public static Configuration AddIdentityMapping<TUser, TRole, TKey>(
            this Configuration configuration)
            where TKey : IEquatable<TKey>
            where TUser : IdentityUser<TKey>
            where TRole : IdentityRole<TKey>
        {
            return configuration.AddIdentityMapping<TUser, TRole, TKey>(Generators.Identity);
        }

        public static Configuration AddIdentityMapping<TUser, TRole, TKey>(
            this Configuration configuration,
            IGeneratorDef generatorDef)
            where TKey : IEquatable<TKey>
            where TUser : IdentityUser<TKey>
            where TRole : IdentityRole<TKey>
        {
            var mapper = new ModelMapper();

            mapper.AddMapping(new IdentityRoleMapping<TRole, TKey>(generatorDef));
            mapper.AddMapping(new IdentityRoleClaimMapping<TKey>(generatorDef));
            mapper.AddMapping(new IdentityUserMapping<TUser, TKey>(generatorDef));
            mapper.AddMapping(new IdentityUserClaimMapping<TKey>(generatorDef));
            mapper.AddMapping<IdentityUserLoginMapping<TKey>>();
            mapper.AddMapping<IdentityUserRoleMapping<TKey>>();
            mapper.AddMapping<IdentityUserTokenMapping<TKey>>();

            configuration.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());

            return configuration;
        }

        public static IdentityBuilder AddHibernateStores(
            this IdentityBuilder builder)
        {
            AddStores<long>(builder.Services, builder.UserType, builder.RoleType);
            return builder;
        }

        public static IdentityBuilder AddHibernateStores<TKey>(
            this IdentityBuilder builder)
        {
            AddStores<TKey>(builder.Services, builder.UserType, builder.RoleType);
            return builder;
        }

        private static void AddStores<TKey>(
            IServiceCollection services,
            System.Type userType,
            System.Type roleType)
        {
            if (roleType != null)
            {
                // register user store type
                var userStoreServiceType = typeof(IUserStore<>)
                    .MakeGenericType(userType);
                var userStoreImplType = typeof(UserStore<,,>)
                    .MakeGenericType(userType, roleType, typeof(TKey));
                services.AddScoped(userStoreServiceType, userStoreImplType);
                // add role store type
                var roleStoreSvcType = typeof(IRoleStore<>)
                    .MakeGenericType(roleType);
                var roleStoreImplType = typeof(RoleStore<,>)
                    .MakeGenericType(roleType, typeof(TKey));
                services.AddScoped(roleStoreSvcType, roleStoreImplType);
            }
            else
            {
                // register user only store type
                var userStoreServiceType = typeof(IUserStore<>)
                    .MakeGenericType(userType);
                var userStoreImplType = typeof(UserOnlyStore<,>)
                    .MakeGenericType(userType, typeof(TKey));
                services.AddScoped(userStoreServiceType, userStoreImplType);
            }
        }
    }
}

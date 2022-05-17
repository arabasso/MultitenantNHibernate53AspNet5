using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using NHibernate;
using NHibernate.Cfg;

namespace MultitenantNHibernate53AspNet5
{
    public class Startup
    {
        private readonly ConcurrentDictionary<string, ISessionFactory> _sessionFactories = new();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddDefaultIdentity<Models.IdentityUser<long>>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddHibernateStores<long>();
            services.AddScoped(c =>
            {
                var accessor = c.GetRequiredService<IHttpContextAccessor>();

                var tenant = accessor.HttpContext?.Request.Host.Host;

                var sessionFactory = _sessionFactories.GetOrAdd(tenant, BuildSessionFactory);

                return sessionFactory.OpenSession();
            });
            services.AddRazorPages();
        }

        private ISessionFactory BuildSessionFactory(
            string tenant)
        {
            return new Configuration()
                .DataBaseIntegration(db =>
                {
                    db.ConnectionString = Configuration.GetConnectionString(tenant);
                    db.Driver<NHibernate.Driver.MySqlConnector.MySqlConnectorDriver>();
                    db.Dialect<NHibernate.Dialect.MySQL57Dialect>();
                    db.SchemaAction = SchemaAutoAction.Create;
                })
                .AddIdentityMapping<Models.IdentityUser<long>, long>()
                .BuildSessionFactory();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}

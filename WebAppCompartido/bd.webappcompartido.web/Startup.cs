using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ds.core.data;
using ds.core.entities;
using ds.zeus.data;

using Swashbuckle.AspNetCore.Swagger;
using ds.core.Services;
using ds.zeus.services.Interfaces;
using ds.zeus.services.Services;
using ds.core.services.Interfaces;
using ds.core.services.Services;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using ds.core.web.shared.LDAP;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ZeusWebSite
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option=> 
                           {
                               option.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                           }
                           );

            services.AddMemoryCache();

            services.AddDbContext<CoreDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ZeusConnection")));

            services.AddDbContext<ZeusDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ZeusConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Cookies.ApplicationCookie.AuthenticationScheme = "ApplicationCookie";
                options.Cookies.ApplicationCookie.DataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo(@"c:\shared-auth-ticket-keys\"));
                
            })
                .AddUserManager<LdapUserManager<ApplicationUser>>()
                .AddEntityFrameworkStores<CoreDbContext>()
                .AddDefaultTokenProviders();



            services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });


            

            //LDAP Provider
            services.Configure<LdapConfig>(Configuration.GetSection("ldap"));
            services.AddScoped<IAuthenticationService<ApplicationUser>, LdapAuthenticationService<ApplicationUser>>();

            // Add servicios de log .
            services.AddScoped<ICommonSecurityService, CommonSecurityService>();
            services.AddScoped<ILogEntryService, LogEntryService>();

            //Servicios de mensajes
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();


            //Servicios Compartidos


            services.AddSingleton<IAreaConocimientoServicio, AreaConocimientoServicio>();
            services.AddSingleton<ICiudadServicio, CiudadServicio>();
            services.AddSingleton<IFactorServicio, FactorServicio>();
            services.AddSingleton<IInstitucionFinancieraServicio, InstitucionFinancieraServicio>();
            services.AddSingleton<IMarcaServicio, MarcaServicio>();
            services.AddSingleton<INacionalidadServicio, NacionalidadServicio>();
            services.AddSingleton<IPaisServicio, PaisServicio>();
            services.AddSingleton<IPaqueteInformaticoServicio, PaqueteInformaticoServicio>();
            services.AddSingleton<IParentescosServicio, ParentescoServicio>();
            services.AddSingleton<IParroquiaServicio, ParroquiaService>();
            services.AddSingleton<IProvinciaServicio, ProvinciaServicio>();
            services.AddSingleton<ISexoServicio, SexoServicio>();
            services.AddSingleton<ITipoAccionPersonalServicio, TipoAccionPersonalServicio>();
            services.AddSingleton<ITipoActivoFijoServicio, TipoActivoFijoServicio>();
            services.AddSingleton<ITipoPermisoServicio, TipoPermisoServicio>();
            services.AddSingleton<ITipoArticuloServicio, TipoArticuloServicio>();
            services.AddSingleton<ITipoCertificadoServicio, TipoCertificadoServicio>();
            services.AddSingleton<ITipoConcursoServicio, TipoConcursoServicio>();
            services.AddSingleton<ITipoDiscapacidadServicio, TipoDiscapacidadServicio>();
            services.AddSingleton<ITipoEnfermedadServicio, TipoEnfermedadServicio>();
            services.AddSingleton<ITipoIdentificacionServicio, TipoIdentificacionServicio>();
            services.AddSingleton<ITipoMovimientoInternoServicio, TipoMovimientoInternoServicio>();
            services.AddSingleton<ITipoPermisoServicio, TipoPermisoServicio>();
            services.AddSingleton<ITipoProvisionServicio, TipoProvisionServicio>();
            services.AddSingleton<ITipoRMUServicio, TipoRMUServicio>();
            services.AddSingleton<ITipoSangreServicio, TipoSangreServicio>();
            services.AddSingleton<ITipoTransporteServicio, TipoTransporteServicio>();
            services.AddSingleton<ITipoViaticoServicio, TipoViaticoServicio>();
            services.AddSingleton<IModeloServicio, ModeloServicio>();

            services.AddSingleton<IEstadoCivilServicio, EstadoCivilServicio>();
            services.AddSingleton<IEstudioServicio, EstudioServicio>();
            services.AddSingleton<IEtniaServicio, EtniaServicio>();
            services.AddSingleton<IItemViaticoServicio, ItemViaticoServicio>();
            services.AddSingleton<ISucursalServicio, SucursalServicio>();

            services.AddScoped<IMenuServicio, MenuServicio>();



            services.AddSingleton<ISeguridadServicio, SeguridadServicio>();

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ZEUS API", Version = "v1" });
            });




        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();


                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
                {
                    serviceScope.ServiceProvider.GetService<CoreDbContext>()
                        .Database.Migrate();

                    serviceScope.ServiceProvider.GetService<CoreDbContext>().EnsureSeedData();

                    var userManager = app.ApplicationServices.GetService<UserManager<ApplicationUser>>();
                    var roleManager = app.ApplicationServices.GetService<RoleManager<ApplicationRole>>();

                    serviceScope.ServiceProvider.GetService<CoreDbContext>().EnsureSeedData(userManager, roleManager);

                    serviceScope.ServiceProvider.GetService<ZeusDbContext>()
                         .Database.Migrate();
                }
                
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIdentity();

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ZEUS API V1");
            });
        }
    }
}

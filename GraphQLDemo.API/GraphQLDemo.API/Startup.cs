using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using Google.Apis.Auth.OAuth2;
using GraphQLDemo.API.DataLoaders;
using GraphQLDemo.API.Schema.Queries;
using GraphQLDemo.API.Schema.Queries.Mutaions;
using GraphQLDemo.API.Schema.Subscriptions;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;
using GraphQLDemo.API.Services.Instructors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GraphQLDemo.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Problem after installing updated version of HotChocolate.AspNetCore
            // -> AddGraphQLServer is not available in the services

            // Probable Solution 


            // if AddGraphQLServer is not available after installing updated version of the package then see which
            // version of the package support the dot net verstion you are using.

            services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>()
                .AddFiltering()
                .AddSorting()
                .AddProjections()
                .AddAuthorization();

            // Initialize Firebase Admin SDK
            /*
             * FirebaseApp.Create: Initializes the Firebase Admin SDK with the provided credentials.
             * The credentials JSON file is required to authenticate and interact with Firebase services.
             */

            // Configure JWT Bearer Authentication
            services.AddSingleton(FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("./firebase-config.json")
            }));
            services.AddFirebaseAuthentication();

            services.AddAuthorization(o => o.AddPolicy("IsAdmin", p => p.RequireClaim(FirebaseUserClaimType.EMAIL, "joynal@gmail.com")));

            /*
             * services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme): 
             * Registers the authentication services with JWT Bearer as the default scheme.
             * AddJwtBearer: Configures JWT Bearer authentication with specific options:
             * options.Authority: The URL that the JWT tokens should be validated against (Firebase project).
             */

            // subscription provider -> give a place where hotchocolate can manage the event.
            services.AddInMemorySubscriptions();


            var connectionString = _configuration.GetConnectionString("default");
            services.AddPooledDbContextFactory<SchoolDBContext>(sbd => sbd.UseSqlite(connectionString));

            services.AddScoped<CoursesRepository>();
            services.AddScoped<InstructorRepository>();
            services.AddScoped<InstructorDataLoader>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseWebSockets();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}

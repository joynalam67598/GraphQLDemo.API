using AppAny.HotChocolate.FluentValidation;
using FirebaseAdmin;
using FirebaseAdminAuthentication.DependencyInjection;
using FirebaseAdminAuthentication.DependencyInjection.Extensions;
using FirebaseAdminAuthentication.DependencyInjection.Models;
using FluentValidation.AspNetCore;
using Google.Apis.Auth.OAuth2;
using GraphQLDemo.API.DataLoaders;
using GraphQLDemo.API.Schema.Queries;
using GraphQLDemo.API.Schema.Queries.Mutaions;
using GraphQLDemo.API.Schema.Subscriptions;
using GraphQLDemo.API.Services;
using GraphQLDemo.API.Services.Courses;
using GraphQLDemo.API.Services.Instructors;
using GraphQLDemo.API.Validators;
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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFluentValidation();
            services.AddTransient<CourseTypeInputValidator>();

            services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>()
                .AddType<CourseType>()
                .AddType<InstructorType>()
                .AddType<CourseQuery>()
                .AddFiltering()
                .AddSorting()
                .AddProjections()
                .AddAuthorization()
                .AddFluentValidation(o =>
                {
                    o.UseDefaultErrorMapper();
                });

            services.AddSingleton(FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("./firebase-config.json")
            }));
            services.AddFirebaseAuthentication();

            services.AddAuthorization(o => o.AddPolicy("IsAdmin", p => p.RequireClaim(FirebaseUserClaimType.EMAIL, "joynal@gmail.com")));

            services.AddInMemorySubscriptions();


            var connectionString = _configuration.GetConnectionString("default");
            services.AddPooledDbContextFactory<SchoolDBContext>(sbd => sbd.UseSqlite(connectionString));

            services.AddScoped<CoursesRepository>();
            services.AddScoped<InstructorRepository>();
            services.AddScoped<InstructorDataLoader>();
            services.AddScoped<UserDataLoader>();
        }

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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestWithASPNETUdemy.Model.Context;
using RestWithASPNETUdemy.Business;
using RestWithASPNETUdemy.Business.Implementations;
using System;
using System.Collections.Generic;
using RestWithASPNETUdemy.Repository;
using Serilog;
using RestWithASPNETUdemy.Repository.Generic;
using RestWithASPNETUdemy.Data.Converter.Contract;
using RestWithASPNETUdemy.Data.Converter.Implementations;
using RestWithASPNETUdemy.Data.VO;
using RestWithASPNETUdemy.Model;
using Microsoft.Net.Http.Headers;
using RestWithASPNETUdemy.Hypermedia.Filters;
using RestWithASPNETUdemy.Hypermedia.Enricher;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RestWithASPNETUdemy
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; } // DECLARAÇÃO DE WEBHOSTENVIRONMENT PARA fornecer informações sobre o ambiente de host da Web em que um aplicativo está sendo executado

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            //Cria o Log
            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // PARA CONEXÃO COM BANCO DE DADOS
            var connection = Configuration["MySQLConnection:MySQLConnectionString"];
            services.AddDbContext<MySQLContext>(options => options.UseMySql(connection));

            if (Environment.IsDevelopment())
            {
                MigrationDatabase(connection);
            }

            //Injeção de Dependencia
            services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();
            services.AddScoped<IBookBusiness, BooksBusinessImplementation>();
            /*services.AddScoped<IParser<BooksVO, Books>, BooksConverter>();
            services.AddScoped<IParser<Books, BooksVO>, BooksConverter>();
            services.AddScoped<IParser<PersonVO, Person>, PersonConverter>();
            services.AddScoped<IParser<Person, PersonVO>, PersonConverter>();*/

            //Injeção de Dependencia - Repositório Genérico
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            // Content Negociation
            /*services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true;

                options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
                options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));
            }).AddXmlSerializerFormatters();*/

            //HATEOAS
            var filterOptions = new HyperMediaFilterOptions();
            filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
            filterOptions.ContentResponseEnricherList.Add(new BookEnricher());
            services.AddSingleton(filterOptions);       

            // VERSIONAMENTO DA API
            services.AddApiVersioning();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("DefaultApi", "{controller=values}/{id?}");
            });
        }

        private void MigrationDatabase(string connection)
        {
            try
            {
                var evolveConnection = new MySql.Data.MySqlClient.MySqlConnection(connection);
                var evolve = new Evolve.Evolve(evolveConnection, msg => Log.Information(msg))
                {
                    Locations = new List<string> { "db/migrations", "db/dataset" },
                    IsEraseDisabled = true,

                };
                evolve.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error("Database migration failed", ex);
                throw;
            }
        }
    }
}

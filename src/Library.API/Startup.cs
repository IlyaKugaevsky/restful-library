using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Library.Data.Entities;
using Library.API.Services;
using Library.API.Helpers;

namespace Library.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // services.AddDbContext<LibraryContext>(o => o.UseSqlite("Data Source=library.db"));
            services.AddDbContext<LibraryContext>(o => o.UseSqlite("Data Source=library.db"));

            // register the repository
            services.AddScoped<ILibraryRepository, LibraryRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, LibraryContext libraryContext)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Library.Data.Entities.Author, 
                Library.API.Models.AuthorDto>()
                    .ForMember(dest => dest.Name, opt =>
                    opt.MapFrom( src => $"{src.FirstName} {src.LastName}"))
                    .ForMember(dest => dest.Age, opt =>
                    opt.MapFrom(src => src.DateOfBirth.GetCurrentAge()));
            });

            libraryContext.EnsureSeedDataForContext();

            app.UseMvc(); 
        }
    }
}

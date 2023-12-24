using BLL;
using DLL2;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Lab4
{
    public class Startup(IConfiguration configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
            
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            LibraryContext context = new LibraryContext(options);
            context.Database.EnsureDeleted();

            services.AddScoped<IRepository<Book>>(provider => new BookRepository(context));
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IRepository<Reader>>(provider => new ReaderRepository(context));
            services.AddScoped<IReaderService, ReaderService>();

            services.AddControllersWithViews();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }); });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAll");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Lab 5 api"); });
        }
    }
}
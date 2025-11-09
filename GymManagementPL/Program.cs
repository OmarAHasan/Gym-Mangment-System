using GymManagementBLL;
using GymManagementBLL.Services.AttachmentServies;
using GymManagementBLL.Services.classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementDAL.Data.Context;
using GymManagementDAL.Data.DataSeeding;
using GymManagementDAL.Data.Repositories.Classes;
using GymManagementDAL.Data.Repositories.Interfaces;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(options =>
            {

                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });

            //builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddScoped<IPlanRepository, PlanRepository>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddAutoMapper(X => X.AddProfile(new MappingProfiles()));
            builder.Services.AddScoped<IMemberServies, MemberServies>();
            builder.Services.AddScoped<IPlanRepo, PlanRepo>();
            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            builder.Services.AddScoped<ITrainerServies, TrainerServies>();
            builder.Services.AddScoped<IPlanServies, PlanServies>();
            builder.Services.AddScoped<ISessionServies, SessionServies>();
            builder.Services.AddScoped<IMemberShipServies, MemberShipServies>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IMemberShipRepository, MemberShipRepository>();
            builder.Services.AddScoped<IAttachmentServies, AttachmentServies>();
            builder.Services.AddScoped<IAccountServies, AccountServies>();
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<GymDbContext>();

            builder.Services.ConfigureApplicationCookie(options => { 
            
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";

           });

            builder.Services.AddScoped<IMemberShipServies, MemberShipServies>();
            builder.Services.AddScoped<IBookingService, BookingService>();

            #endregion

            var app = builder.Build();

            #region Dataseeding


            //// we need object from dbcontext so we gonna to ask clr to inject object of gymdbcontext from scopedservies
            using var scoped = app.Services.CreateScope();
            var dbcontext = scoped.ServiceProvider.GetRequiredService<GymDbContext>();
            var rolemanager = scoped.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var usermanager = scoped.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            // to applay dataseed first check if no pending migrations
            var pendingmaigrations = dbcontext.Database.GetPendingMigrations();
            if (pendingmaigrations?.Any() ?? false)
                dbcontext.Database.Migrate();

            // applay dataseed
            GymDbContextSeeding.SeedData(dbcontext);
            IdentityDbContextSeeding.SeedData(rolemanager, usermanager);


            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}

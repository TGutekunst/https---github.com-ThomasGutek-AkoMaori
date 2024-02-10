
using A2.Data;
using A2.Handler;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
//using A2.Helper;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<A2DbContext>(options => options.UseSqlite(builder.Configuration["A2Database"]));
        builder.Services.AddScoped<IA2Repo, A2Repo>();


        builder.Services
            .AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, A2AuthHandler>("Authentication", null);
        builder.Services.AddDbContext<A2DbContext>(
            options => options.UseSqlite(builder.Configuration["AuthDbConnection"])
        );
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireClaim("organizer"));
            options.AddPolicy("UserOnly", policy => policy.RequireClaim("user"));

            options.AddPolicy("AdminAndUser", policy => {
                policy.RequireAssertion(context =>
                    (context.User.HasClaim(c =>
                    (c.Type == "organizer" || c.Type == "user"))));
            });
        });



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        //new added
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
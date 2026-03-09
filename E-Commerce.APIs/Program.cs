using E_Commerce.APIs.Middleware;
using E_Commerce.APIs.Servicies;
using E_Commerce.App.Application;
using E_Commerce.App.Application.Abstruction;
using E_Commerce.App.Application.Abstruction.Models.Auth;
using E_Commerce.App.Domain.Contract.Peresistence.DbIntializer;
using E_Commerce.App.Domain.Entities.Identity;
using E_Commerce.App.Infrastructre;
using E_Commerce.App.Infrastructre.presistent;
using E_Commerce.App.Infrastructre.presistent.Identity;
using E_Commerce_Api.Controller;
using E_Commerce_Api.Controller.Error;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace E_Commerce.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var WebApplicationbuilder = WebApplication.CreateBuilder(args);


            #region Configuration Service

            // Add services to the container.

            WebApplicationbuilder.Services.AddControllers()
                .ConfigureApiBehaviorOptions(option => {
                    option.SuppressModelStateInvalidFilter = false;
                    option.InvalidModelStateResponseFactory = (actionContext) =>
                    {
                        var errors = actionContext.ModelState.Where(P => P.Value!.Errors.Count > 0)
                                                             .Select(P => new ApiValidationsErrorResponse.ValidationError()
                                                             {
                                                                 Field = P.Key,
                                                                 Errors = P.Value!.Errors.Select(E => E.ErrorMessage)
                                                             });

                        return new BadRequestObjectResult(new ApiValidationsErrorResponse() { Errors = errors });

                    };
                })
                .AddApplicationPart(typeof(ControllerAssemblyInformation).Assembly);

            // ?? Configure Swagger To Accept Bearer Token (JWT)
            WebApplicationbuilder.Services.AddEndpointsApiExplorer();
            WebApplicationbuilder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter Bearer Token Like This: Bearer {your token}"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            WebApplicationbuilder.Services.AddHttpContextAccessor();
            WebApplicationbuilder.Services.AddScoped(typeof(ILoggedInUserService), typeof(LoggedInUserService));

            WebApplicationbuilder.Services.AddPersistenceService(WebApplicationbuilder.Configuration);
            WebApplicationbuilder.Services.AddApplicatinServices(WebApplicationbuilder.Configuration);
            WebApplicationbuilder.Services.AddInfrastructureServices(WebApplicationbuilder.Configuration);
            WebApplicationbuilder.Services.Configure<JWTSettings>(WebApplicationbuilder.Configuration.GetSection("JWTSettings"));

            WebApplicationbuilder.Services.AddIdentity<ApplicationsUser, IdentityRole>(Identityoptions => {


                //Identityoptions.SignIn.RequireConfirmedEmail = true;
                //Identityoptions.SignIn.RequireConfirmedPhoneNumber = true;
                //Identityoptions.SignIn.RequireConfirmedPhoneNumber = true;

                Identityoptions.Password.RequireNonAlphanumeric = true;
                Identityoptions.Password.RequiredUniqueChars = 2;
                Identityoptions.Password.RequiredLength = 6;
                Identityoptions.Password.RequireDigit = true;
                Identityoptions.Password.RequireLowercase = true;
                Identityoptions.Password.RequireUppercase = true;

                Identityoptions.Lockout.AllowedForNewUsers = true;
                Identityoptions.Lockout.MaxFailedAccessAttempts = 5;
                Identityoptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                Identityoptions.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";

            }
            )
                .AddEntityFrameworkStores<StorIdentityDbContext>().AddDefaultTokenProviders();

            WebApplicationbuilder.Services.AddAuthentication(authentationOption
                => {
                    authentationOption.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(jwtOption => {
                    jwtOption.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = WebApplicationbuilder.Configuration["JWTSettings:issuer"],
                        ValidAudience = WebApplicationbuilder.Configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(WebApplicationbuilder.Configuration["JWTSettings:Key"]!)),
                        ClockSkew = TimeSpan.FromMinutes(0)
                    };
                });

          
            #endregion

            var app = WebApplicationbuilder.Build();

            #region Update Database and Data Seeding

            var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider;
            var stroreContext = service.GetRequiredService<IStroreContextIntializer>();
            var IdentityContext = service.GetRequiredService<IStoreIdentityContextIntializer>();

            var LoggerFactory = service.GetRequiredService<ILoggerFactory>();
            //var LoggerFactoryLogger = service.GetRequiredService(typeof(ILoggerFactory));
            try
            {

                await stroreContext.UpdateDateBase();
                await stroreContext.SeedData(WebApplicationbuilder.Environment.ContentRootPath);

                await IdentityContext.UpdateDateBase();
                await IdentityContext.SeedData();
            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError("An error occurred while applying migrations or Data Seeding.");
                Console.WriteLine(ex);
            }

            #endregion

            #region Configuration Kestral Middelware

            // Configure the HTTP request pipeline.

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            // if (app.Environment.IsDevelopment())
            // {
            //     app.UseSwagger();
            //     app.UseSwaggerUI();
            // }
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseStatusCodePagesWithReExecute("/Errors/{0}");

            app.UseDefaultFiles();
            app.UseStaticFiles();


            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            #endregion
            app.Run();
        }
    }
}

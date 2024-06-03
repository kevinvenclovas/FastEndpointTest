using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json.Converters;

namespace FastEndpointTest
{
    public static class StartUpExtension
    {
        public static void SetUpCors(this IServiceCollection collectionService)
        {
            collectionService.AddCors(options =>
            {
                // TODO Setup for Release
                options.AddPolicy(name: "All",
                                    builder =>
                                    {
                                        builder.AllowAnyOrigin();
                                        builder.AllowAnyMethod();
                                        builder.AllowAnyHeader();
                                    });

                options.AddPolicy(name: "WebhookSendInBlue",
                                 builder =>
                                 {
                                     builder.AllowAnyOrigin();
                                     builder.AllowAnyMethod();
                                     builder.AllowAnyHeader();
                                 });

                options.AddPolicy(name: "Intern",
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin();
                                      builder.AllowAnyMethod();
                                      builder.AllowAnyHeader();
                                  });

            });
        }
    
        public static void SetUpFastEndpoints(this IServiceCollection collectionService) 
        {
            collectionService.AddFastEndpoints
            (
                o =>
                {
                    o.IncludeAbstractValidators = true;
                }
            );

            // Add reponse caching
            collectionService.AddResponseCaching();

            // Add Swagger
            collectionService.SwaggerDocument(o =>
            {
                o.DocumentSettings = s =>
                {
                    s.DocumentName = "v1";
                    s.Title = "Alfa Tools";
                    s.Version = "v1";
                };
                o.ExcludeNonFastEndpoints = true;
                o.EnableJWTBearerAuth = true;
                o.AutoTagPathSegmentIndex = 0;
                o.ShortSchemaNames = true;
                o.TagDescriptions = t =>
                {
                    t["Authentication"] = "All endpoints to our dear users <3";
                    t["Health"] = "All endpoints about application health";
                    t["Application"] = "All endpoints about application informations";
                    t["User"] = "All endpoints about application users";
                };
                o.NewtonsoftSettings = s =>
                {
                    s.Converters.Add(new StringEnumConverter());
                };
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            collectionService.AddEndpointsApiExplorer();
        }
    
        public static void SetUpAuthentication(this IServiceCollection collectionService)
        {
            // Add authentication
            collectionService.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            collectionService.AddJWTBearerAuth("Bx65sP54FEXU40J22VOF6hrPEYT1suy5wnz73aGXwLV7UQHKl8QtkSDRhsvMdbh");

            collectionService.AddAuthorization();
        }

    }
}

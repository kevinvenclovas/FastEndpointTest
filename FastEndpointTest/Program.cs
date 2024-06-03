using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FastEndpointTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddFastEndpoints();

            builder.Services.SwaggerDocument(o =>
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
                };
                o.NewtonsoftSettings = s =>
                {
                    s.Converters.Add(new StringEnumConverter());
                };
            });

            builder.Services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            builder.Services.AddJWTBearerAuth("Bx65sP54FEXU40J22VOF6hrPEYT1suy5wnz73aGXwLV7UQHKl8QtkSDRhsvMdbh");

            builder.Services.AddAuthorization();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseFastEndpoints(c =>
            {
                c.Serializer.RequestDeserializer = async (req, tDto, jCtx, ct) =>
                {
                    using var reader = new StreamReader(req.Body);
                    return JsonConvert.DeserializeObject(await reader.ReadToEndAsync(), tDto);
                };
                c.Serializer.ResponseSerializer = (rsp, dto, cType, jCtx, ct) =>
                {
                    rsp.ContentType = cType;
                    return rsp.WriteAsync(JsonConvert.SerializeObject(dto), ct);
                };
                c.Endpoints.RoutePrefix = "api";
              
                c.Endpoints.ShortNames = true;
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

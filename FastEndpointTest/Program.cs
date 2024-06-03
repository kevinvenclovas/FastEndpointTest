using FastEndpoints;
using FastEndpointTest;
using Microsoft.AspNetCore.HttpLogging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

try
{

    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();

    // SetUp FastEndpoints
    builder.Services.SetUpFastEndpoints();

    // Configurate Http logging
    builder.Services.AddHttpLogging(logging =>
    {
        logging.LoggingFields =
            HttpLoggingFields.RequestMethod |
            HttpLoggingFields.RequestPath |
            HttpLoggingFields.RequestBody |
            HttpLoggingFields.ResponseBody |
            HttpLoggingFields.ResponseStatusCode;
        logging.RequestBodyLogLimit = 4096;
        logging.ResponseBodyLogLimit = 4096;
        logging.CombineLogs = true;
    });

    // Add cors settings
    builder.Services.SetUpCors();

    // Add services to the container.
    builder.Services.AddControllers();

    // SetUp authentication
    builder.Services.SetUpAuthentication();

    // Add middleware
    builder.Services.AddMvc();

    var app = builder.Build();

    // Add Swagger UI
#if !RELEASE

    app.UseOpenApi();

    app.UseSwaggerUi(c =>
    {
        c.Path = string.Empty;
        c.DocumentTitle = "AlfaTool API";
    });

#endif

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

    app.UseHttpLogging();

    // Configure the HTTP request pipeline.
    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();

    JsonConvert.DefaultSettings = () => new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        }
    };

    app.UseResponseCaching();
    app.Run();

}
catch (Exception exception)
{
    throw;
}

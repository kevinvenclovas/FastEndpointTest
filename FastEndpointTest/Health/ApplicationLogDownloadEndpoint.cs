using AlfaTools.Modules.Core.Services.Logging;
using AlfaTools.Modules.Models.Core.Logging;
using AlfaTools.Utils.Entities;
using ExcelBuilder;
using ExcelBuilder.Entities;
using FastEndpoints;

namespace FastEndpointTest.Health
{
    public class ApplicationLogDownloadEndpoint : EndpointWithoutRequest
    {
     
        public override void Configure()
        {
          
            AccessControl(
                "Log_Export",
                Apply.ToThisEndpoint,
                "Health"
                );

            Get("health/logs/export");
        }

        public override async Task HandleAsync(CancellationToken c)
        {
            await SendAsync(new object()).ConfigureAwait(false);
        }

    }
}

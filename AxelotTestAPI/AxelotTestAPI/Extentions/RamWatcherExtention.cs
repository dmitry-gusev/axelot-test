using AxelotTestAPI.Domain;
using AxelotTestAPI.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxelotTestAPI.Extentions
{
    public static class RamWatcherExtention
    {
        public static IHostBuilder AddRamWatcher(this IHostBuilder builder) =>
            builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IMemoryService, MemoryService>();

                services.AddHostedService<IMemoryService>(sp=>sp.GetRequiredService<IMemoryService>());
            });
            

    }
}

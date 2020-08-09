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
    /// <summary>
    /// Расширение для сервиса сбора данных о памяти приложений (процессов)
    /// </summary>
    public static class RamWatcherExtention
    {
        public static IHostBuilder AddRamWatcher(this IHostBuilder builder) =>
            builder.ConfigureServices((context, services) =>
            {
                //Регистрация сервиса
                services.AddSingleton<IMemoryService, MemoryService>();
                //Запуск сервиса процессом
                services.AddHostedService<IMemoryService>(sp=>sp.GetRequiredService<IMemoryService>());
            });
            

    }
}

using AxelotTestAPI.Domain.Enums;
using AxelotTestAPI.Domain.Interfaces;
using AxelotTestAPI.Extentions;
using AxelotTestAPI.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AxelotTestAPI.Domain
{
    /// <inheritdoc cref="IMemoryService"/>
    public class MemoryService : IMemoryService
    {
        /// <summary>
        /// Контейнес с аблюдаемыми процессами
        /// </summary>
        ConcurrentDictionary<string, ApplicationDataResult> _db = new ConcurrentDictionary<string, ApplicationDataResult>();

        /// <summary>
        /// Период обновления данных о приложениях
        /// </summary>
        int _periodUpdate = 5;


        //Логер
        ILogger<MemoryService> _logger;
        //Сервис провайдер
        IServiceProvider _serviceProvider;

        /// <summary>
        /// Внутренний токен отмены процессов
        /// </summary>
        CancellationTokenSource _internalToken;

        /// <summary>
        /// Текущая задача удаления не запрашиваемых приложений
        /// </summary>
        Task _currentDeleteTask, _currentActiveTask;

        /// <summary>
        /// Сервис контейнер, содержит в себе все активные процессы, управляет жизненным циклом
        /// </summary>
        /// <param name="logger">Логер</param>
        /// <param name="serviceProvider">Сервис провайдер</param>
        public MemoryService(ILogger<MemoryService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;



        }

        /// <summary>
        /// Удаление данных о приложениях, которые не запрашивали более 1 минуты
        /// </summary>
        /// <returns></returns>
        async Task DeleteDroppedWatchers()
        {
            //Пока не отменится работа, вечный цикл
            while (!_internalToken.Token.IsCancellationRequested)
            {
                //Ключи оставленных заданий
                var droppedTask = _db.Where(x => (DateTime.Now - x.Value.LastDataGave).TotalMinutes >= 1)
                    .Select(k => k.Key).ToList();

                //Удаление 
                droppedTask.AsParallel().ForAll(w =>
                {
                    _db.TryRemove(w, out var app);
                });

                //Пауза перед следующим циклом блокируем поток
                await Task.Delay(30 * 1000, _internalToken.Token).ConfigureAwait(false);
            }
        }



        /// <summary>
        /// Удаление объектов
        /// </summary>
        public void Dispose()
        {
            //Команда процессам выключится
            _internalToken.Cancel();
            //Таймаут на выключение
            Task.WaitAll(Task.Delay(1000));
            //Очиска
            _db.Clear();
            _db = null;
        }

        /// <inheritdoc cref="IMemoryService"/>
        public async Task<AppRamDataResult> GetRam(MemoryScale memScale, string processName)
        {
            //Задача есть
            if (_db.ContainsKey(processName))
            {
                return await GetProcessData(processName, memScale);
            }
            return await CreateAppWatch(processName, memScale);
        }

        /// <summary>
        /// Основная задача обновления данных
        /// </summary>
        /// <returns></returns>
        async Task ApplicationDataUpdate()
        {
            while (!_internalToken.Token.IsCancellationRequested)
            {
                var tasks = _db.Select(d => Task.Factory.StartNew(async () =>
                {
                    d.Value.Update(await GetCurrentProcessMemory(d.Value.ProcessName));
                }));
                await Task.WhenAll(tasks.ToArray());

                await Task.Delay(_periodUpdate * 1000, _internalToken.Token).ConfigureAwait(false);
            }

        }


        /// <summary>
        /// Добавление задания для отслеживания
        /// </summary>
        /// <param name="processName">Название процесса, приложения</param>
        /// <param name="memScale">Еденица измерения памяти</param>
        /// <returns>Данные по заданию</returns>
        private async Task<AppRamDataResult> CreateAppWatch(string processName, MemoryScale memScale)
        {
            var app = await GetCurrentProcessMemory(processName);
            app.MemorySc = memScale;
            app.LastDataGave = DateTime.Now;
            while (_internalToken.Token.IsCancellationRequested || !_db.TryAdd(processName, app))
            {
                await Task.Delay(100).ConfigureAwait(false);
            }

            return new AppRamDataResult() { ProcessName = app.ProcessName, ProcTime = app.ValueTime, Scale = memScale.GetDescription(), Value = app.ValueToStringResult() };
        }


        /// <summary>
        /// Получение размера потребляемой памяти приложением
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        private async Task<ApplicationDataResult> GetCurrentProcessMemory(string processName)
        {
            var result = new ApplicationDataResult()
            {
                ProcessName = processName,
                MemorySc = MemoryScale.Mb,
            };

            //По идее в задании говорится про один процесс,
            //но одноименных процессов может быть несколько
            //считаем ram у всех
            var process = Process.GetProcessesByName(processName);
            long sumProcessRAM = 0;
            foreach (var pr in process)
            {
                sumProcessRAM += pr.WorkingSet64;
            }
            result.Value = sumProcessRAM;
            result.ValueTime = DateTime.Now;

            return await Task.FromResult(result);
        }



        /// <summary>
        /// Получение информации о существующем приложении
        /// </summary>
        /// <param name="processName">Название приложения</param>
        /// <param name="memScale">Еденица измерения памяти</param>
        /// <returns></returns>
        private async Task<AppRamDataResult> GetProcessData(string processName, MemoryScale memScale)
        {
            while (!_internalToken.Token.IsCancellationRequested)

            {
                if (_db.TryGetValue(processName, out var app))
                {
                    app.MemorySc = memScale;
                    app.LastDataGave = DateTime.Now;
                    return new AppRamDataResult() { ProcessName = app.ProcessName, ProcTime = app.ValueTime, Scale = memScale.GetDescription(), Value = app.ValueToStringResult() };
                }
                await Task.Delay(100).ConfigureAwait(false);
            }

            throw new Exception($"Информация о приложении {processName} не получена, процесс отменен");

        }

        /// <inheritdoc cref="IMemoryService"/>
        public async Task SetPeriod(int period)
        {
            _periodUpdate = period;
            await Task.CompletedTask;
        }

        /// <inheritdoc cref="IMemoryService"/>
        public async Task<int> GetCurrentPeriod()
        {
            return await Task.FromResult(_periodUpdate);
        }

        /// <inheritdoc cref="IMemoryService"/>
        public async Task<List<string>> GetAllProcesses()
        {
            return await Task.FromResult(Process.GetProcesses().Select(x => x.ProcessName).ToList());
        }

        /// <summary>
        /// Старт сервиса
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Сервис сбора RAM о приложениях запускается ...");
            //Связываем внутренний токен отмены с внешним
            _internalToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            //Стартуем задачи
            //Удаления устаревших
            _currentDeleteTask = Task.Run(DeleteDroppedWatchers);
            //Сбор RAM приложений по интервалу
            _currentActiveTask = Task.Run(ApplicationDataUpdate);
            _logger.LogInformation("Сервис сбора RAM о приложениях запущен");
            await Task.CompletedTask;
        }

        /// <summary>
        /// Остановка сервиса
        /// </summary>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Сервис сбора RAM о приложениях остановлен");

        }
    }
}

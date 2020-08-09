using AxelotTestAPI.Domain.Enums;
using AxelotTestAPI.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxelotTestAPI.Domain.Interfaces
{
    /// <summary>
    /// Сервис поставщик данных для API
    /// </summary>
    public interface IMemoryService: IHostedService,IDisposable
    {
        /// <summary>
        /// Информация о потреблении памяти процессом
        /// </summary>
        /// <param name="memScale">Еденица измерения</param>
        /// <param name="processName">Название процесса</param>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Данные по указанному процессу, если процесс не найден, возвращает ошибку
        /// </returns>
        Task<AppRamDataResult> GetRam(MemoryScale memScale, string processName);

        
        /// <summary>
        /// Установка периода обновления данных о процессах
        /// </summary>
        /// <param name="period">Новый интервал обновления</param>
        /// <returns></returns>
        Task SetPeriod(int period);

        /// <summary>
        /// Возвращает текущее значение периода обновления
        /// </summary>
        /// <returns>Число</returns>
        Task<int> GetCurrentPeriod();

        /// <summary>
        /// Список всех процессов сервера
        /// </summary>
        /// <returns>Список</returns>
        Task<List<string>> GetAllProcesses();
    }
}

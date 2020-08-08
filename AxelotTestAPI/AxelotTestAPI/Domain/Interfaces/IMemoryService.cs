using AxelotTestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxelotTestAPI.Domain.Interfaces
{
    /// <summary>
    /// Сервис поставщик данных для API
    /// </summary>
    public interface IMemoryService
    {
        /// <summary>
        /// Информация о потреблении памяти процессом или процессами
        /// </summary>
        /// <param name="memScale">Еденица измерения</param>
        /// <param name="processName">Название процесса, если процесс не указан, возвращает информацию по всем процессам</param>
        /// <param name="sessionKey">Ключ сессии</param>
        /// <remarks>
        /// 
        /// </remarks>
        /// <returns>Список данных по найденным процессам, если указано имя не существующего процесса,
        /// возвращает ошибку - ProcessNotFoundException
        /// </returns>
        Task<AppRamResult> GetRam(string memScale, string sessionKey, string processName="");

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxelotTestAPI.Models
{
    /// <summary>
    /// Ответ на запрос API RAM
    /// </summary>
    public class AppRamResult : AppWebApiResultBase<AppRamDataResult>
    {
        /// <summary>
        /// Данные по запрошенному приложению (процессу)
        /// </summary>
        public override AppRamDataResult Data { get; set; }
    }

    /// <summary>
    /// Данные по приложению (процессу)
    /// </summary>
    public class AppRamDataResult
    {
        /// <summary>
        /// Дата и время сервера когда получено значение
        /// </summary>
        public DateTime ProcTime { get; set; }

        /// <summary>
        /// Название процесса
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// Значение потребления памяти в выбранно еденице измерения
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Еденица изменерения памяти (Мб, Гб)
        /// </summary>
        public string Scale { get; set; }

    }
}

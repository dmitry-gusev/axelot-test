using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxelotTestAPI.Models
{
    /// <summary>
    /// Ответ на запрос API RAM
    /// </summary>
    public class AppRamResult : AppWebApiResultBase<IEnumerable<AppRamDataResult>>
    {
        /// <summary>
        /// Список данных по запрошенным процессам
        /// </summary>
        public override IEnumerable<AppRamDataResult> Data { get; set; }
    }

    /// <summary>
    /// Данные по процессу
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
        /// Значение потребления памяти в установленных еденицах
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Еденица изменерения
        /// </summary>
        public string Scale { get; set; }

    }
}

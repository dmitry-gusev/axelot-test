using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxelotTestAPI.Models
{
    /// <summary>
    /// Результат операции строковый
    /// </summary>
    /// <remarks>
    /// Для функций возвращающих строковый результат
    /// </remarks>
    public class AppRamStringResult : AppWebApiResultBase<string>
    {
        /// <summary>
        /// Непосредственно результат операции - строка
        /// </summary>
        public override string Data { get; set; }
    }
}

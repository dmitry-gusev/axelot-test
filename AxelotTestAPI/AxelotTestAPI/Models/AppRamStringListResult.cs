using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxelotTestAPI.Models
{
    /// <summary>
    /// Результат выполнения операции
    /// </summary>
    /// <remarks>
    /// Для операций возвращающих список строк
    /// </remarks>
    public class AppRamStringListResult : AppWebApiResultBase<List<string>>
    {
        /// <summary>
        /// Непосредственно результат оперции, список строк
        /// </summary>
        public override List<string> Data { get; set; }
    }
}

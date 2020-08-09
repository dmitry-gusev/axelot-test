using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxelotTestAPI.Models
{
    /// <summary>
    /// Числовой результат выполнения операции
    /// </summary>
    /// <remarks>
    /// Результат API функции, возвращающей числовой (целый) результат
    /// </remarks>
    public class AppRamIntResult : AppWebApiResultBase<int>
    {
        /// <summary>
        /// Непосредственный результат
        /// </summary>
        public override int Data { get; set; }
    }
}

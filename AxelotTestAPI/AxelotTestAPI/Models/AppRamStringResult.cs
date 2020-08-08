using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxelotTestAPI.Models
{
    /// <summary>
    /// Результат операции строковый
    /// </summary>
    public class AppRamStringResult : AppWebApiResultBase<string>
    {
        public override string Data { get; set; }
    }
}

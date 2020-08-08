using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxelotTestAPI.Models
{
    /// <summary>
    /// Результат выполнения операции
    /// </summary>
    public class AppRamStringListResult : AppWebApiResultBase<List<string>>
    {
        public override List<string> Data { get; set; }
    }
}

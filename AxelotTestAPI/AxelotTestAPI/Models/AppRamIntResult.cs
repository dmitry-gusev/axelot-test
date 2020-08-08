using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxelotTestAPI.Models
{
    /// <summary>
    /// Числовой результат выполнения операции
    /// </summary>
    public class AppRamIntResult : AppWebApiResultBase<int>
    {
        public override int Data { get; set; }
    }
}

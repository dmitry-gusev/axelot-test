using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxelotTestAPI.Models
{
    /// <summary>
    /// Ответ на запрос API RAM
    /// </summary>
    public class AppRamListResult : AppWebApiResultBase<List<AppRamDataResult>>
    {
        /// <summary>
        /// Список данных о приложениях
        /// </summary>
        public override List<AppRamDataResult> Data { get; set; }
    }


}

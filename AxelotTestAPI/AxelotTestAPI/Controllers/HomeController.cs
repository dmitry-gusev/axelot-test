using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AxelotTestAPI.Attributes;
using AxelotTestAPI.Domain.Interfaces;
using AxelotTestAPI.Extentions;
using AxelotTestAPI.Interfaces.Abstracts;
using AxelotTestAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AxelotTestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //Проверяет ключ сессии, если его нет, устанавливает
    //и обновляет время последнего запроса к серверу
    [SessionsAtribute]
    public class HomeController : AppBaseController
    {
        /// <summary>
        /// Текущий логгер
        /// </summary>
        ILogger<HomeController> _logger;

        /// <summary>
        /// Сервис обработки запросов API
        /// </summary>
        IMemoryService _memoryService;

        public HomeController(ILogger<HomeController> logger, IMemoryService memoryService)
        {
            _logger = logger;
            _memoryService = memoryService;
        }

        /// <summary>
        /// Информация о потребляемой памяти процессом сервера
        /// </summary>
        /// <param name="memScale">Единицы измерения: Мб и Гб</param>
        /// <param name="processName">Название процесса, если не указано, информация запрашивается оп всем процессам</param>
        /// <returns>Результат в виде </returns>
        [HttpGet]
        public async Task<JsonResult> RAM(string memScale, string processName = "")
        {
            //Ключ сессии
            string key = HttpContext.Session.Get<string>("Key");
            return new JsonResult(new AppRamResult() { });

        }

        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AxelotTestAPI.Domain.Enums;
using AxelotTestAPI.Domain.Interfaces;
using AxelotTestAPI.Extentions;
using AxelotTestAPI.Interfaces.Abstracts;
using AxelotTestAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AxelotTestAPI.Controllers
{
    /// <summary>
    /// Основной контроллер API
    /// </summary>
    [Route("")]
    [ApiController]
    [Produces("application/json")]
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

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="logger">Логгер</param>
        /// <param name="memoryService">Сервис наблюдения за процессами</param>
        public HomeController(ILogger<HomeController> logger, IMemoryService memoryService)
        {
            _logger = logger;
            _memoryService = memoryService;
        }

        /// <summary>
        /// RAM
        /// </summary>
        /// <remarks>
        /// Информация о потребляемой памяти процессом сервера. 
        /// Параметр memScale может принимать значения: Мб или Гб. 
        /// Указывается без учета регистра.
        /// </remarks>
        /// <param name="memScale">Единицы измерения: Мб и Гб</param>
        /// <param name="processName">Название процесса</param>
        /// <returns>Результат в виде данных приложении (процессе)</returns>
        [HttpGet("ram")]
        [ProducesResponseType(typeof(AppRamResult),200)]
        public async Task<IActionResult> GetRam([Required] string memScale, [Required] string processName )
        {
            try
            {
                //Еденица измерения памяти, если параметр не корректный, вызовет ошибку
                //Сравнение без учета регистра
                var _memScale = memScale.ToUpper() switch
                {
                    "МБ" => MemoryScale.Mb,
                    "ГБ" => MemoryScale.Gb,
                    _ => throw new Exception("Еденица измерения памяти указана не верно, доступно два варианта: Мб или Гб")
                };
                //Если процесс вообще не существует, потребление памяти у него будет 0,
                //Хотя может и стоило вызывать ошибку
                var result =await _memoryService.GetRam(_memScale, processName);
                return new JsonResult(new AppRamResult() { Success = true, Data = result });

            }
            catch (Exception e)
            {
                return await Task.FromResult(new JsonResult(new AppRamResult() { Success = false, Data = null, Error = $"{e.Message} {e.InnerException?.Message}" }));
            }


        }

        /// <summary>
        /// SET PERIOD
        /// </summary>
        /// <remarks>
        /// Установка периода обновления данных о потреблении памяти приложениями (процессами).
        /// </remarks>
        /// <param name="period">Параметры периода обновления информации о процессах</param>
        /// <returns></returns>
        [HttpPut("set-period")]
        [ProducesResponseType(typeof(AppRamStringResult), 200)]
        public async Task<IActionResult> SetPeriod([Required] int period)
        {
            try
            {

                await _memoryService.SetPeriod(period);
                return new JsonResult(new AppRamStringResult() { Success = true, Data = null });

            }
            catch (Exception e)
            {
                return await Task.FromResult(new JsonResult(new AppRamStringResult() { Success = false, Data = null, Error = $"{e.Message} {e.InnerException?.Message}" }));
            }


        }

        /// <summary>
        /// CURRENT PERIOD
        /// </summary>
        /// <remarks>
        /// Показывает текущее значение интервала обновления.
        /// </remarks>
        /// <returns></returns>
        [HttpGet("current-period")]
        [ProducesResponseType(typeof(AppRamIntResult), 200)]
        public async Task<IActionResult> GetPeriod()
        {
            try
            {
                var result = await _memoryService.GetCurrentPeriod();
                return new JsonResult(new AppRamIntResult() { Success = true, Data = result });

            }
            catch (Exception e)
            {
                return await Task.FromResult(new JsonResult(new AppRamIntResult() { Success = false, Data = 0, Error = $"{e.Message} {e.InnerException?.Message}" }));
            }


        }

        /// <summary>
        /// ALL PROCESSES
        /// </summary>
        /// <remarks>
        /// Все процессы сервера
        /// </remarks>
        /// <returns></returns>
        [HttpGet("all-processes")]
        [ProducesResponseType(typeof(AppRamStringListResult), 200)]
        public async Task<IActionResult> GetProcesses()
        {
            try
            {
                var result = await _memoryService.GetAllProcesses();
                return new JsonResult(new AppRamStringListResult() { Success = true, Data = result });

            }
            catch (Exception e)
            {
                return await Task.FromResult(new JsonResult(new AppRamStringListResult() { Success = false, Error = $"{e.Message} {e.InnerException?.Message}" }));
            }


        }

        /// <summary>
        /// ALL WATCHED PROCESS
        /// </summary>
        /// <remarks>
        /// Все приложения для которых осуществляется сбор данных.
        /// </remarks>
        /// <returns></returns>
        [HttpGet("all-watched-processes")]
        [ProducesResponseType(typeof(AppRamListResult), 200)]
        public async Task<IActionResult> GetWatchedProcesses()
        {
            try
            {
                var result = await _memoryService.GetWatchedProcesses();
                return new JsonResult(new AppRamListResult() { Success = true, Data = result });

            }
            catch (Exception e)
            {
                return await Task.FromResult(new JsonResult(new AppRamListResult() { Success = false, Error = $"{e.Message} {e.InnerException?.Message}" }));
            }


        }


    }
}

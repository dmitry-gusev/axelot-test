using Microsoft.VisualStudio.TestTools.UnitTesting;
using AxelotTestAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using AxelotTestAPI.Domain.Interfaces;
using AxelotTestAPI.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using AxelotTestAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Results;
using System.Diagnostics;
using Newtonsoft.Json;
using AxelotTestAPI.Interfaces.Abstracts;
using System.Threading.Tasks;
using System.Threading;

namespace AxelotTestAPI.Controllers.Tests
{

    /// <summary>
    /// Тест контроллера Home
    /// </summary>
    [TestClass()]
    public class HomeControllerTests
    {
        /// <summary>
        /// Подготовка объектов к тесту
        /// </summary>
        /// <returns></returns>
        private (HomeController, MemoryService) GetController()
        {
            var logger = Mock.Of<ILogger<MemoryService>>();
            var logger_home = Mock.Of<ILogger<HomeController>>();
            var memoryService = new MemoryService(logger);
            var token = new CancellationTokenSource();
            memoryService.StartAsync(token.Token).GetAwaiter().GetResult();
            var home = new HomeController(logger_home, memoryService);
            return (home, memoryService);
        }

        /// <summary>
        /// Тест получения данных о процессе, приложении
        /// </summary>
        [TestMethod()]
        public void GetRamTest()
        {

            var (home, _) = GetController();
            var contentResult = home.GetRam("Мб", "svhost").Result as JsonResult;
            Assert.IsNotNull(contentResult, "Ответ метода GetRam не распознан");

            var resObj = contentResult.Value as AppRamResult;
            Assert.IsNotNull(contentResult, "Ответ метода GetRam содержит не корректные данные");
            Assert.IsTrue(resObj.Success, $"Ошибка в в методе GetRam, ошибочный ответ Success=false, Error={resObj.Error}");
            var data = resObj.Data;

            Assert.AreEqual("Мб", data.Scale, "Ошибочная еденица измерения памяти в ответе GetRam");
            Assert.AreEqual("svhost", data.ProcessName, "Ошибочная еденица измерения памяти в ответе GetRam");

            contentResult = home.GetRam("ПРП", "svhost").Result as JsonResult;
            resObj = contentResult.Value as AppRamResult;
            Assert.IsNotNull(contentResult, "Ответ метода GetRam содержит не корректные данные");
            Assert.IsFalse(resObj.Success, "Ошибка ответа метода GetRam на ошибочный параметр memScale");
        }

        /// <summary>
        /// Тест установки нового периода опроса для сервиса
        /// </summary>
        [TestMethod()]
        public void SetPeriodTest()
        {
            var (home, memService) = GetController();
            var contentResult = home.SetPeriod(20).Result as JsonResult;
            Assert.IsNotNull(contentResult, "Ответ метода SetPeriod не распознан");

            var resObj = contentResult.Value as AppRamStringResult;
            Assert.IsNotNull(contentResult, "Ответ метода SetPeriod содержит не корректные данные");
            Assert.IsTrue(resObj.Success, $"Ошибка в в методе SetPeriod, ошибочный ответ Success=false, Error={resObj.Error}");
            Assert.AreEqual(20, memService.GetCurrentPeriod().Result, "Установка периода обновления выполнена с ошибкой");
        }


        /// <summary>
        /// Тест метода получения текущего значения периода обновления
        /// </summary>
        [TestMethod()]
        public void GetPeriodTest()
        {
            var (home, memService) = GetController();
            var contentResult = home.GetPeriod().Result as JsonResult;
            Assert.IsNotNull(contentResult, "Ответ метода GetPeriod не распознан");

            var resObj = contentResult.Value as AppRamIntResult;
            Assert.IsNotNull(contentResult, "Ответ метода GetPeriod содержит не корректные данные");
            Assert.IsTrue(resObj.Success, $"Ошибка в в методе GetPeriod, ошибочный ответ Success=false, Error={resObj.Error}");
            var data = resObj.Data;

            Assert.AreEqual(5, data, "Значение периода по умолчанию не корректное");
        }
    }
}
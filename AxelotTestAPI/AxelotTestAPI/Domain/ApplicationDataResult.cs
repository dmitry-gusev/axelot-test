using AxelotTestAPI.Domain.Enums;
using AxelotTestAPI.Extentions;
using AxelotTestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AxelotTestAPI.Domain
{
    /// <summary>
    /// Потребление памяти приложением
    /// </summary>
    public class ApplicationDataResult
    {
        /// <summary>
        /// Название отслеживаемого приложения, процесса
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// Состояние
        /// </summary>
        /// <remarks>
        /// Если процесс неожиданно пропал, приложение закрылось, 
        /// он помечается как не активный
        /// </remarks>
        public bool IsActive { get; set; }

        /// <summary>
        /// Время последнего запроса данных по задаче
        /// </summary>
        public DateTime LastDataGave { get; set; }

        /// <summary>
        /// Время получения значения
        /// </summary>
        public DateTime ValueTime { get; set; }


        /// <summary>
        /// Значение потребляемой памяти
        /// </summary>
        public long Value { get; set; }

        /// <summary>
        /// Еденица измерения памяти
        /// </summary>
        public MemoryScale MemorySc { get; set; }

        /// <summary>
        /// Обработка результата
        /// </summary>
        /// <returns></returns>
        public string ValueToStringResult() => MemorySc switch
        {
            MemoryScale.Mb => ($"{ (float)Value/(1024 * 1024):f2}"),
            MemoryScale.Gb => ($"{ (float)Value/(1024 * 1024 * 1024):f2}"),
            _ => ""
        };

        /// <summary>
        /// Обновление данных о памяти
        /// </summary>
        /// <param name="source">Инстанс с новыми значениями</param>
        internal void Update(ApplicationDataResult source)
        {
            this.ValueTime = source.ValueTime;
            this.Value = source.Value;
        }

        /// <summary>
        /// Преобразование типов
        /// из текущего в результат данных API оперции
        /// </summary>
        /// <param name="source">Источник данных</param>
        public static implicit operator AppRamDataResult(ApplicationDataResult source)
        {
            return new AppRamDataResult() {
                ProcessName = source.ProcessName,
                ProcTime = source.ValueTime,
                Scale = source.MemorySc.GetDescription(),
                Value = $"{source.ValueToStringResult():f2}" 
            };
        }
    }
}

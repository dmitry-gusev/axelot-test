using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxelotTestAPI.Models
{
    /// <summary>
    /// Базовый класс результата API Операции
    /// </summary>
    public abstract class AppWebApiResultBase<T>
    {
        /// <summary>
        /// Признак выполнения операции True операция выполнена, False операция завершилась ошибкой
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Описание ошибки, если операция завершилась с ошибкой или пустая строка в другом случае
        /// </summary>
        public string Error { get; set; }

       /// <summary>
       /// Полезная нагрузка ответа, может принимать занчение null
       /// </summary>
        public abstract T Data { get; set; }

    }
}

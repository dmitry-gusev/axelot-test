using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace AxelotTestAPI.Domain.Enums
{
    /// <summary>
    /// Единицы измерения памяти
    /// </summary>
    public enum MemoryScale
    {
        /// <summary>
        /// В мегабайтах
        /// </summary>
        [Description("Мб")]
        Mb=0,
        /// <summary>
        /// В гигабайтах
        /// </summary>
        [Description("Гб")]
        Gb = 1

    }
}

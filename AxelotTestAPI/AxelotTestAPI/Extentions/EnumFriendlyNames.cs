using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AxelotTestAPI.Extentions
{

    /// <summary>
    /// Расширение для перечислений
    /// </summary>
    public static class EnumFriendlyNames
    {
        /// <summary>
        /// Получает содержимое атрибута Description для значения перечислений
        /// </summary>
        /// <remarks>
        /// Enum dig {
        ///     [Description("Один")]
        ///     one=0,
        ///     two=1
        /// }
        /// var k=dig.one;
        /// var k2=dig.two;
        /// GetDescription(k) //Один
        /// GetDescription(k2) //two
        /// </remarks>
        /// <param name="value">Значение</param>
        /// <returns>Строка</returns>
        public static string GetDescription(this Enum value)
        {
            string description = value.ToString();
            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)fi.GetCustomAttribute(typeof(DescriptionAttribute));

            return attribute?.Description ?? value.ToString();

        }

    }
}

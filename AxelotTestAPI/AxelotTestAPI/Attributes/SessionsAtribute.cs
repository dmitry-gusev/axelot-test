using AxelotTestAPI.Extentions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AxelotTestAPI.Attributes
{
    /// <summary>
    /// Проверяет ключ сессии, если его нет, устанавливает
    /// и обновляет время последнего запроса пользователя к API
    /// </summary>
    public class SessionsAtribute : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Проверка сессии
            string key = context.HttpContext.Session.Get<string>("Key");
            if (string.IsNullOrEmpty(key))
            {
                //Новый идентификатор
                key = Guid.NewGuid().ToString();
                //Добавляем в сессию
                context.HttpContext.Session.Set<string>("Key", key);
            }

            //Фиксируем время запроса
            context.HttpContext.Session.Set<DateTime>("LastTime", DateTime.Now);

            return base.OnActionExecutionAsync(context, next);
        }
    }
}

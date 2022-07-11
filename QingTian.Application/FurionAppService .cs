using Furion.DynamicApiController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace QingTian.Application
{
    [ApiDescriptionSettings("Application")]
    public class FurionAppService : IDynamicApiController
    {
        [AllowAnonymous]
        public string Get()
        {
            return $"GET 请求";
        }
        public string Post()
        {
            return $"POST 请求";
        }
        public string Delete()
        {
            return $"DELETE 请求";
        }
        public string Put()
        {
            return $"PUT 请求";
        }
        public string Patch()
        {
            return $"PATCH 请求";
        }
    }
}
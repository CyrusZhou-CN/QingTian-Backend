using Furion.DynamicApiController;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Application
{
    /// <inheritdoc cref="ITestInheritdoc"/>
    [Route("test"),ApiDescriptionSettings("Application")]
    public class TestInheritdoc : ITestInheritdoc, IDynamicApiController
    {
        /// <inheritdoc/>
        [HttpGet("name")]
        public string GetName(int e)
        {
            return "Furion";
        }

        /// <inheritdoc/>
        public string GetName2(int e)
        {
            return $"Furion:{e}";
        }
    }
    /// <summary>
    /// 测试注释续承
    /// </summary>
    public interface ITestInheritdoc
    {
        /// <summary>
        /// 获取名称
        /// </summary>
        /// <returns></returns>
        string GetName(int e);
        /// <summary>
        /// 获取名称
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        string GetName2(int e);
    }
}

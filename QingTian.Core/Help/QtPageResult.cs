using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 分页列表结果
    /// </summary>
    public static class QtPageResult
    {
        /// <summary>
        /// 替换sqlsugar分页
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static dynamic QtPagedResult<T>(this SqlSugarPagedList<T> page)
        {
            return new
            {
                Code= 200,
                Page = page.PageIndex,
                page.PageSize,
                Total = page.TotalCount,
                page.Items,
                Message = "ok"
            };
        }
    }
}

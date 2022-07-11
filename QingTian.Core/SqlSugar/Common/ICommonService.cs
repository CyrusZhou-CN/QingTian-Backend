using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    public interface ICommonService
    {
        /// <summary>
        /// 获取库表信息
        /// </summary>
        /// <param name="IsCache"></param>
        /// <returns></returns>
        Task<IEnumerable<EntityInfo>> GetEntityInfos(bool IsCache = true);
    }
}

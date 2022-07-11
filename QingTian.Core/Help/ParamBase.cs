using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{    /// <summary>
     /// 权限通用参数
     /// </summary>
    public class ParamBase : PageParamBase
    {
        /// <summary>
        /// 授权菜单
        /// </summary>
        public List<long> GrantMenuIdList { get; set; } = new List<long>();

        /// <summary>
        /// 授权角色
        /// </summary>
        public virtual List<long> GrantRoleIdList { get; set; } = new List<long>();

        /// <summary>
        /// 授权数据
        /// </summary>
        public virtual List<long> GrantOrgIdList { get; set; } = new List<long>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 用户角色服务
    /// </summary>
    public interface ISysUserRoleService
    {
        /// <summary>
        /// 根据角色Id删除用户角色表关联信息
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<int> DeleteUserRoleListByRoleIdAsync(long roleId);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> DeleteUserRoleListByUserIdAsync(long userId);
        /// <summary>
        /// 获取用户组织机构Id列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        Task<List<long>> GetUserRoleDataScopeIdListAsync(long userId, long orgId);
        /// <summary>
        /// 获取用户的角色Id列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<long>> GetUserRoleIdListAsync(long userId);
        /// <summary>
        /// 授权用户角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task GrantRoleAsync(UpdateUserParam param);
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public interface ISysUserService
    {
        /// <summary>
        /// 账号是否存在
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task IsAccountExist(QueryUserExistParam param);
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task AddUser(AddUserParam param);
        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task ChangeUserStatus(UpdateUserParam param);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task DeleteUser(DeleteUserParam param);
        /// <summary>
        /// 导出用户数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<IActionResult> ExportUser([FromQuery] UserParam param);
        /// <summary>
        /// 导入用户数据
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task ImportUser(IFormFile file);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> GetUser([FromQuery] QueryUserParam param);
        /// <summary>
        /// 根据用户Id获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<dynamic> GetUserById(long userId);
        /// <summary>
        /// 获取用户数据范围（机构Id集合）并缓存
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>   
        Task<List<long>> GetUserDataScopeIdList(long userId);
        /// <summary>
        /// 获取用户拥有数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> GetUserOwnData([FromQuery] QueryUserParam param);
        /// <summary>
        /// 获取用户拥有角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> GetUserOwnRole([FromQuery] QueryUserParam param);
        /// <summary>
        /// 获取用户选择器
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> GetUserSelector([FromQuery] UserParam param);
        /// <summary>
        /// 授权用户数据范围
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task GrantUserData(UpdateUserParam param);
        /// <summary>
        /// 授权用户角色
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task GrantUserRole(UpdateUserParam param);
        /// <summary>
        /// 分页查询用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> QueryUserPageList([FromQuery] UserParam param);
        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task ResetUserPwd(QueryUserParam param);
        /// <summary>
        /// 将OAuth账号转换成账号
        /// </summary>
        /// <param name="authUser"></param>
        /// <param name="sysUser"></param>
        /// <returns></returns>
        Task SaveAuthUserToUser(AuthUserParam authUser, UserParam sysUser);
        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateAvatar(UploadAvatarParam param);
        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateUser(UpdateUserParam param);
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateUserInfo(UpdateUserParam param);
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateUserPwd(UpdatePasswordUserParam param);
        /// <summary>
        /// 获取用户数据范围（用户Id集合）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<long>> GetDataScopeIdUserList(long userId);
        /// <summary>
        /// 检查普通用户数据范围
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        void CheckDataScopeByUserId(long userId);
        /// <summary>
        /// 检查普通用户数据范围
        /// </summary>
        /// <param name="orgId"></param>
        void CheckDataScope(long orgId);
    }
}

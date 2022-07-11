using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 系统缓存服务
    /// </summary>
    public interface ISysCacheService
    {
        /// <summary>
        /// 删除指定缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Del(string key);
        /// <summary>
        /// 删除指定缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> DelAsync(string key);
        /// <summary>
        /// 删除指定关键字开头的缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> DelByStartsWithAsync(string key);
        /// <summary>
        /// 获取所有缓存关键字
        /// </summary>
        /// <returns></returns>
        List<string> GetAllCacheKeys();
        /// <summary>
        /// 获取用户菜单缓存
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<MenuTreeNode>> GetMenuAsync(long userId);
        /// <summary>
        /// 获取用户权限缓存
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<string>> GetPermissionAsync(long userId);
        /// <summary>
        /// 缓存用户菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="menus"></param>
        /// <returns></returns>
        Task SetMenuAsync(long userId, List<MenuTreeNode> menus);
        /// <summary>
        /// 缓存用户权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        Task SetPermissionAsync(long userId, List<string> permissions);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool Set(string key, object value);
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> SetAsync(string key, object value);
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetAsync(string key);
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);
        /// <summary>
        /// 判断指定键是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(string key);
        /// <summary>
        /// 判断指定键是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(string key);
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        Task<List<string>> GetAllPermissionAsync();
        /// <summary>
        /// 缓存所有权限
        /// </summary>
        /// <param name="permissions"></param>
        /// <returns></returns>
        Task SetAllPermissionAsync(List<string> permissions);
        /// <summary>
        /// 获取数据范围缓存（用户Id集合）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<long>> GetUsersDataScope(long userId);
        /// <summary>
        /// 获取数据范围缓存（机构Id集合）
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<long>> GetDataScope(long userId);
        /// <summary>
        /// 缓存数据范围（用户Id集合）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dataScopes"></param>
        /// <returns></returns>
        Task SetUsersDataScope(long userId, List<long> dataScopes);
        /// <summary>
        /// 缓存数据范围（机构Id集合）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="dataScopes"></param>
        /// <returns></returns>
        Task SetDataScope(long userId, List<long> dataScopes);
    }
}

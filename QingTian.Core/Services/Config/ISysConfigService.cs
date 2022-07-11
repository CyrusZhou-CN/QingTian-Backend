using Microsoft.AspNetCore.Mvc;
using QingTian.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core.Services
{
    /// <summary>
    /// 系统参数配置服务
    /// </summary>
    public interface ISysConfigService
    {
        /// <summary>
        /// 增加系统参数配置
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task AddConfig(AddConfigParam param);
        /// <summary>
        /// 删除系统参数配置
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task DeleteConfigAsync(DeleteConfigParam param);
        /// <summary>
        /// 获取系统参数配置
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<SysConfig> GetConfigAsync([FromQuery] QueryConfigParam param);
        /// <summary>
        /// 获取系统参数配置列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> GetConfigListAsync();
        /// <summary>
        /// 分页获取系统参数配置
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<dynamic> QueryConfigPageListAsync([FromQuery] ConfigParam param);
        /// <summary>
        /// 更新系统参数配置
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task UpdateConfigAsync(UpdateConfigParam param);
        /// <summary>
        /// 获取验证码开关标识
        /// </summary>
        /// <returns></returns>
        Task<bool> GetCaptchaOpenFlagAsync();
        /// <summary>
        /// 获取默认密码
        /// </summary>
        /// <returns></returns>
        Task<string> GetDefaultPasswordAsync();
        /// <summary>
        /// 更新配置缓存
        /// </summary>
        /// <param name="code"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task UpdateConfigCacheAsync(string code, object value);
    }
}

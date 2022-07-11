using Furion.FriendlyException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QingTian.Core
{
    /// <summary>
    /// 自定义系统错误码
    /// </summary>
    [ErrorCodeType]
    public enum ErrorCode
    {
        /// <summary>
        /// 没有权限操作该数据
        /// </summary>
        [ErrorCodeItemMetadata("没有权限操作该数据")]
        E1013,

        /// <summary>
        /// 账号已存在
        /// </summary>
        [ErrorCodeItemMetadata("账号已存在")]
        E1003,

        /// <summary>
        /// 禁止删除超级管理员
        /// </summary>
        [ErrorCodeItemMetadata("禁止删除超级管理员")]
        E1014,

        /// <summary>
        /// 禁止自我毁灭
        /// </summary>
        [ErrorCodeItemMetadata("禁止自我毁灭")]
        E1001,

        /// <summary>
        /// 已经存在重复项
        /// </summary>
        [ErrorCodeItemMetadata("已经存在重复项")]
        E0005,

        /// <summary>
        /// 禁止删除系统参数
        /// </summary>
        [ErrorCodeItemMetadata("禁止删除系统参数")]
        E0006,

        /// <summary>
        /// 禁止修改超级管理员状态
        /// </summary>
        [ErrorCodeItemMetadata("禁止修改超级管理员状态")]
        E1015,

        /// <summary>
        /// 旧密码不匹配
        /// </summary>
        [ErrorCodeItemMetadata("旧密码输入错误")]
        E1004,

        /// <summary>
        /// 用户名或密码不正确
        /// </summary>
        [ErrorCodeItemMetadata("用户名或密码不正确")]
        E1000,

        /// <summary>
        /// 账号已冻结
        /// </summary>
        [ErrorCodeItemMetadata("账号已冻结")]
        E1017,
        /// <summary>
        /// 数据已存在
        /// </summary>
        [ErrorCodeItemMetadata("数据已存在")]
        E1006,
        /// <summary>
        /// 记录不存在
        /// </summary>
        [ErrorCodeItemMetadata("记录不存在")]
        E1002,
        /// <summary>
        /// 没有权限
        /// </summary>
        [ErrorCodeItemMetadata("没有权限")]
        E1016,

        /// <summary>
        /// 已有相同组织机构,编码或名称相同
        /// </summary>
        [ErrorCodeItemMetadata("已有相同组织机构")]
        E2002,

        /// <summary>
        /// 只能增加下级机构
        /// </summary>
        [ErrorCodeItemMetadata("只能增加下级机构")]
        E2006,

        /// <summary>
        /// 该机构下有员工禁止删除
        /// </summary>
        [ErrorCodeItemMetadata("该机构下有员工禁止删除")]
        E2004,

        /// <summary>
        /// 附属机构下有员工禁止删除
        /// </summary>
        [ErrorCodeItemMetadata("附属机构下有员工禁止删除")]
        E2005,

        /// <summary>
        /// 父机构不存在
        /// </summary>
        [ErrorCodeItemMetadata("父机构不存在")]
        E2000,

        /// <summary>
        /// 当前机构Id不能与父机构Id相同
        /// </summary>
        [ErrorCodeItemMetadata("当前机构Id不能与父机构Id相同")]
        E2001,

        /// <summary>
        /// 已存在同名或同编码职位
        /// </summary>
        [ErrorCodeItemMetadata("已存在同名或同编码职位")]
        E6000,
        /// <summary>
        /// 该职位下有员工禁止删除
        /// </summary>
        [ErrorCodeItemMetadata("该职位下有员工禁止删除")]
        E6001,

        /// <summary>
        /// 菜单已存在
        /// </summary>
        [ErrorCodeItemMetadata("菜单已存在")]
        E4000,

        /// <summary>
        /// 路由地址为空
        /// </summary>
        [ErrorCodeItemMetadata("路由地址为空")]
        E4001,

        /// <summary>
        /// 权限标识格式为空
        /// </summary>
        [ErrorCodeItemMetadata("权限标识格式为空")]
        E4003,

        /// <summary>
        /// 权限标识格式错误
        /// </summary>
        [ErrorCodeItemMetadata("权限标识格式错误")]
        E4004,

        /// <summary>
        /// 父级菜单不能为当前节点，请重新选择父级菜单
        /// </summary>
        [ErrorCodeItemMetadata("父级菜单不能为当前节点，请重新选择父级菜单")]
        E4006,
        /// <summary>
        /// 禁止删除系统菜单
        /// </summary>
        [ErrorCodeItemMetadata("禁止删除系统菜单")]
        E4007,

        /// <summary>
        /// 禁止 停用/隐藏 系统菜单
        /// </summary>
        [ErrorCodeItemMetadata("禁止 停用/隐藏 系统菜单")]
        E4008,

        /// <summary>
        /// 通知消息状态错误
        /// </summary>
        [ErrorCodeItemMetadata("通知消息状态错误")]
        E7000,

        /// <summary>
        /// 通知消息删除失败
        /// </summary>
        [ErrorCodeItemMetadata("通知消息删除失败")]
        E7001,

        /// <summary>
        /// 通知消息状态错误
        /// </summary>
        [ErrorCodeItemMetadata("通知消息状态错误")]
        E7002,

        /// <summary>
        /// 字典值已存在
        /// </summary>
        [ErrorCodeItemMetadata("字典值已存在,名称或编码重复")]
        E3003,

        /// <summary>
        /// 字典值不存在
        /// </summary>
        [ErrorCodeItemMetadata("字典值不存在")]
        E3004,

        /// <summary>
        /// 字典状态错误
        /// </summary>
        [ErrorCodeItemMetadata("字典状态错误")]
        E3005,

        /// <summary>
        /// 字典类型已存在
        /// </summary>
        [ErrorCodeItemMetadata("字典类型已存在,名称或编码重复")]
        E3001,

        /// <summary>
        /// 字典类型不存在
        /// </summary>
        [ErrorCodeItemMetadata("字典类型不存在")]
        E3000,

        /// <summary>
        /// 该表代码模板已经生成过
        /// </summary>
        [ErrorCodeItemMetadata("该表代码模板已经生成过")]
        E1400,

        /// <summary>
        /// 父菜单不存在
        /// </summary>
        [ErrorCodeItemMetadata("父菜单不存在")]
        E1505,
        /// <summary>
        /// 请添加数据列
        /// </summary>
        [ErrorCodeItemMetadata("请添加数据列")]
        db1000,
        /// <summary>
        /// 数据表不存在
        /// </summary>
        [ErrorCodeItemMetadata("数据表不存在")]
        db1001,
        /// <summary>
        /// QingTian.Core.Entity中已经存在相同的实体
        /// </summary>
        [ErrorCodeItemMetadata("QingTian.Core.Entity中已经存在相同的实体")]
        db1002,
        /// <summary>
        /// 父类不存在
        /// </summary>
        [ErrorCodeItemMetadata("父类不存在")]
        db1003,

    }
}

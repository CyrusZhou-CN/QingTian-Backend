using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{

    public class CodeGenPageParam : ParamBase
    {
        /// <summary>
        /// 代码生成器Id
        /// </summary>
        public virtual long Id { get; set; }
        /// <summary>
        /// 作者姓名
        /// </summary>
        public virtual string AuthorName { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public virtual string ClassName { get; set; }

        /// <summary>
        /// 是否移除表前缀
        /// </summary>
        public virtual string TablePrefix { get; set; }

        /// <summary>
        /// 生成方式
        /// </summary>
        public virtual GenerateType GenerateType { get; set; }

        /// <summary>
        /// 数据库表名
        /// </summary>
        public virtual string TableName { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        public virtual string NameSpace { get; set; }

        /// <summary>
        /// 业务名（业务代码包名称）
        /// </summary>
        public virtual string BusName { get; set; }

        /// <summary>
        /// 功能名（数据库表名称）
        /// </summary>
        public virtual string TableComment { get; set; }

        /// <summary>
        /// 菜单父级
        /// </summary>
        public virtual long MenuPid { get; set; }
    }
}
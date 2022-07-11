using SqlSugar;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 递增主键实体基类
    /// </summary>
    public abstract class AutoIncrementEntity : IEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsIdentity = true, ColumnDescription = "Id主键", IsPrimaryKey = true)] //通过特性设置主键和自增列 
        // 注意是在这里定义你的公共实体
        public virtual int Id { get; set; }
        public virtual void Create()
        {

        }

        public virtual void Modify()
        {
        }
    }
    public class PrimaryKeyEntity : IEntity
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(ColumnDescription = "Id主键", IsPrimaryKey = true)]
        // 注意是在这里定义你的公共实体
        public virtual long Id { get; set; }
        public virtual void Create()
        {
            if (Id <= 0)
            {
                Id = SnowFlakeSingle.Instance.NextId();
            }
        }

        public virtual void Modify()
        {
        }
    }
}
using Furion;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Entity
{
    /// <summary>
    /// 实体基类
    /// </summary>
    public class DbEntityBase : PrimaryKeyEntity, IEntity
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnDescription = "创建时间", IsNullable = true)]
        public virtual DateTime? CreatedTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(ColumnDescription = "更新时间", IsNullable = true)]
        public virtual DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 创建者Id
        /// </summary>
        [SugarColumn(ColumnDescription = "创建者Id", IsNullable = true)]
        public virtual long? CreatedUserId { get; set; }

        /// <summary>
        /// 创建者名称
        /// </summary>
        [MaxLength(20)]
        [SugarColumn(ColumnDescription = "创建者名称", IsNullable = true)]
        public virtual string CreatedUserName { get; set; }

        /// <summary>
        /// 修改者Id
        /// </summary>
        [SugarColumn(ColumnDescription = "修改者Id", IsNullable = true)]
        public virtual long? ModifyUserId { get; set; }

        /// <summary>
        /// 修改者名称
        /// </summary>
        [MaxLength(20)]
        [SugarColumn(ColumnDescription = "修改者名称", IsNullable = true)]
        public virtual string ModifyUserName { get; set; }

        /// <summary>
        /// 软删除
        /// </summary>
        [SugarColumn(ColumnDescription = "软删除")]
        public virtual bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Create
        /// </summary>
        public override void Create()
        {
            base.Create();
            var userId = App.User?.FindFirst(ConstClaim.UserId)?.Value;
            var userName = App.User?.FindFirst(ConstClaim.Name)?.Value;
            CreatedTime = DateTime.Now;
            if (!string.IsNullOrEmpty(userId))
            {
                CreatedUserId = long.Parse(userId);
                CreatedUserName = userName;
            }
        }
        /// <summary>
        /// Modify
        /// </summary>
        public override void Modify()
        {
            base.Modify();
            var userId = App.User?.FindFirst(ConstClaim.UserId)?.Value;
            var userName = App.User?.FindFirst(ConstClaim.Name)?.Value;
            ModifyTime = DateTime.Now;
            if (!string.IsNullOrEmpty(userId))
            {
                ModifyUserId = long.Parse(userId);
                ModifyUserName = userName;
            }
        }

        /// <summary>
        /// 更新信息列
        /// </summary>
        /// <returns></returns>
        public virtual string[] UpdateColumn()
        {
            var result = new[] { nameof(ModifyUserId), nameof(ModifyUserName), nameof(ModifyTime) };
            return result;
        }

        /// <summary>
        /// 假删除的列，包含更新信息
        /// </summary>
        /// <returns></returns>
        public virtual string[] FalseDeleteColumn()
        {
            var updateColumn = UpdateColumn();
            var deleteColumn = new[] { nameof(IsDeleted) };
            var result = new string[updateColumn.Length + deleteColumn.Length];
            deleteColumn.CopyTo(result, 0);
            updateColumn.CopyTo(result, deleteColumn.Length);
            return result;
        }
    }
}
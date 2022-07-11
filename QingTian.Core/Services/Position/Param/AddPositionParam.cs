using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class AddPositionParam: PositionParam
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required(ErrorMessage = "职位名称不能为空")]
        public override string Name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "职位编码不能为空")]
        public override string Code { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class QueryConfigParam : ConfigParam
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        [Required(ErrorMessage = "应用Id不能为空")]
        public long Id { get; set; }
    }
}
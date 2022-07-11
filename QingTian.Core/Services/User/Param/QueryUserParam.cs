using System.ComponentModel.DataAnnotations;

namespace QingTian.Core.Services
{
    public class QueryUserParam : UserParam
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage = "用户Id不能为空")]
        public long Id { get;  set; }
    }
    public class QueryUserExistParam
    {
        public long Id { get; set; }
        public string Account { get; set; }
    }
}
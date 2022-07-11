namespace QingTian.Core.Services
{
    public class CreateEntityParam
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>student</example>
        public string TableName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>Student</example>
        public string EntityName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>AutoIncrementEntity</example>
        public string BaseClassName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>QingTian.Application</example>
        public string Position { get; set; }
    }
}
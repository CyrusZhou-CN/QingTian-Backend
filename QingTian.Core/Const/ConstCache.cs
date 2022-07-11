namespace QingTian.Core
{
    public class ConstCache
    {
        /// <summary>
        /// 数据范围缓存
        /// </summary>
        public const string CACHE_KEY_DATASCOPE = "datascope_";
        public const string CACHE_KEY_USERSDATASCOPE = "usersdatascope_";
        /// <summary>
        /// 所有权限缓存
        /// </summary>
        public static string CACHE_KEY_ALLPERMISSION = "allpermission";
        /// <summary>
        /// 权限缓存
        /// </summary>
        public static string CACHE_KEY_PERMISSION = "permission_";
        
        /// <summary>
        /// 菜单缓存
        /// </summary>
        public static string CACHE_KEY_MENU = "menu_";

        /// <summary>
        /// 库表实体信息缓存
        /// </summary>
        public static string CACHE_KEY_ENTITYINFO = "tableentity";

        /// <summary>
        /// 验证码缓存
        /// </summary>
        public const string CACHE_KEY_CODE = "cachekeycode_";

        /// <summary>
        /// 删除字段
        /// </summary>
        public const string DELETE_FIELD = "IsDeleted";
    }
}
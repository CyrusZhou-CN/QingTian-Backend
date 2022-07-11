namespace QingTian.Core.Services
{
    public class DbTableInfoParam
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<DbColumnInfoParam> DbColumnInfoList { get; set; }
    }
}
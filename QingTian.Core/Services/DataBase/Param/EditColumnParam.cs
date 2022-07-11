namespace QingTian.Core.Services
{
    public class EditColumnParam
    {
        public string TableName { get; set; }
        public string OldName { get; set; }
        public string DbColumnName { get; set; }
        public string ColumnDescription { get; set; }
    }
}
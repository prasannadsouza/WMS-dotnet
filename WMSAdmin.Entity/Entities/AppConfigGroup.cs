namespace WMSAdmin.Entity.Entities
{
    public class AppConfigGroup
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
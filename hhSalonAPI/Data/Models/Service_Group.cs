using System.ComponentModel.DataAnnotations.Schema;

namespace hhSalonAPI.Data.Models
{
    public class Service_Group
    {
        [Column("service_id")]
        public int ServiceId { get; set; }
        public Service Service { get; set; }

        [Column("group_id")]
        public int GroupId { get; set; }
        public GroupOfServices Group { get; set; }
    }
}

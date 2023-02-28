using hhSalonAPI.Data.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hhSalonAPI.Data.Models
{
    [Index(propertyNames: nameof(Name), IsUnique = true)]
    public class Service:IEntityBase
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Column("name")]
        [StringLength(45)]
        public string Name { get; set; }

        [Required]
        [Column("price")]
        public double Price { get; set; }

        public Service_Group Service_Group { get; set; }
    }
}

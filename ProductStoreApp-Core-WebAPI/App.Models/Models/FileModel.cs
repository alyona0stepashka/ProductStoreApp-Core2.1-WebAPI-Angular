using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace App.Models
{
    public class FileModel
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int IdProduct { get; set; }

        public string Name { get; set; }
        public string Path { get; set; }

        [JsonIgnore]
        public virtual Product Product { get; set; }
    }
}
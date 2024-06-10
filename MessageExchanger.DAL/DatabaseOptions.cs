using System.ComponentModel.DataAnnotations;

namespace MessageExchanger.DAL
{
    public class DatabaseOptions
    {
        [Required]
        public string ConnectionString { get; set; } = null!;
    }
}

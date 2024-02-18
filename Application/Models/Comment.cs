using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{   
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        // Based on our code the .NET core will automatically create a foreign key for us
        // and ensure the relationship between the two tables
        public int? StockId { get; set; }

        // Navigation property
        // This property will allow us to navigate to other side of the relationship
        public Stock? Stock { get; set; }

        // Define a one to one relationship between the AppUser and Comment
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
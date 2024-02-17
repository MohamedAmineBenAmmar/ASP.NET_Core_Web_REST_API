using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs.Comment
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        // Based on our code the .NET core will automatically create a foreign key for us
        // and ensure the relationship between the two tables
        public int? StockId { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Models
{
    [Table("Portfolios")]
    // This model represents a join table between the User and Stock models
    public class Portfolio
    {
        // Setting up the foreign keys
        public string AppUserId { get; set; }
        public int StockId { get; set; }

        // Setting up navigation properties ()just needed for us us developers)
        public AppUser AppUser { get; set; }
        public Stock Stock { get; set; }
    }
}
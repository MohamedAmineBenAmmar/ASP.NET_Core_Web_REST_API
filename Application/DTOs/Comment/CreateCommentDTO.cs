using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Application.DTOs.Comment
{
    public class CreateCommentDTO
    {
        [Required]
        [MinLength(5, ErrorMessage = "Title must be at least 5 characters long")]
        [MaxLength(250, ErrorMessage = "Title must be at most 250 characters long")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MinLength(5, ErrorMessage = "Content must be at least 5 characters long")]
        [MaxLength(250, ErrorMessage = "Content must be at most 250 characters long")]
        public string Content { get; set; } = string.Empty;
    }
}
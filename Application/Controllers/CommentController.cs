using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Mappers;
using Application.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController: ControllerBase
    {
        private readonly CommentRepository _commentRepository;
        public CommentController(CommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var comments = await _commentRepository.GetAllCommentsAsync();
            var commentsDTOs = comments.Select(comment => comment.ToCommentDTO());
            return Ok(commentsDTOs);
        }
    }
}
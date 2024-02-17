using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Comment;
using Application.Interfaces;
using Application.Mappers;
using Application.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        public CommentController(CommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            // Telling the controller to perform the data validation that we specified
            // in the DTO class
            // This is a common pattern to perform model validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comments = await _commentRepository.GetAllCommentsAsync();
            var commentsDTOs = comments.Select(comment => comment.ToCommentDTO());
            return Ok(commentsDTOs);
        }

        // Example of URL constraint
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _commentRepository.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDTO());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, CreateCommentDTO commentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _stockRepository.StockExistsAsync(stockId))
            {
                return BadRequest("Stock does not exist");
            }

            var commentModel = commentDTO.ToCommentFromCreate(stockId);
            await _commentRepository.CreateCommentAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { id = commentModel }, commentModel.ToCommentDTO());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var comment = await _commentRepository.DeleteCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound("Comment does not exist");
            }

            return NoContent();
        }
    }
}
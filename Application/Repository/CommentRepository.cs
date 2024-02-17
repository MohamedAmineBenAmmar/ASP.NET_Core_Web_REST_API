using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Data;
using Application.Interfaces;
using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _dbcontext;
        public CommentRepository(ApplicationDBContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Comment> CreateCommentAsync(Comment commentModel)
        {
            await _dbcontext.Comments.AddAsync(commentModel);
            await _dbcontext.SaveChangesAsync();

            return commentModel;
        }

        public async Task<Comment?> DeleteCommentByIdAsync(int id)
        {
            var comment = await _dbcontext.Comments.FirstOrDefaultAsync(comment => comment.Id == id);
            if (comment == null)
            {
                return null;
            }

            _dbcontext.Comments.Remove(comment);
            await _dbcontext.SaveChangesAsync();

            return comment;
        }

        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            var stocks = await _dbcontext.Comments.ToListAsync();
            return stocks;
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            var comment = await _dbcontext.Comments.FirstOrDefaultAsync(comment => comment.Id == id);
            if (comment == null)
            {
                return null;
            }

            return comment;
        }
    }
}
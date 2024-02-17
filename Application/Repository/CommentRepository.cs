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
       
        public async Task<List<Comment>> GetAllCommentsAsync()
        {
            var stocks = await _dbcontext.Comments.ToListAsync();
            return stocks;
        }
    }
}
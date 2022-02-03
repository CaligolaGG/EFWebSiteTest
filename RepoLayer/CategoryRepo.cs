using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DataLayer;

namespace RepoLayer
{
    /// <summary>
    /// Class to interact with the Category table in the db
    /// </summary>
    public class CategoryRepo
    {
        private MyDbContext _ctx;

        public CategoryRepo(MyDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<Category>> GetAllAsync() => await _ctx.Categories.ToListAsync();
           
        

    }

}


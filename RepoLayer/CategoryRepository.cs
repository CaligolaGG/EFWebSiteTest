﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain;
using RepositoryLayer.Interfaces;

namespace RepositoryLayer
{
    /// <summary>
    /// Class to interact with the Category table in the db
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private MyDbContext _ctx;

        public CategoryRepository(MyDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<List<Category>> GetAllAsync() => await _ctx.Categories.ToListAsync();



    }

}

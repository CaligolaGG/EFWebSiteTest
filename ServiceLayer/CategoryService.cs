using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain;
using RepoLayer;


namespace ServiceLayer
{
    public class CategoryService
    {
        private ICategoryRepository _categoryRepo;
        public CategoryService(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<List<Category>> GetAllAsync() => await _categoryRepo.GetAllAsync();
        
    }
}

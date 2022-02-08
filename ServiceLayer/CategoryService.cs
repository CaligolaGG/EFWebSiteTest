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
        private CategoryRepo _categoryRepo;
        public CategoryService(CategoryRepo categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<List<Category>> GetAllAsync() => await _categoryRepo.GetAllAsync();
        
    }
}

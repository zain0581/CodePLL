using codepulse.API.Data;
using codepulse.API.Modells.Domain;
using codepulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace codepulse.API.Repositories.Implementation
{
    public class CategoryRepo : ICategory
    {
        private readonly dbContext dbContext;
        public CategoryRepo(dbContext dbcontext)
        {
                this.dbContext = dbcontext;
        }
        public  async Task<Category> CreateAsync(Category ctegory)
        {
            await dbContext.Categories.AddAsync(ctegory);
            await dbContext.SaveChangesAsync();
            return ctegory;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetById(Guid id)
        {
          return await dbContext.Categories.FirstOrDefaultAsync(c=>c.Id == id);
        }

        public async Task<Category?> GetByName(string name)
        {
          return await dbContext.Categories.FirstAsync(c=>c.Name == name);
        }

        public async Task<Category?> EditCategory(Category category)
        {
            var categ = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == category.Id);
            if (categ != null) 
            { 
                dbContext.Entry(categ).CurrentValues.SetValues(category);
                await dbContext.SaveChangesAsync();
                return category;
            }
            return null;
            
        }

        public async Task<Category?> DeleteCategoryById(Guid id)
        {
            var existingCategory = await dbContext.Categories.FirstOrDefaultAsync(x=>x.Id == id);
            if(existingCategory== null)
            {
                return null;
            }
            if (existingCategory.Id == id)
            {
                dbContext.Categories.Remove(existingCategory);
                await dbContext.SaveChangesAsync();
                return existingCategory;
            }
            return null;
        }
    }
}

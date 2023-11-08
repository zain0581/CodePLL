using codepulse.API.Data;
using codepulse.API.Modells.Domain;
using codepulse.API.Repositories.Interface;

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

      
    }
}

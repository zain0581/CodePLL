using codepulse.API.Modells.Domain;

namespace codepulse.API.Repositories.Interface
{
    public interface ICategory
    {
        Task<Category> CreateAsync(Category ctegory);
        Task<IEnumerable<Category>> GetAllAsync();

        Task<Category?> GetById(Guid id);

        Task<Category?> EditCategory(Category category);

        Task<Category?> DeleteCategoryById(Guid id);
        Task<Category?> GetByName(string name);    
        

    }
}

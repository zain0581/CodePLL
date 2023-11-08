using codepulse.API.Modells.Domain;

namespace codepulse.API.Repositories.Interface
{
    public interface ICategory
    {
        Task<Category> CreateAsync(Category ctegory);
    }
}

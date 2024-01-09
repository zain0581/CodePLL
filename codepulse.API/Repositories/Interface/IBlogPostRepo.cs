using codepulse.API.Modells.Domain;

namespace codepulse.API.Repositories.Interface
{
    public interface IBlogPostRepo
    {
        Task<BlogPost> CreateBlogPost(BlogPost post);

        Task<IEnumerable<BlogPost>> GetAllAsync();
    }
}

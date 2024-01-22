using codepulse.API.Modells.Domain;

namespace codepulse.API.Repositories.Interface
{
    public interface IBlogPostRepo
    {
        Task<BlogPost> CreateBlogPost(BlogPost post);

        Task<IEnumerable<BlogPost>> GetAllAsync();

        Task <BlogPost?> GetById(Guid id);

        Task <BlogPost?> GetByTitle( string title);
        Task<BlogPost?> GetByUrlHandle(string UrlHandle);

        Task <BlogPost?> EditBlogPost(BlogPost post);    

        Task <BlogPost?> DeletBlogPostById(Guid id);
    }
}

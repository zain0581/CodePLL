using codepulse.API.Data;
using codepulse.API.Modells.Domain;
using codepulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace codepulse.API.Repositories.Implementation
{
    public class BlogPostRepo : IBlogPostRepo
    {
        private readonly dbContext dbContext;
        public BlogPostRepo(dbContext dbContext)
        {
            this.dbContext = dbContext; 
                
        }

        public async Task<BlogPost> CreateBlogPost(BlogPost post)
        {
           await dbContext.BlogPosts.AddAsync(post);
            await dbContext.SaveChangesAsync(); 
            return post;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
          return  await dbContext.BlogPosts.ToListAsync();
            
        }
    }
}

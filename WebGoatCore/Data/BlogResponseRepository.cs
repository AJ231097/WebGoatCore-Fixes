using Microsoft.EntityFrameworkCore;
using WebGoatCore.Models;

namespace WebGoatCore.Data
{
    public class BlogResponseRepository
    {
        private readonly NorthwindContext _context;

        public BlogResponseRepository(NorthwindContext context)
        {
            _context = context;
        }

        public void CreateBlogResponse(BlogResponse response)
        {
            //TODO: should put this in a try/catch
            // Use EntityFramework FromSQLRaw for faster query
            var responseBack = _context.BlogResponses.FromSqlRaw(
                $"INSERT INTO BlogResponses (Author, BlogEntryId, ResponseDate, Contents) VALUES ( '{response.Author}', '{response.BlogEntryId}', '{response.ResponseDate}', '{response.Contents}' ); SELECT * FROM BlogResponses WHERE changes() = 1 AND Id = last_insert_rowid();").ToListAsync();

            

        }
    }
}

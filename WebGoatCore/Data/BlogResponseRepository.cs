using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
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
            

            var c = response.Contents;
            var a = response.Author;
            var bid = response.BlogEntryId;
            var rd = response.ResponseDate;
            var responseBack = _context.BlogResponses.FromSqlInterpolated(
                $"INSERT INTO BlogResponses (Author, BlogEntryId, ResponseDate, Contents) VALUES ( {a}, {bid}, {rd}, {c} ); SELECT * FROM BlogResponses WHERE changes() = 1 AND Id = last_insert_rowid();").ToListAsync();




        }
    }
}

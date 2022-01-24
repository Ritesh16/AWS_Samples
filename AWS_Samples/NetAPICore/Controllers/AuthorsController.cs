using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetAPICore.Data;
using NetAPICore.Entities;

namespace NetAPICore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly BookContext bookContext;

        public AuthorsController(BookContext bookContext)
        {
            this.bookContext = bookContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> Get()
        {
            return await bookContext.Authors.Include(a => a.Books).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> Get(int id)
        {
            var author = await bookContext.Authors.Include(a => a.Books).FirstOrDefaultAsync(x => x.AuthorId == id);
            if (author == null)
            {
                return NotFound();
            }
            return author;
        }
    }
}

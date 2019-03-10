#region

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdeaPool.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#endregion

namespace IdeaPool.Api.Controllers
{
    public class IdeasController: BaseController
    {
        private readonly IdeaPoolContext _context;

        public IdeasController(IdeaPoolContext context): base(context) 
            => _context = context;

        // DELETE: api/Idea/5
        [HttpDelete]
        public async Task<ActionResult<Idea>> Delete(int id)
        {
            var idea = await _context.Idea.FindAsync(id);
            if(idea == null) return NotFound();

            _context.Idea.Remove(idea);
            await _context.SaveChangesAsync();

            return idea;
        }

        // GET: api/Idea
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Idea>>> Get() 
            => await _context.Idea.ToListAsync();

        // GET: api/Idea/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Idea>> Get(int id)
        {
            var idea = await _context.Idea.FindAsync(id);

            if(idea == null) return NotFound();

            return idea;
        }

        private bool IdeaExists(int id) 
            => _context.Idea.Any(e => e.Id == id);

        // POST: api/Idea
        [HttpPost]
        public async Task<ActionResult<Idea>> Post(Idea idea)
        {
            _context.Idea.Add(idea);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = idea.Id }, idea);
        }

        // PUT: api/Idea/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Put(int id, Idea idea)
        {
            if(id != idea.Id) return BadRequest();

            _context.Entry(idea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!IdeaExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }
    }
}
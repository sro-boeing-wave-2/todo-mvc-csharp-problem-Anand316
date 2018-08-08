using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_api_2.Models;

namespace Web_api_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly NotesContext _context;

        public NotesController(NotesContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        [HttpGet]
        public IEnumerable<Note> GetNote()
        {
            return  _context.Note.Include(i => i.CList).Include(i => i.Labels);
        }

        // GET: api/Notes/5
        [HttpGet("{title}")]
        public async Task<IActionResult> GetNoteByTitle([FromRoute] string title)
        {
            List<Note> x = new List<Note>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Note.Include(i => i.CList).Include(i => i.Labels).Where(z => z.Title == title).ToListAsync();

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        [HttpGet("Pin/{pin}")]
        public async Task<IActionResult> GetNoteByPin([FromRoute] bool pin)
        {
            List<Note> x = new List<Note>();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Note.Include(i => i.CList).Include(i => i.Labels).Where(z => z.Pin == pin).ToListAsync();

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        [HttpGet("getLabel/{text}")]
        public async Task<IActionResult> GetNoteByLabel([FromRoute] string Text)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Note.Include(i=>i.CList).Include(i=>i.Labels).Where(z => z.Labels.Exists(x=>x.Text==Text )).ToListAsync();

            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        } 

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNote([FromRoute] int id, [FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != note.ID)
            {
                return BadRequest();
            }
            //GetNote().Where(i => i.ID == id).ForEachAsync(z => { z = note; });


            _context.Note.Update(note);
            //_context.Entry(note).State = EntityState.Modified;
            

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Notes
        [HttpPost]
        public async Task<IActionResult> PostNote([FromBody] Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Note.Add(note);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNote", new { id = note.ID }, note);
        }

        // DELETE: api/Notes/5
        [HttpDelete("delete/{title}")]
        public async Task<IActionResult> DeleteNote([FromRoute] string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var note = await _context.Note.Include(i => i.CList).Include(i => i.Labels).Where(z => z.Title == title).ToListAsync();
            if (note == null)
            {
                return NotFound();
            }

            _context.Note.RemoveRange(note);
            await _context.SaveChangesAsync();

            return Ok(note);
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.ID == id);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController : ControllerBase
    {

        public TestoviContext Context { get; set; }

        public TagController(TestoviContext context)
        {
            Context = context;
        }

        /* [Route("PreuzmiTaga/{id}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiTaga(int id)
        {
            return StatusCode(404, "Tag not found...");
        } */

        [Route("PreuzmiTagove")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiTagove()
        {
            return Ok(Context.Tagovi);
        }

        [Route("PreuzmiTag/{index}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiTag(int index)
        {
            // Lazy loading
            // Eager loading - cela baza uključena
            // Explicit loading - samo podaci koje mi želimo

            var tagovi = Context.Tagovi;

            var tag = await tagovi.Where(p => p.ID == index).ToListAsync();

            return Ok(tag[0]);
        }

        [Route("DodajTag")]
        [HttpPost]
        public async Task<ActionResult> DodajTag([FromBody] Tag tag)
        {
            if (string.IsNullOrWhiteSpace(tag.Title) || tag.Title.Length > 50)
            {
                return BadRequest("Ne valja Title...");
            }

            try
            {
                Context.Tagovi.Add(tag); // Ne upisuje odmah u DB
                int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                // return Ok($"Sve je u redu! ID novog taga je: {tag.ID}"); // DB ažurira i model pa sada znamo ID
                return Ok(tag); // DB ažurira i model pa sada znamo ID
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("PromeniTag/{ID}")]
        [HttpPut]
        public async Task<ActionResult> PromeniTag(int ID, string title)
        {
            if (ID < 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            if (string.IsNullOrWhiteSpace(title) || title.Length > 50)
            {
                return BadRequest("Ne valja title...");
            }

            try
            {
                var tag = Context.Tagovi.Where(s => s.ID == ID).FirstOrDefault(); // var menja bilo koji tip

                if (tag != null)
                {
                    tag.Title = title;

                    int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                    return Ok($"Tag ID = {tag.ID} je uspešno izmenjen"); // DB ažurira i model pa sada znamo ID
                }
                else
                {
                    return BadRequest("Tag nije pronađen!");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            // return StatusCode(404, "Tag not found...");
        }

        [Route("IzbrisiTag/{id}")]
        [HttpDelete]
        public async Task<ActionResult> IzbrisiTag(int id)
        {
            if (id < 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            try
            {
                var tag = await Context.Tagovi.FindAsync(id);

                if (tag != null)
                {
                    Context.Tagovi.Remove(tag);
                    int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                    // return Ok($"Tag ID = {tag.ID} | Title = {tag.Title} je uspešno izbrisan!"); // DB ažurira i model pa sada znamo ID
                    return Ok(tag); // DB ažurira i model pa sada znamo ID
                }
                else
                {
                    return BadRequest("Tag nije pronađen!");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

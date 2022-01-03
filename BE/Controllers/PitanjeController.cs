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
    public class PitanjeController : ControllerBase
    {

        public TestoviContext Context { get; set; }

        public PitanjeController(TestoviContext context)
        {
            Context = context;
        }

        /* [Route("PreuzmiPitanjea/{id}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiPitanjea(int id)
        {
            return StatusCode(404, "Pitanje not found...");
        } */

        [Route("PreuzmiPitanja")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiPitanja()
        {
            var pitanja = await Context.Pitanja.OrderByDescending(x => x.DateCreated).ToListAsync();
            return Ok(pitanja);
        }

        [Route("PreuzmiPitanje/{index}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiPitanje(int index)
        {
            // Lazy loading
            // Eager loading - cela baza uključena
            // Explicit loading - samo podaci koje mi želimo

            var pitanja = Context.Pitanja;

            var pitanje = await pitanja.Where(p => p.ID == index).ToListAsync();

            return Ok(pitanje[0]);
        }

        [Route("DodajPitanje")]
        [HttpPost]
        public async Task<ActionResult> DodajPitanje([FromBody] Pitanje pitanje)
        {
            if (string.IsNullOrWhiteSpace(pitanje.Question) || pitanje.Question.Length > 100)
            {
                return BadRequest("Ne valja Pitanje...");
            }
            if (string.IsNullOrWhiteSpace(pitanje.Answer) || pitanje.Answer.Length > 100)
            {
                return BadRequest("Ne valja Odgovor...");
            }

            try
            {
                pitanje.DateCreated = DateTime.Now;
                Context.Pitanja.Add(pitanje); // Ne upisuje odmah u DB
                int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                // return Ok($"Sve je u redu! ID novog pitanja je: {pitanje.ID}"); // DB ažurira i model pa sada znamo ID
                return Ok(pitanje); // DB ažurira i model pa sada znamo ID
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("PromeniPitanje")]
        [HttpPut]
        public async Task<ActionResult> PromeniPitanje([FromBody] Pitanje pitanje)
        {
            if (pitanje.ID < 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            if (string.IsNullOrWhiteSpace(pitanje.Question) || pitanje.Question.Length > 50)
            {
                return BadRequest("Ne valja question...");
            }

            if (string.IsNullOrWhiteSpace(pitanje.Answer) || pitanje.Answer.Length > 50)
            {
                return BadRequest("Ne valja Answer...");
            }

            try
            {
                var p = Context.Pitanja.Where(s => s.ID == pitanje.ID).FirstOrDefault(); // var menja bilo koji tip

                if (p != null)
                {
                    p.Question = pitanje.Question;
                    p.Answer = pitanje.Answer;
                    p.isArchived = pitanje.isArchived;

                    int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                    return Ok($"Pitanje ID = {pitanje.ID} je uspešno izmenjeno"); // DB ažurira i model pa sada znamo ID
                }
                else
                {
                    return BadRequest("pitanje nije pronađeno!");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            // return StatusCode(404, "pitanje not found...");
        }

        [Route("IzbrisiPitanje/{id}")]
        [HttpDelete]
        public async Task<ActionResult> IzbrisiPitanje(int id)
        {
            if (id < 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            try
            {
                var pitanje = await Context.Pitanja.FindAsync(id);

                if (pitanje != null)
                {
                    Context.Pitanja.Remove(pitanje);
                    int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                    // return Ok($"Pitanje ID = {pitanje.ID} | Question = {pitanje.Question} je uspešno izbrisano!"); // DB ažurira i model pa sada znamo ID
                    return Ok(pitanje); // DB ažurira i model pa sada znamo ID
                }
                else
                {
                    return BadRequest("Pitanje nije pronađeno!");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

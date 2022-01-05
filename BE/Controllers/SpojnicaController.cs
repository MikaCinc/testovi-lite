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
    public class SpojnicaController : ControllerBase
    {

        public TestoviContext Context { get; set; }

        public SpojnicaController(TestoviContext context)
        {
            Context = context;
        }

        /* [Route("PreuzmiPitanjea/{id}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiPitanjea(int id)
        {
            return StatusCode(404, "Pitanje not found...");
        } */

        [Route("PreuzmiSpojnice")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiSpojnice()
        {
            var spojnice = await Context.Spojnice
            .OrderByDescending(x => x.DateCreated)
            .Include(x => x.Tagovi)
            .ThenInclude(x => x.Tag)
            .Include(x => x.Pitanja)
            .ThenInclude(x => x.Pitanje)
            .Select(p => new
            {
                p.ID,
                p.Title,
                p.Archived,
                p.Highlighted,
                p.Priority,
                p.NumberOfGames,
                p.DateCreated,
                Tagovi = p.Tagovi.Select(t => new
                {
                    t.Tag.ID,
                    t.Tag.Title
                }),
                Pitanja = p.Pitanja.Select(t => new
                {
                    t.Pitanje.ID,
                    t.Pitanje.Question,
                    t.Pitanje.Answer,
                    t.Pitanje.isArchived,
                    t.Pitanje.Highlighted,
                    t.Pitanje.DateCreated,
                })
            })
            .ToListAsync();
            return Ok(spojnice);
        }

        [Route("PreuzmiSpojnicu/{index}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiPitanje(int index)
        {
            // Lazy loading
            // Eager loading - cela baza uključena
            // Explicit loading - samo podaci koje mi želimo

            var spojnice = Context.Spojnice;

            var spojnica = await spojnice.Where(p => p.ID == index).ToListAsync();

            return Ok(spojnica[0]);
        }

        [Route("DodajSpojnicu")]
        [HttpPost]
        public async Task<ActionResult> DodajSpojnicu([FromBody] Spojnica spojnica)
        {
            if (string.IsNullOrWhiteSpace(spojnica.Title) || spojnica.Title.Length > 100)
            {
                return BadRequest("Ne valja Title...");
            }

            try
            {
                spojnica.DateCreated = DateTime.Now;
                spojnica.Archived = false;
                spojnica.Highlighted = true;
                spojnica.Priority = 1;
                spojnica.NumberOfGames = 0;
                Context.Spojnice.Add(spojnica); // Ne upisuje odmah u DB
                int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                                                                    // return Ok($"Sve je u redu! ID novog pitanja je: {spojnica.ID}"); // DB ažurira i model pa sada znamo ID
                return Ok(spojnica); // DB ažurira i model pa sada znamo ID
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("PromeniSpojnicu")]
        [HttpPut]
        public async Task<ActionResult> PromeniSpojnicu([FromBody] Spojnica spojnica)
        {
            if (spojnica.ID < 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            if (string.IsNullOrWhiteSpace(spojnica.Title) || spojnica.Title.Length > 50)
            {
                return BadRequest("Ne valja title...");
            }

            try
            {
                var p = Context.Spojnice.Where(s => s.ID == spojnica.ID).FirstOrDefault(); // var menja bilo koji tip

                if (p != null)
                {
                    p.Title = spojnica.Title;
                    p.Archived = spojnica.Archived;
                    p.Highlighted = spojnica.Highlighted;
                    p.NumberOfGames = spojnica.NumberOfGames;
                    p.Priority = spojnica.Priority;

                    int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                    return Ok(spojnica); // DB ažurira i model pa sada znamo ID
                }
                else
                {
                    return BadRequest("spojnica nije pronađena!");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            // return StatusCode(404, "pitanje not found...");
        }

        [Route("DodajTag/{spojnicaId}/{tagId}")]
        [HttpPut]
        public async Task<ActionResult> DodajTag(int tagId, int spojnicaId)
        {
            if (tagId < 0 || spojnicaId < 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            try
            {
                var foundTag = Context.Tagovi.Where(t => t.ID == tagId).FirstOrDefault(); // var menja bilo koji tip
                var foundSpojnica = Context.Spojnice.Where(s => s.ID == spojnicaId).FirstOrDefault(); // var menja bilo koji tip

                if (foundTag != null && foundSpojnica != null)
                {
                    bool vecPostoji = Context.SpojniceTagovi.Where(st => st.Spojnica.ID == spojnicaId && st.Tag.ID == tagId).ToList().Count > 0;
                    if (!vecPostoji)
                    {
                        SpojniceTagovi s = new SpojniceTagovi
                        {
                            Spojnica = foundSpojnica,
                            Tag = foundTag
                        };
                        Context.SpojniceTagovi.Add(s);
                        int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                        return Ok(foundSpojnica); // DB ažurira i model pa sada znamo ID
                    }
                    else
                    {
                        return BadRequest("Tag već postoji u ovoj spojnici!");
                    }
                }
                else
                {
                    return BadRequest("Spojnica ili tag nije pronađen/a!");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzbrisiTag/{spojnicaId}/{tagId}")]
        [HttpPut]
        public async Task<ActionResult> IzbrisiTag(int tagId, int spojnicaId)
        {
            if (tagId < 0 || spojnicaId < 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            try
            {
                var foundTag = Context.Tagovi.Where(t => t.ID == tagId).FirstOrDefault(); // var menja bilo koji tip
                var foundSpojnica = Context.Spojnice.Where(s => s.ID == spojnicaId).FirstOrDefault(); // var menja bilo koji tip

                if (foundTag != null && foundSpojnica != null)
                {
                    var veza = Context.SpojniceTagovi.Where(st => st.Spojnica.ID == spojnicaId && st.Tag.ID == tagId).ToList();
                    bool vecPostoji = veza.Count > 0;
                    if (vecPostoji)
                    {
                        Context.SpojniceTagovi.Remove(veza[0]);
                        int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                        return Ok(foundSpojnica); // DB ažurira i model pa sada znamo ID
                    }
                    else
                    {
                        return BadRequest("Tag ne postoji u ovoj spojnici!");
                    }
                }
                else
                {
                    return BadRequest("Spojnica ili tag nije pronađen/a!");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajPitanje/{spojnicaId}")]
        [HttpPut]
        public async Task<ActionResult> DodajPitanje([FromBody] Pitanje pitanje, int spojnicaId)
        {
            if (pitanje.ID < 0 || spojnicaId < 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            try
            {
                var foundPitanje = Context.Pitanja.Where(t => t.ID == pitanje.ID).FirstOrDefault(); // var menja bilo koji tip
                var foundSpojnica = Context.Spojnice.Where(s => s.ID == spojnicaId).FirstOrDefault(); // var menja bilo koji tip

                if (foundSpojnica != null)
                {
                    if (foundPitanje != null)
                    {
                        bool vecPostoji = Context.SpojnicePitanja.Where(st => st.Spojnica.ID == spojnicaId && st.Pitanje.ID == pitanje.ID).ToList().Count > 0;
                        if (!vecPostoji)
                        {
                            SpojnicePitanja s = new SpojnicePitanja
                            {
                                Spojnica = foundSpojnica,
                                Pitanje = foundPitanje
                            };
                            Context.SpojnicePitanja.Add(s);
                            int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                            return Ok(foundSpojnica); // DB ažurira i model pa sada znamo ID
                        }
                        else
                        {
                            return BadRequest("Pitanje već postoji u ovoj spojnici!");
                        }
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(pitanje.Question) || pitanje.Question.Length > 100)
                        {
                            return BadRequest("Ne valja Pitanje...");
                        }
                        if (string.IsNullOrWhiteSpace(pitanje.Answer) || pitanje.Answer.Length > 100)
                        {
                            return BadRequest("Ne valja Odgovor...");
                        }

                        pitanje.DateCreated = DateTime.Now;
                        Context.Pitanja.Add(pitanje); // Pitanje je novo
                        SpojnicePitanja s = new SpojnicePitanja
                        {
                            Spojnica = foundSpojnica,
                            Pitanje = pitanje
                        };
                        Context.SpojnicePitanja.Add(s);
                        int successCode = await Context.SaveChangesAsync();
                        return Ok(foundSpojnica);
                    }
                }
                else
                {
                    return BadRequest("Spojnica nije pronađena!");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzbrisiPitanje/{spojnicaId}/{pitanjeId}")]
        [HttpPut]
        public async Task<ActionResult> IzbrisiPitanje(int pitanjeId, int spojnicaId)
        {
            if (pitanjeId < 0 || spojnicaId < 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            try
            {
                var foundPitanje = Context.Pitanja.Where(t => t.ID == pitanjeId).FirstOrDefault(); // var menja bilo koji tip
                var foundSpojnica = Context.Spojnice.Where(s => s.ID == spojnicaId).FirstOrDefault(); // var menja bilo koji tip

                if (foundPitanje != null && foundSpojnica != null)
                {
                    var veza = Context.SpojnicePitanja.Where(st => st.Spojnica.ID == spojnicaId && st.Pitanje.ID == pitanjeId).ToList();
                    bool vecPostoji = veza.Count > 0;
                    if (vecPostoji)
                    {
                        Context.SpojnicePitanja.Remove(veza[0]);
                        int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                        return Ok(foundSpojnica); // DB ažurira i model pa sada znamo ID
                    }
                    else
                    {
                        return BadRequest("Pitanje ne postoji u ovoj spojnici!");
                    }
                }
                else
                {
                    return BadRequest("Spojnica ili pitanje nije pronađen/o!");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzbrisiSpojnicu/{id}")]
        [HttpDelete]
        public async Task<ActionResult> IzbrisiSpojnicu(int id)
        {
            if (id < 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            try
            {
                var spojnica = await Context.Spojnice.FindAsync(id);

                if (spojnica != null)
                {
                    Context.SpojniceTagovi.RemoveRange(Context.SpojniceTagovi.Where(s => s.Spojnica.ID == id));
                    Context.SpojnicePitanja.RemoveRange(Context.SpojnicePitanja.Where(s => s.Spojnica.ID == id));
                    Context.Spojnice.Remove(spojnica);
                    int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                                                                        // return Ok($"Pitanje ID = {pitanje.ID} | Question = {pitanje.Question} je uspešno izbrisano!"); // DB ažurira i model pa sada znamo ID
                    return Ok(spojnica); // DB ažurira i model pa sada znamo ID
                }
                else
                {
                    return BadRequest("spojnica nije pronađeno!");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

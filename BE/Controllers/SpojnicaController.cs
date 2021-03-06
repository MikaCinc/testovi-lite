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
            // .Where(s => s.Highlighted == true)
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

        [Route("PreuzmiSpojnice/{setId}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiSpojniceIzSeta(int setId)
        {
            /* var spojniceIDsIzSeta = Context.SetSpojnice
                .Where(x => x.Set.ID == setId)
                .Select(x => x.Spojnica.ID)
                .ToList(); */

            var spojnice = await Context.SetSpojnice
            .Where(x => x.Set.ID == setId)
            .Where(x => x.Spojnica.Highlighted == true)
            .Include(x => x.Spojnica.Tagovi)
            .ThenInclude(x => x.Tag)
            .Include(x => x.Spojnica.Pitanja)
            .ThenInclude(x => x.Pitanje)
            .Select(x => x.Spojnica)
            .OrderByDescending(x => x.DateCreated)
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
        public async Task<ActionResult> PreuzmiSpojnicu(int index)
        {
            // Lazy loading
            // Eager loading - cela baza uklju??ena
            // Explicit loading - samo podaci koje mi ??elimo

            var spojnice = Context.Spojnice;

            var spojnica = await spojnice.Where(p => p.ID == index).ToListAsync();

            return Ok(spojnica[0]);
        }

        [Route("DodajSpojnicu/{setId}")]
        [HttpPost]
        public async Task<ActionResult> DodajSpojnicu([FromBody] Spojnica spojnica, int setId)
        {
            if (string.IsNullOrWhiteSpace(spojnica.Title) || spojnica.Title.Length > 100)
            {
                return BadRequest("Ne valja Title...");
            }

            if (setId < 0)
            {
                return BadRequest("Ne valja SetId...");
            }

            try
            {
                var set = await Context.Setovi.FindAsync(setId);
                if (set == null)
                {
                    return BadRequest("Set not found...");
                }
                spojnica.DateCreated = DateTime.Now;
                spojnica.Archived = false;
                spojnica.Highlighted = true;
                spojnica.Priority = spojnica.Priority;
                spojnica.NumberOfGames = 0;
                Context.Spojnice.Add(spojnica); // Ne upisuje odmah u DB
                Context.SetSpojnice.Add(new SetSpojnice { Set = set, Spojnica = spojnica });
                int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                                                                    // return Ok($"Sve je u redu! ID novog pitanja je: {spojnica.ID}"); // DB a??urira i model pa sada znamo ID
                return Ok(spojnica); // DB a??urira i model pa sada znamo ID
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
                return BadRequest("Pogre??an ID!");
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
                    return Ok(spojnica); // DB a??urira i model pa sada znamo ID
                }
                else
                {
                    return BadRequest("spojnica nije prona??ena!");
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
                return BadRequest("Pogre??an ID!");
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
                        return Ok(foundSpojnica); // DB a??urira i model pa sada znamo ID
                    }
                    else
                    {
                        return BadRequest("Tag ve?? postoji u ovoj spojnici!");
                    }
                }
                else
                {
                    return BadRequest("Spojnica ili tag nije prona??en/a!");
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
                return BadRequest("Pogre??an ID!");
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
                        return Ok(foundSpojnica); // DB a??urira i model pa sada znamo ID
                    }
                    else
                    {
                        return BadRequest("Tag ne postoji u ovoj spojnici!");
                    }
                }
                else
                {
                    return BadRequest("Spojnica ili tag nije prona??en/a!");
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
                return BadRequest("Pogre??an ID!");
            }

            try
            {
                var foundPitanje = Context.Pitanja.Where(t => t.ID == pitanje.ID).FirstOrDefault(); // var menja bilo koji tip
                var foundSpojnica = Context.Spojnice.Where(s => s.ID == spojnicaId).FirstOrDefault(); // var menja bilo koji tip
                var foundSet = Context.SetSpojnice.Where(s => s.Spojnica.ID == spojnicaId).Select(s => s.Set).FirstOrDefault(); // var menja bilo koji tip

                if (foundSet == null)
                {
                    return BadRequest("Set nije prona??en!");
                }

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

                            /* SetPitanja sp = new SetPitanja
                            {
                                Set = foundSet,
                                Pitanje = foundPitanje
                            };
                            Context.SetPitanja.Add(sp); */
                            int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                            return Ok(foundSpojnica); // DB a??urira i model pa sada znamo ID
                        }
                        else
                        {
                            return BadRequest("Pitanje ve?? postoji u ovoj spojnici!");
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

                        SetPitanja sp = new SetPitanja
                        {
                            Set = foundSet,
                            Pitanje = pitanje
                        };
                        Context.SetPitanja.Add(sp);

                        int successCode = await Context.SaveChangesAsync();
                        return Ok(foundSpojnica);
                    }
                }
                else
                {
                    return BadRequest("Spojnica nije prona??ena!");
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
                return BadRequest("Pogre??an ID!");
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
                        return Ok(foundSpojnica); // DB a??urira i model pa sada znamo ID
                    }
                    else
                    {
                        return BadRequest("Pitanje ne postoji u ovoj spojnici!");
                    }
                }
                else
                {
                    return BadRequest("Spojnica ili pitanje nije prona??en/o!");
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
                return BadRequest("Pogre??an ID!");
            }

            try
            {
                var spojnica = await Context.Spojnice.FindAsync(id);

                if (spojnica != null)
                {
                    Context.SpojniceTagovi.RemoveRange(Context.SpojniceTagovi.Where(s => s.Spojnica.ID == id));
                    Context.SpojnicePitanja.RemoveRange(Context.SpojnicePitanja.Where(s => s.Spojnica.ID == id));
                    Context.SetSpojnice.RemoveRange(Context.SetSpojnice.Where(s => s.Spojnica.ID == id));
                    Context.Spojnice.Remove(spojnica);
                    int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                                                                        // return Ok($"Pitanje ID = {pitanje.ID} | Question = {pitanje.Question} je uspe??no izbrisano!"); // DB a??urira i model pa sada znamo ID
                    return Ok(spojnica); // DB a??urira i model pa sada znamo ID
                }
                else
                {
                    return BadRequest("spojnica nije prona??eno!");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

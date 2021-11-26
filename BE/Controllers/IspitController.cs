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
    public class IspitController : ControllerBase
    {

        public TestoviContext Context { get; set; }

        public IspitController(TestoviContext context)
        {
            Context = context;
        }

        /* [Route("PreuzmiStudenta/{id}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiStudenta(int id)
        {
            return StatusCode(404, "Student not found...");
        } */

        [Route("IspitniRokovi")]
        [HttpGet]
        public async Task<ActionResult> IspitniRokovi()
        {
            return Ok(await Context.Rokovi.Select(p => new { p.ID, p.Naziv }).ToListAsync());
        }

        [Route("DodajPolozeniIspit/{index}/{IDPredmeta}/{IDRoka}/{ocena}")]
        [HttpPost]
        public async Task<ActionResult> DodajPolozeniIspit(int index, int IDPredmeta, int IDRoka, int ocena)
        {
            if (index < 10000 || index > 20000)
            {
                return BadRequest("Pogrešan Index!");
            }
            if (ocena < 5 || ocena > 10)
            {
                return BadRequest("Pogrešna ocena!");
            }
            try
            {
                var student = await Context.Studenti.Where(p => p.Index == index).FirstOrDefaultAsync();
                if (student == null)
                {
                    return BadRequest("Student ne postoji!");
                }

                var predmet = await Context.Predmeti.Where(p => p.ID == IDPredmeta).FirstOrDefaultAsync();
                if (predmet == null)
                {
                    return BadRequest("Predmet ne postoji!");
                }

                var rok = await Context.Rokovi.FindAsync(IDRoka);
                if (rok == null)
                {
                    return BadRequest("Rok ne postoji!");
                }

                Spoj s = new Spoj
                {
                    Student = student,
                    Predmet = predmet,
                    IspitniRok = rok,
                    Ocena = ocena
                };

                Context.StudentiPredmeti.Add(s); // Ne upisuje odmah u DB
                int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB


                var podaciOStudentu = await Context.StudentiPredmeti
                    .Include(p => p.Student)
                    .Include(p => p.Predmet)
                    .Include(p => p.IspitniRok)
                    .Where(p => p.Student.Index == index)
                    .Select(p => new
                    {
                        p.Student.Index,
                        p.Student.Ime,
                        p.Student.Prezime,
                        Predmet = p.Predmet.Naziv,
                        IspitniRok = p.IspitniRok.Naziv,
                        Ocena = p.Ocena
                    }).ToListAsync();

                return Ok(podaciOStudentu);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

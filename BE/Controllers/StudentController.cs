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
    public class StudentController : ControllerBase
    {

        public TestoviContext Context { get; set; }

        public StudentController(TestoviContext context)
        {
            Context = context;
        }

        /* [Route("PreuzmiStudenta/{id}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiStudenta(int id)
        {
            return StatusCode(404, "Student not found...");
        } */

        [Route("PreuzmiStudente")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiStudente()
        {
            return Ok(Context.Studenti);
        }

        [Route("PreuzmiStudenteFull")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiStudenteFull()
        {
            // Lazy loading
            // Eager loading - cela baza uključena
            // Explicit loading - samo podaci koje mi želimo

            var studenti = Context.Studenti
            .Include(p => p.StudentPredmet) // inlude - koju vezu da uključimo // podaci o svim studentima sa pridruženom klasom StudentPredmet
            .ThenInclude(p => p.Predmet) // Include je na prvom nivou, ThenInclude radi sa tipom podataka iz prethodnog Include // Ovde studentima dodajemo Predmete preko tabele Spoj
            .Include(p => p.StudentPredmet) // Opet radimo na prvom nivou // Tabela Spoj
            .ThenInclude(p => p.IspitniRok); // Pa svemu tome dodajemo preko tabele Spoj podatke o Ispitnim rokovima 

            return Ok(studenti);
        }

        [Route("PreuzmiStudenteKrozQuery")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiStudenteKrozQuery([FromQuery] int[] rokIDs)
        {
            // Lazy loading
            // Eager loading - cela baza uključena
            // Explicit loading - samo podaci koje mi želimo

            var studenti = Context.Studenti
            .Include(p => p.StudentPredmet) // inlude - koju vezu da uključimo // podaci o svim studentima sa pridruženom klasom StudentPredmet
            .ThenInclude(p => p.Predmet) // Include je na prvom nivou, ThenInclude radi sa tipom podataka iz prethodnog Include // Ovde studentima dodajemo Predmete preko tabele Spoj
            .Include(p => p.StudentPredmet) // Opet radimo na prvom nivou // Tabela Spoj
            .ThenInclude(p => p.IspitniRok); // Pa svemu tome dodajemo preko tabele Spoj podatke o Ispitnim rokovima 

            var studenti2 = await studenti.ToListAsync();

            return Ok(studenti2.Where(s => s.StudentPredmet.Any(sp => rokIDs.Contains(sp.IspitniRok.ID))));
        }

        [Route("PreuzmiStudentaFull/{index}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiStudentaFull(int index)
        {
            // Lazy loading
            // Eager loading - cela baza uključena
            // Explicit loading - samo podaci koje mi želimo

            var studenti = Context.Studenti
            .Include(p => p.StudentPredmet) // inlude - koju vezu da uključimo // podaci o svim studentima sa pridruženom klasom StudentPredmet
            .ThenInclude(p => p.Predmet) // Include je na prvom nivou, ThenInclude radi sa tipom podataka iz prethodnog Include // Ovde studentima dodajemo Predmete preko tabele Spoj
            .Include(p => p.StudentPredmet) // Opet radimo na prvom nivou // Tabela Spoj
            .ThenInclude(p => p.IspitniRok); // Pa svemu tome dodajemo preko tabele Spoj podatke o Ispitnim rokovima 

            var student = await studenti.Where(p => p.Index == index).ToListAsync();

            return Ok(student[0]);
        }

        [Route("PreuzmiStudentaMaloDrugacije/{index}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiStudentaMaloDrugacije(int index)
        {
            var studenti = Context.Studenti
            .Include(p => p.StudentPredmet) // inlude - koju vezu da uključimo // podaci o svim studentima sa pridruženom klasom StudentPredmet
            .ThenInclude(p => p.Predmet) // Include je na prvom nivou, ThenInclude radi sa tipom podataka iz prethodnog Include // Ovde studentima dodajemo Predmete preko tabele Spoj
            .Include(p => p.StudentPredmet) // Opet radimo na prvom nivou // Tabela Spoj
            .ThenInclude(p => p.IspitniRok); // Pa svemu tome dodajemo preko tabele Spoj podatke o Ispitnim rokovima 

            var student = await studenti.Where(p => p.Index == index).ToListAsync();

            return Ok(
                student.Select(p => new
                {
                    Ind = p.Index,
                    Ime = p.Ime,
                    Premdeti = p.StudentPredmet.Select(s => new
                    {
                        Predmet = s.Predmet.Naziv,
                        Ocena = s.Ocena,
                        IspitniRok = s.IspitniRok.Naziv,
                        GodinaPredmeta = s.Predmet.Godina
                    })
                }).ToList()
            );
        }

        [Route("DodajStudenta")]
        [HttpPost]
        public async Task<ActionResult> DodajStudenta([FromBody] Student student)
        {
            if (student.Index < 10000 || student.Index > 20000)
            {
                return BadRequest("Pogrešan Index!");
            }

            if (string.IsNullOrWhiteSpace(student.Ime) || student.Ime.Length > 50)
            {
                return BadRequest("Ne valja ime...");
            }

            if (string.IsNullOrWhiteSpace(student.Prezime) || student.Prezime.Length > 50)
            {
                return BadRequest("Ne valja prezime...");
            }

            try
            {
                Context.Studenti.Add(student); // Ne upisuje odmah u DB
                int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                return Ok($"Sve je u redu! ID novog studenta je: {student.ID}"); // DB ažurira i model pa sada znamo ID
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            // return StatusCode(404, "Student not found...");
        }

        [Route("PromenStudenta/{index}/{ime}/{prezime}")]
        [HttpPut]
        public async Task<ActionResult> PromenStudenta(int index, string ime, string prezime)
        {
            if (index < 10000 || index > 20000)
            {
                return BadRequest("Pogrešan Index!");
            }

            if (string.IsNullOrWhiteSpace(ime) || ime.Length > 50)
            {
                return BadRequest("Ne valja ime...");
            }

            if (string.IsNullOrWhiteSpace(prezime) || prezime.Length > 50)
            {
                return BadRequest("Ne valja prezime...");
            }

            try
            {
                var student = Context.Studenti.Where(s => s.Index == index).FirstOrDefault(); // var menja bilo koji tip

                if (student != null)
                {
                    student.Ime = ime;
                    student.Prezime = prezime;

                    int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                    return Ok($"Student ID = {student.ID} je uspešno izmenjen"); // DB ažurira i model pa sada znamo ID
                }
                else
                {
                    return BadRequest("Student nije pronađen!");
                }

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            // return StatusCode(404, "Student not found...");
        }

        [Route("PromeniStudentaFull")]
        [HttpPut]
        public async Task<ActionResult> PromeniStudentaFull([FromBody] Student student)
        {
            if (student.Index < 10000 || student.Index > 20000)
            {
                return BadRequest("Pogrešan Index!");
            }

            if (string.IsNullOrWhiteSpace(student.Ime) || student.Ime.Length > 50)
            {
                return BadRequest("Ne valja ime...");
            }

            if (string.IsNullOrWhiteSpace(student.Prezime) || student.Prezime.Length > 50)
            {
                return BadRequest("Ne valja prezime...");
            }

            try
            {
                /* var trenutniStudent = await Context.Studenti.FindAsync(student.ID);

                if (trenutniStudent != null)
                {
                    trenutniStudent.Index = student.Index;
                    trenutniStudent.Ime = student.Ime;
                    trenutniStudent.Prezime = student.Prezime;

                    int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                    return Ok($"Student ID = {student.ID} je uspešno izmenjen"); // DB ažurira i model pa sada znamo ID
                }
                else
                {
                    return BadRequest("Student nije pronađen!");
                } */

                // Alternativa
                Context.Studenti.Update(student); // U student objektu mora da postoji ID da bi on znao koga da ažurira u DB
                int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                return Ok($"Student ID = {student.ID} je uspešno izmenjen"); // DB ažurira i model pa sada znamo ID
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            // return StatusCode(404, "Student not found...");
        }

        [Route("IzbrisiStudenta/{id}")]
        [HttpDelete]
        public async Task<ActionResult> IzbrisiStudenta(int id)
        {
            if (id < 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            try
            {
                var student = await Context.Studenti.FindAsync(id);

                if (student != null)
                {
                    Context.Studenti.Remove(student);
                    int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                    return Ok($"Student ID = {student.ID} | Index = {student.Index} je uspešno izbrisan!"); // DB ažurira i model pa sada znamo ID
                }
                else
                {
                    return BadRequest("Student nije pronađen!");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}

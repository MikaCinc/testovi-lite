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
    public class SetController : ControllerBase
    {

        public TestoviContext Context { get; set; }

        public SetController(TestoviContext context)
        {
            Context = context;
        }

        [Route("PreuzmiSetove")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiSetove()
        {
            return Ok(Context.Setovi.ToList());
        }

        [Route("DodajSet")]
        [HttpPost]
        public async Task<ActionResult> DodajSet(string title)
        {
            if (string.IsNullOrWhiteSpace(title) || title.Length > 50)
            {
                return BadRequest("Ne valja title...");
            }

            try
            {
                Set set = new Set()
                {
                    Title = title
                };
                Context.Setovi.Add(set); // Ne upisuje odmah u DB
                int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                // return Ok($"Sve je u redu! ID novog seta je: {set.ID}"); // DB ažurira i model pa sada znamo ID
                return Ok(set); // DB ažurira i model pa sada znamo ID
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzbrisiSet/{id}")]
        [HttpDelete]
        public async Task<ActionResult> IzbrisiSet(int id)
        {
            if (id < 0)
            {
                return BadRequest("Pogrešan ID!");
            }

            try
            {
                var set = await Context.Setovi.FindAsync(id);

                if (set != null)
                {
                    Context.SetSpojnice.RemoveRange(Context.SetSpojnice.Where(s => s.Set.ID == id));
                    Context.SetPitanja.RemoveRange(Context.SetPitanja.Where(s => s.Set.ID == id));
                    Context.SetTagovi.RemoveRange(Context.SetTagovi.Where(s => s.Set.ID == id));
                    Context.Setovi.Remove(set);
                    int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                    // return Ok($"set ID = {set.ID} | Title = {set.Title} je uspešno izbrisan!"); // DB ažurira i model pa sada znamo ID
                    return Ok(set); // DB ažurira i model pa sada znamo ID
                }
                else
                {
                    return BadRequest("Set nije pronađen!");
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
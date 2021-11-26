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
    public class PredmetController : ControllerBase
    {

        public TestoviContext Context { get; set; }

        public PredmetController(TestoviContext context)
        {
            Context = context;
        }

        [Route("PreuzmiPredmete")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiPredmete()
        {
            return Ok(await Context.Predmeti.Select(x => new { x.ID, x.Naziv }).ToListAsync());
        }

        [Route("DodajPredmet")]
        [HttpPost]
        public async Task<ActionResult> DodajPredmet([FromBody] Predmet predmet)
        {
            if (predmet.Godina < 1 || predmet.Godina > 4)
            {
                return BadRequest("Pogrešna godina!");
            }

            if (string.IsNullOrWhiteSpace(predmet.Naziv) || predmet.Naziv.Length > 50)
            {
                return BadRequest("Ne valja Naziv...");
            }

            try
            {
                Context.Predmeti.Add(predmet); // Ne upisuje odmah u DB
                int successCode = await Context.SaveChangesAsync(); // Sada se upisuje u DB
                return Ok($"Sve je u redu! ID novog predmeta je: {predmet.ID}"); // DB ažurira i model pa sada znamo ID
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}

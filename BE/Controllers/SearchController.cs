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
    public class SearchController : ControllerBase
    {

        public TestoviContext Context { get; set; }

        public SearchController(TestoviContext context)
        {
            Context = context;
        }

        /* [Route("PreuzmiPitanjea/{id}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiPitanjea(int id)
        {
            return StatusCode(404, "Pitanje not found...");
        } */

        [Route("PreuzmiSpojnicePoTagu/{tagId}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiSpojnicePoTagu(int tagId)
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
            .Where(x => x.Tagovi.Any(t => t.ID == tagId))
            .ToListAsync();
            return Ok(spojnice);
        }

        [Route("PreuzmiSpojnicePoNazivu/{naziv}/{setId}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiSpojnicePoNazivu(string naziv, int setId)
        {
            var spojnice = await Context.SetSpojnice
            .Where(x => x.Set.ID == setId)
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
            .Where(x => x.Title.ToLower().Contains(naziv.ToLower()))
            .ToListAsync();
            return Ok(spojnice);
        }

        /* [Route("PreuzmiSpojnicu/{index}")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiSpojnicu(int index)
        {
            // Lazy loading
            // Eager loading - cela baza uključena
            // Explicit loading - samo podaci koje mi želimo

            var spojnice = Context.Spojnice;

            var spojnica = await spojnice.Where(p => p.ID == index).ToListAsync();

            return Ok(spojnica[0]);
        } */
    }
}
using Microsoft.AspNetCore.Mvc;
using isgasoir.Services.ServiceApi;
using System.Linq;

namespace isgasoir.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly LLMApiImpl _llm;

        public ActivitiesController(IUnitOfWork uow, LLMApiImpl llm)
        {
            _uow = uow;
            _llm = llm;
        }

        [HttpPost("generate/{chapitreId}")]
        public IActionResult Generate(long chapitreId)
        {
            var chap = _uow.chapitreRepository.findById(chapitreId);
            if (chap == null) return NotFound();

            var generated = _llm.GenerateActivityAsync(chap.Title, chap.Content).GetAwaiter().GetResult();

            var activity = new Activity
            {
                Title = $"Activité auto pour {chap.Title}",
                Instructions = generated,
                ChapitreId = chap.Id
            };

            _uow.activityRepository.add(activity);
            _uow.complete();

            return CreatedAtAction(nameof(GetById), new { id = activity.Id }, activity);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(long id)
        {
            var a = _uow.activityRepository.findById(id);
            if (a == null) return NotFound();
            return Ok(a);
        }

        [HttpGet("by-chapitre/{chapitreId}")]
        public IActionResult ListByChapitre(long chapitreId)
        {
            var list = _uow.activityRepository.Query.Where(a => a.ChapitreId == chapitreId).ToList();
            return Ok(list);
        }
    }
}

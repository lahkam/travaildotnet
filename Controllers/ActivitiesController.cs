using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace isgasoir.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public ActivitiesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _uow.activityRepository.findAll();
            return Ok(list);
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

        [HttpPost]
        public IActionResult Create([FromBody] Activity activity)
        {
            // Vérifier que le chapitre existe
            var chap = _uow.chapitreRepository.findById(activity.ChapitreId);
            if (chap == null) return BadRequest($"Chapitre {activity.ChapitreId} introuvable");
            _uow.activityRepository.add(activity);
            _uow.complete();
            return CreatedAtAction(nameof(GetById), new { id = activity.Id }, activity);
        }

        [HttpPost("generate/{chapitreId}")]
        public IActionResult Generate(long chapitreId)
        {
            var chap = _uow.chapitreRepository.findById(chapitreId);
            if (chap == null) return NotFound();

            // génération de secours (sans LLM)
            var generated = $"Instructions générées automatiquement pour le chapitre '{chap.Title}' :\n" +
                            (string.IsNullOrWhiteSpace(chap.Content) ? "(pas de contenu)" : (chap.Content.Length > 200 ? chap.Content.Substring(0, 200) + "..." : chap.Content));

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
    }
}

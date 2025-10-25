using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace isgasoir.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChapitresController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public ChapitresController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _uow.chapitreRepository.findAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var item = _uow.chapitreRepository.findById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Chapitre chapitre)
        {
            _uow.chapitreRepository.add(chapitre);
            _uow.complete();
            return CreatedAtAction(nameof(Get), new { id = chapitre.Id }, chapitre);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Chapitre chapitre)
        {
            var existing = _uow.chapitreRepository.findById(id);
            if (existing == null) return NotFound();
            chapitre.Id = id;
            _uow.chapitreRepository.update(chapitre);
            _uow.complete();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var existing = _uow.chapitreRepository.findById(id);
            if (existing == null) return NotFound();
            _uow.chapitreRepository.remove(existing);
            _uow.complete();
            return NoContent();
        }

        [HttpGet("{id}/activities")]
        public IActionResult GetActivities(long id)
        {
            var acts = _uow.activityRepository.Query.Where(a => a.ChapitreId == id).ToList();
            return Ok(acts);
        }
    }
}

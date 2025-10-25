using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace isgasoir.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FiliereController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public FiliereController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _uow.filiereRepository.findAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var f = _uow.filiereRepository.Query.Include(f => f.Semestres).ThenInclude(s => s.Modules).ThenInclude(m => m.Chapitres).ThenInclude(c => c.Activities).FirstOrDefault(x => x.Id == id);
            if (f == null) return NotFound();
            return Ok(f);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Filiere filiere)
        {
            _uow.filiereRepository.add(filiere);
            _uow.complete();
            return CreatedAtAction(nameof(Get), new { id = filiere.Id }, filiere);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Filiere filiere)
        {
            var existing = _uow.filiereRepository.findById(id);
            if (existing == null) return NotFound();
            filiere.Id = id;
            _uow.filiereRepository.update(filiere);
            _uow.complete();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var existing = _uow.filiereRepository.findById(id);
            if (existing == null) return NotFound();
            _uow.filiereRepository.remove(existing);
            _uow.complete();
            return NoContent();
        }
    }
}

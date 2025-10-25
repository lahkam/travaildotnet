using Microsoft.AspNetCore.Mvc;

namespace isgasoir.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentApi : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public StudentApi(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _uow.studantRepository.findAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var item = _uow.studantRepository.findById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Studant studant)
        {
            _uow.studantRepository.add(studant);
            _uow.complete();
            return CreatedAtAction(nameof(Get), new { id = studant.Id }, studant);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Studant studant)
        {
            var existing = _uow.studantRepository.findById(id);
            if (existing == null) return NotFound();
            studant.Id = id;
            _uow.studantRepository.update(studant);
            _uow.complete();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var existing = _uow.studantRepository.findById(id);
            if (existing == null) return NotFound();
            _uow.studantRepository.remove(existing);
            _uow.complete();
            return NoContent();
        }
    }
}

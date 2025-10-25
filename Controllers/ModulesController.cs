using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace isgasoir.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModulesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public ModulesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var list = _uow.moduleRepository.findAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var item = _uow.moduleRepository.findById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Module module)
        {
            _uow.moduleRepository.add(module);
            _uow.complete();
            return CreatedAtAction(nameof(Get), new { id = module.Id }, module);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Module module)
        {
            var existing = _uow.moduleRepository.findById(id);
            if (existing == null) return NotFound();
            module.Id = id;
            _uow.moduleRepository.update(module);
            _uow.complete();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var existing = _uow.moduleRepository.findById(id);
            if (existing == null) return NotFound();
            _uow.moduleRepository.remove(existing);
            _uow.complete();
            return NoContent();
        }

        [HttpGet("semestre/{semId}")]
        public IActionResult GetBySemestre(long semId)
        {
            var list = _uow.moduleRepository.findByCretiria(m => ((Module)m).Sem != null && ((Module)m).Sem.Id == semId).ToList();
            return Ok(list);
        }
    }
}

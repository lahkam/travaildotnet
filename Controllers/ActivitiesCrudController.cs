using Microsoft.AspNetCore.Mvc;

namespace isgasoir.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesCrudController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public ActivitiesCrudController(IUnitOfWork uow)
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
        public IActionResult Get(long id)
        {
            var item = _uow.activityRepository.findById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Activity activity)
        {
            _uow.activityRepository.add(activity);
            _uow.complete();
            return CreatedAtAction(nameof(Get), new { id = activity.Id }, activity);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Activity activity)
        {
            var existing = _uow.activityRepository.findById(id);
            if (existing == null) return NotFound();
            activity.Id = id;
            _uow.activityRepository.update(activity);
            _uow.complete();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var existing = _uow.activityRepository.findById(id);
            if (existing == null) return NotFound();
            _uow.activityRepository.remove(existing);
            _uow.complete();
            return NoContent();
        }
    }
}

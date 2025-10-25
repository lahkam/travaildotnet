using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace isgasoir.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemestreController : ControllerBase
    {

        // private readonly ApplicationContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Microsoft.Extensions.Logging.ILogger<SemestreController> _logger;

        public SemestreController(IUnitOfWork unitOfWork, Microsoft.Extensions.Logging.ILogger<SemestreController> logger = null)
        {
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }

        // GET: api/semestres
        [HttpGet]
        public List<Semestre> Getsemestres()
        {
            if (_unitOfWork.semestreRepository == null)
            {
                return null;// NotFound();
            }
            return _unitOfWork.semestreRepository.Query.Include(s => s.Modules).ToList();
           // return _unitOfWork.semestre_repository.findAll();
        }

        // GET: api/semestres/5
        [HttpGet("{id}")]
        public ActionResult<Semestre> Getsemestre(long id)
        {
            if (_unitOfWork.semestreRepository == null)
            {
                return NotFound();
            }
         /*   var @semestre = _unitOfWork.semestre_repository.findById(id);
            List<Module> ms = _unitOfWork.moduleRepository.findByCretiria(s => s.Sem.Id == id).ToList();
            @semestre.Modules = ms;*/

            var @semestre=  _unitOfWork.semestreRepository.Query.Include(s=> s.Modules).Where(w=>w.Id== id).First();   

            if (@semestre == null)
            {
                return NotFound();
            }

            return @semestre;
        }


        // GET: api/semestre/filiere/5
        [HttpGet("filiere/{filiereId}")]
        public IActionResult GetByFiliere(long filiereId)
        {
            if (_unitOfWork.semestreRepository == null) return NotFound();
            var list = _unitOfWork.semestreRepository.Query
                        .Where(s => s.FiliereId == filiereId)
                        .Include(s => s.Modules)
                        .ToList();
            return Ok(list);
        }

        // POST: api/semestres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Semestre> Postsemestre(Semestre @semestre)
        {
            if (_unitOfWork.semestreRepository == null)
            {
                return Problem("Entity set 'ApplicationContext.semestres'  is null.");
            }
            _unitOfWork.semestreRepository.add(@semestre);
            _unitOfWork.complete();

            return CreatedAtAction("Getsemestre", new { id = @semestre.Id }, @semestre);
        }

        // DELETE: api/semestres/5
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            return RemoveSemestre(id);
        }

        // Also accept plural route to avoid 405 when callers use /api/semestres/{id}
        [HttpDelete]
        [Route("/api/semestres/{id}")]
        public IActionResult DeletePlural(long id)
        {
            return RemoveSemestre(id);
        }

        private IActionResult RemoveSemestre(long id)
        {
            if (_unitOfWork.semestreRepository == null) return NotFound();
            var existing = _unitOfWork.semestreRepository.findById(id);
            if (existing == null) return NotFound();

            // Remove dependent entities to avoid FK constraint violations
            try
            {
                // Remove modules for this semestre (include Sem to evaluate FK)
                var modules = _unitOfWork.moduleRepository.Query
                    .ToList()
                    .Where(m => m.Sem != null && m.Sem.Id == id)
                    .ToList();

                foreach (var mod in modules)
                {
                    var chapitres = _unitOfWork.chapitreRepository.Query
                        .ToList()
                        .Where(c => c.Module != null && c.Module.Id == mod.Id)
                        .ToList();

                    foreach (var ch in chapitres)
                    {
                        var acts = _unitOfWork.activityRepository.Query
                            .Where(a => a.ChapitreId == ch.Id)
                            .ToList();

                        foreach (var a in acts)
                        {
                            _unitOfWork.activityRepository.remove(a);
                        }

                        _unitOfWork.chapitreRepository.remove(ch);
                    }

                    _unitOfWork.moduleRepository.remove(mod);
                }

                // finally remove semestre
                _unitOfWork.semestreRepository.remove(existing);
                _unitOfWork.complete();
                return NoContent();
            }
            catch (System.Exception ex)
            {
                // log if logger available
                try { _logger?.LogError(ex, "Error removing semestre {Id}", id); } catch { }
                return StatusCode(500, "Erreur lors de la suppression du semestre");
            }
        }

        /* private bool semestreExists(long id)
         {
             return (_context.semestres?.Any(e => e.Id == id)).GetValueOrDefault();
         }*/
    }
}

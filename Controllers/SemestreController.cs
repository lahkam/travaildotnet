using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace isgasoir.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SemestreController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Microsoft.Extensions.Logging.ILogger<SemestreController>? _logger;

        public SemestreController(IUnitOfWork unitOfWork, Microsoft.Extensions.Logging.ILogger<SemestreController>? logger = null)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // GET: api/semestre
        [HttpGet]
        public List<Semestre> Getsemestres()
        {
            if (_unitOfWork.semestreRepository == null)
            {
                return new List<Semestre>();
            }
            return _unitOfWork.semestreRepository.Query.Include(s => s.Modules).ToList();
        }

        // GET: api/semestre/5
        [HttpGet("{id}")]
        public ActionResult<Semestre> Getsemestre(long id)
        {
            if (_unitOfWork.semestreRepository == null) return NotFound();
            var semestre = _unitOfWork.semestreRepository.Query.Include(s => s.Modules).FirstOrDefault(w => w.Id == id);
            if (semestre == null) return NotFound();
            return semestre;
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

        // POST: api/semestre
        [HttpPost]
        public ActionResult<Semestre> Postsemestre(Semestre semestre)
        {
            if (_unitOfWork.semestreRepository == null)
            {
                return Problem("Entity set 'ApplicationContext.semestres' is null.");
            }
            _unitOfWork.semestreRepository.add(semestre);
            _unitOfWork.complete();
            return CreatedAtAction(nameof(Getsemestre), new { id = semestre.Id }, semestre);
        }

        // DELETE: api/semestre/5
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

            try
            {
                var uowConcrete = _unitOfWork as UnitOfWork;
                if (uowConcrete == null) return StatusCode(500, "UnitOfWork concrete type unavailable");

                var ctx = uowConcrete.context;

                var semestre = ctx.Semestrees
                    .Include(s => s.Modules!)
                        .ThenInclude(m => m.Chapitres!)
                            .ThenInclude(c => c.Activities!)
                    .FirstOrDefault(s => s.Id == id);

                if (semestre == null) return NotFound();

                int removedActivities = 0, removedChapitres = 0, removedModules = 0;

                if (semestre.Modules != null)
                {
                    foreach (var mod in semestre.Modules.ToList())
                    {
                        if (mod.Chapitres != null)
                        {
                            foreach (var ch in mod.Chapitres.ToList())
                            {
                                if (ch.Activities != null)
                                {
                                    foreach (var a in ch.Activities.ToList())
                                    {
                                        ctx.Activities.Remove(a);
                                        removedActivities++;
                                    }
                                }
                                ctx.Chapitres.Remove(ch);
                                removedChapitres++;
                            }
                        }
                        ctx.Modules.Remove(mod);
                        removedModules++;
                    }
                }

                ctx.Semestrees.Remove(semestre);
                ctx.SaveChanges();

                return Ok(new
                {
                    message = "Semestre supprimé",
                    removedModules,
                    removedChapitres,
                    removedActivities
                });
            }
            catch (System.Exception ex)
            {
                try { _logger?.LogError(ex, "Error removing semestre {Id}", id); } catch { }
                return StatusCode(500, "Erreur lors de la suppression du semestre");
            }
        }
    }
}

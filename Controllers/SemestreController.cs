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

        public SemestreController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
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
           // return _unitOfWork.semestreRepository.findAll();
        }

        // GET: api/semestres/5
        [HttpGet("{id}")]
        public ActionResult<Semestre> Getsemestre(long id)
        {
            if (_unitOfWork.semestreRepository == null)
            {
                return NotFound();
            }
         /*   var @semestre = _unitOfWork.semestreRepository.findById(id);
            List<Module> ms = _unitOfWork.moduleRepository.findByCretiria(s => s.Sem.Id == id).ToList();
            @semestre.Modules = ms;*/

            var @semestre=  _unitOfWork.semestreRepository.Query.Include(s=> s.Modules).Where(w=>w.Id== id).First();   

            if (@semestre == null)
            {
                return NotFound();
            }

            return @semestre;
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

        /* private bool semestreExists(long id)
         {
             return (_context.semestres?.Any(e => e.Id == id)).GetValueOrDefault();
         }*/
    }
}

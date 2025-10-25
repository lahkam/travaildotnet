using Microsoft.AspNetCore.Mvc;

namespace isgasoir.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiagnosticsController : ControllerBase
    {
        private readonly ApplicationContext _ctx;

        public DiagnosticsController(ApplicationContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet("dbstate")]
        public IActionResult DbState()
        {
            try
            {
                var fil = _ctx.Filieres.ToList();
                var sem = _ctx.Semestrees.ToList();
                var mod = _ctx.Modules.ToList();
                var chap = _ctx.Chapitres.ToList();
                var acts = _ctx.Activities.ToList();

                return Ok(new { Filieres = fil, Semestres = sem, Modules = mod, Chapitres = chap, Activities = acts });
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}

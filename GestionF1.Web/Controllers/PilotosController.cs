using Microsoft.AspNetCore.Mvc;
using GestionF1.Data.Entidades;
using GestionF1.Servicio;
using GestionF1.Web.Models;


namespace GestionPilotos.Web.Controllers
{
    public class PilotosController : Controller
    {
        private readonly IPilotoServicio _pilotoServicio;
        private readonly IEscuderiasServicio _escuderiasServicio;

        public PilotosController(IPilotoServicio _pilotoService, IEscuderiasServicio _escuderiasService)
        {
            _pilotoServicio = _pilotoService;
            _escuderiasServicio = _escuderiasService;
        }
        [HttpGet]
        public IActionResult ListadoPilotos()
        {
            int? idEscuderia = TempData["IdEscuderia"] as int?;
            List<Piloto> model = new List<Piloto>();
            if (idEscuderia.HasValue)
            {
                model = _pilotoServicio.ObtenerPilotosFiltradosPorEscuderia(idEscuderia.Value);
                ViewBag.Escuderias = _escuderiasServicio.ObtenerEscuderias();
            }
            else
            {
                model = _pilotoServicio.ObtenerPilotos();
                ViewBag.Escuderias = _escuderiasServicio.ObtenerEscuderias();
            }
            return View(model);

        }
        [HttpPost]
        public IActionResult FiltrarPorEscuderia(int idEscuderia)
        {
            TempData["IdEscuderia"] = idEscuderia;
            return RedirectToAction("ListadoPilotos");
        }

        [HttpGet]
        public IActionResult EliminarPiloto(int idPiloto)
        {
            _pilotoServicio.EliminarPiloto(idPiloto);
            return RedirectToAction("ListadoPilotos");
        }

        [HttpGet]
        public IActionResult AgregarPiloto()
        {
            var model = new PilotoViewModel
            {
                EscuderiasDisponibles = _escuderiasServicio.ObtenerEscuderias().Select(Escuderium => new Escuderium
                {
                    IdEscuderia = Escuderium.IdEscuderia,
                    NombreEscuderia = Escuderium.NombreEscuderia
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AgregarPiloto(PilotoViewModel piloto)
        {

            Piloto piloto1 = new Piloto
            {
                NombrePiloto = piloto.NombrePiloto,
                IdEscuderia = piloto.IdEscuderia,
                Eliminado = false
            };

            _pilotoServicio.AgregarPiloto(piloto1);
            return RedirectToAction("ListadoPilotos");
        }
    }
}
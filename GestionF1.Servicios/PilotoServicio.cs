using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionF1.Data.Entidades;
using Microsoft.EntityFrameworkCore;


namespace GestionF1.Servicio
{
    public interface IPilotoServicio
    {
        void AgregarPiloto(Piloto Piloto);
        Piloto? ObtenerPilotoPorId(int id);
        List<Piloto> ObtenerPilotos();
        void EliminarPiloto(int id);

        List<Piloto> ObtenerPilotosFiltradosPorEscuderia(int idEscuderia);
    }
    public class PilotoServicio : IPilotoServicio
    {
        public readonly GestionF1Context _context;

        public PilotoServicio(GestionF1Context context)
        {
            _context = context;
        }
        public void AgregarPiloto(Piloto Piloto)
        {
            _context.Pilotos.Add(Piloto);
            _context.SaveChanges();
        }

        public void EliminarPiloto(int id)
        {

            Piloto? Piloto = ObtenerPilotoPorId(id);
            if (Piloto != null)
            {
                Piloto.Eliminado = true;
                _context.SaveChanges();
            }


        }

        public List<Piloto> ObtenerPilotos()
        {
            return _context.Pilotos.Include(e => e.IdEscuderiaNavigation).Where(e => e.Eliminado == false).ToList();
        }

        public List<Piloto> ObtenerPilotosFiltradosPorEscuderia(int idEscuderia)
        {
            return _context.Pilotos
                .Include(e => e.IdEscuderiaNavigation)
                .Where(d => d.Eliminado == false && d.IdEscuderia == idEscuderia)
                .OrderBy(d => d.IdEscuderiaNavigation.NombreEscuderia)
                .ToList();
        }

        public Piloto? ObtenerPilotoPorId(int id)
        {
            return _context.Pilotos.Find(id);
        }
    }


}
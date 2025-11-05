using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionF1.Data.Entidades;
using Microsoft.EntityFrameworkCore;

namespace GestionF1.Servicio
{
    public interface IEscuderiasServicio
    {
        List<Escuderium> ObtenerEscuderias();
    }
    public class EscuderiasServicio : IEscuderiasServicio
    {
        public readonly GestionF1Context _context;

        public EscuderiasServicio(GestionF1Context context)
        {
            _context = context;
        }
        public List<Escuderium> ObtenerEscuderias()
        {

            return _context.Escuderia
                .OrderBy(d => d.NombreEscuderia)
                .ToList();
        }

    }
}
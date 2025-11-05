using GestionF1.Data.Entidades;

namespace GestionF1.Web.Models
{
    public class PilotoViewModel
    {

        public string NombrePiloto { get; set; }

        public int IdEscuderia { get; set; }

        public List<Escuderium> EscuderiasDisponibles { get; set; }

    }
}

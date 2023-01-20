namespace ManejoPresupuesto.Models
{
    public class ResultadoObtenerPorMes
    {
        public int mes { get; set; }
        public DateTime FechaReferencia { get; set; }
        public decimal Monto { get; set; }
        public decimal Ingreso { get; set; }
        public decimal Gasto { get; set; }
        public TipoOperacion TipoOperacionId { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cole.Models
{
    public class DeudaModel : IComparable<DeudaModel>
    {
        public int Dni { get; set; }
        public string NombreYapellido { get; set; }
        public List<Cuota> CuotasAdeudadas { get; set; }
        public float TotalDeuda { get; set; }


        public int CompareTo(DeudaModel other)
        {
            return this.NombreYapellido.CompareTo(other.NombreYapellido);
        }
    }
}
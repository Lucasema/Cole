//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cole.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Evaluacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Evaluacion()
        {
            this.Califica = new HashSet<Califica>();
        }
    
        public System.DateTime Fecha { get; set; }
        public string Titulo { get; set; }
        public string Tipo { get; set; }
        public Nullable<int> IdMateria { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Califica> Califica { get; set; }
        public virtual Materia Materia { get; set; }
    }
}

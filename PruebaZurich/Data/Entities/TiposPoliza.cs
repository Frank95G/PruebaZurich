using System;
using System.Collections.Generic;

namespace PruebaZurich.Data.Entities;

public partial class TiposPoliza
{
    public int TipoPolizaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Poliza> Polizas { get; set; } = new List<Poliza>();
}

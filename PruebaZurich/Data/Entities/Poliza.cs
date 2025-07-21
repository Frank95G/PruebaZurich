using System;
using System.Collections.Generic;

namespace PruebaZurich.Data.Entities;

public partial class Poliza
{
    public int PolizaId { get; set; }

    public int ClienteId { get; set; }

    public int TipoPolizaId { get; set; }

    public string NumeroPoliza { get; set; } = null!;

    public DateTime FechaInicio { get; set; }

    public DateTime FechaExpiracion { get; set; }

    public decimal MontoAsegurado { get; set; }

    public string Estado { get; set; } = null!;

    public DateTime FechaEmision { get; set; }

    public DateTime? FechaCancelacion { get; set; }

    public string? MotivoCancelacion { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual TiposPoliza TipoPoliza { get; set; } = null!;
}

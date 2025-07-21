using System;
using System.Collections.Generic;

namespace PruebaZurich.Data.Entities;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public int? UsuarioId { get; set; }

    public string Identificacion { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string? Direccion { get; set; }

    public DateTime FechaRegistro { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual ICollection<Poliza> Polizas { get; set; } = new List<Poliza>();

    public virtual Usuario? Usuario { get; set; }
}

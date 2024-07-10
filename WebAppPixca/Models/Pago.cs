using System;
using System.Collections.Generic;

namespace WebAppPixca.Models;

public partial class Pago
{
    public int IdPago { get; set; }

    public string? Tarjeta { get; set; }

    public string? Efectivo { get; set; }

    public string? Trasferencia { get; set; }

    public int IdVenta { get; set; }

    public virtual ICollection<Envio> Envios { get; set; } = new List<Envio>();

    public virtual Ventum IdVentaNavigation { get; set; } = null!;
}

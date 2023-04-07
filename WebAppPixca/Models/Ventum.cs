using System;
using System.Collections.Generic;

namespace WebAppPixca.Models;

public partial class Ventum
{
    public int IdVenta { get; set; }

    public int CantidadCompra { get; set; }

    public float TotalPago { get; set; }

    public DateTime Fecha { get; set; }

    public int IdProduct { get; set; }

    public int IdUsuario { get; set; }

    public virtual Producto IdProductNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; } = new List<Pago>();
}

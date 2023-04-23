using System;
using System.Collections.Generic;

namespace WebAppPixca.Models;

public partial class Carrito
{
    public int IdCarrito { get; set; }

    public int CantidadProductos { get; set; }

    public float TotalPrecio { get; set; }

    public DateTime Fecha { get; set; }

    public int IdProduct { get; set; }

    public int IdUsuario { get; set; }

    public virtual Producto IdProductNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}

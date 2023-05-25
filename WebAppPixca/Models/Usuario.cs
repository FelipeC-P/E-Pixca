using System;
using System.Collections.Generic;

namespace WebAppPixca.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string ApellidoPater { get; set; } = null!;

    public string ApellidoMater { get; set; } = null!;

    public string NumeroTelefono { get; set; } = null!;

    public string? Curp { get; set; }

    public string? Rfc { get; set; }

    public string Email { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public virtual ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();

    public virtual ICollection<Envio> Envios { get; set; } = new List<Envio>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<Ventum> Venta { get; set; } = new List<Ventum>();
}

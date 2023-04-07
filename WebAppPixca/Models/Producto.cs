using System;
using System.Collections.Generic;

namespace WebAppPixca.Models;

public partial class Producto
{
    public int IdProduct { get; set; }

    public string NombreProduct { get; set; } = null!;

    public float Precio { get; set; }

    public int Cantidad { get; set; }

    public string Descripcion { get; set; } = null!;

    public byte[]? Imagen { get; set; }

    public int IdUsuario { get; set; }

    public int IdCategoria { get; set; }

    public virtual Categorium IdCategoriaNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Ventum> Venta { get; } = new List<Ventum>();
}

using System;
using System.Collections.Generic;

namespace WebAppPixca.Models;

public partial class Categorium
{
    public int IdCategoria { get; set; }

    public string NombreCategoria { get; set; } = null!;

    public virtual ICollection<Producto> Productos { get; } = new List<Producto>();
}

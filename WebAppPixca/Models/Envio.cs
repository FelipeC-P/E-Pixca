using System;
using System.Collections.Generic;

namespace WebAppPixca.Models;

public partial class Envio
{
    public int IdEnvio { get; set; }

    public string Calle { get; set; } = null!;

    public string Referencia1 { get; set; } = null!;

    public string Referencia2 { get; set; } = null!;

    public int NumeroExterior { get; set; }

    public string? NumeroInterior { get; set; }

    public string Municipio { get; set; } = null!;

    public string CodigoPostal { get; set; } = null!;

    public string Localidad { get; set; } = null!;

    public int IdUsuario { get; set; }

    public int IdPago { get; set; }

    public virtual Pago IdPagoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}

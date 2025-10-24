using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynAmino.Models;

[Table("NuAutomatizarProcesos")]
public class Proceso
{
    [Key]
    public int ID { get; set; }
    public required string Interfaz { get; set; }
    public required string Descripcion { get; set; }
    public double Valor { get; set; }
    public string? Hora { get; set; }
    public string Periodo { get; set; } = string.Empty;
    public bool Detenido { get; set; } = false;
}
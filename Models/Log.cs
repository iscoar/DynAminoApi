using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynAmino.Models;

[Table("NuLogsAmino")]
public class Log
{
    [Key]
    [Column("ID")]
    public int LogId { get; set; }

    [Column("Cod_Log")]
    public string ?Code { get; set; }

    [Column("Interfaz")]
    public string ?Interfaze { get; set; }

    [Column("Fecha_Log")]
    public DateTime ?LogDate { get; set; }

    [Column("cod_html")]
    public int ?CodeHtml { get; set; }

    [Column("Descripcion")]
    public string ?Description { get; set; }
}
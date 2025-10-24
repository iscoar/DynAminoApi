using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DynAmino.Models;

[Table("NuAmToken")]
public class Token
{
    [Key]
    [Column("client_id")]
    public required string Id { get; set; }
    [Column("grant_type")]
    public required string GrantType { get; set; }
    [Column("client_secret")]
    public required string ClientSecret { get; set; }
    [Column("scope")]
    public required string Scope { get; set; }
    [Column("Type_Dev")]
    public required string Type { get; set; }
}
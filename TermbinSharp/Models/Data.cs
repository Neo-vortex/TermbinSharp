using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TermbinSharp.Models;

public class Data
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    public string DataString { get; set; }
    public string URL { get; set; }

    public byte[] Checksum { set; get; }
}
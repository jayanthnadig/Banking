using System.ComponentModel.DataAnnotations.Schema;

namespace ASNRTech.CoreService.Core.Models {
  [Table("settings", Schema = "public")]
  public class Setting : BaseModel {

    [Column("key")]
    public string Key { get; set; }

    [Column("value")]
    public string Value { get; set; }
  }
}

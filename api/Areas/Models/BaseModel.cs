using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace ASNRTech.CoreService.Core.Models {
  [Serializable]
  public class BaseModel {

    public BaseModel() {
      //this.Id = -1;
      this.LogicalDelete = true;
    }

    [Column("created_by")]
    public string CreatedBy { get; set; }

    [Column("created_date")]
    public DateTime CreatedOn { get; set; }

    [Column("deleted")]
    [JsonIgnore]
    public bool Deleted { get; set; }

    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [NotMapped]
    [JsonIgnore]
    public bool LogicalDelete { get; set; }

    [Column("modified_by")]
    [JsonIgnore]
    public string ModifiedBy { get; set; }

    [Column("modified_date")]
    [JsonIgnore]
    public DateTime? ModifiedOn { get; set; }
  }

  public class DateRange {

    public DateRange(DateTime startDate, DateTime endDate) {
      this.StartDate = startDate;
      this.EndDate = endDate;

      this.EndDate = this.EndDate.AddHours(23).AddMinutes(59).AddSeconds(59);
    }

    public DateTime EndDate { get; set; }
    public DateTime StartDate { get; set; }

    public override string ToString() {
      return $"{this.StartDate.ToString(CultureInfo.InvariantCulture)} :: {this.EndDate.ToString(CultureInfo.InvariantCulture)}";
    }
  }
}

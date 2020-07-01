using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ASNRTech.CoreService.Attributes {
  [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
  public class StringRangeAttribute : ValidationAttribute {
    public string PropertyName { get; set; }
    public string[] AllowableValues { get; set; }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
      if (this.AllowableValues?.Contains(value?.ToString()) == true) {
        return ValidationResult.Success;
      }

      string msg = $"{this.PropertyName} should contain one of: {string.Join(", ", this.AllowableValues ?? new string[] { "No allowable values found" })}.";
      return new ValidationResult(msg);
    }
  }
}

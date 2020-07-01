using System;

namespace TeamLease.CssService.Alcs {
  public class SpData {
    public string Status { get; set; }
    public string Message { get; set; }
    public string MethodName { get; set; }

    public SpData(string methodName) {
      this.MethodName = methodName;
      this.Status = "Ok";
      this.Message = string.Empty;
    }

    public SpData(string methodName, Exception ex) {
      this.MethodName = methodName;
      this.Status = "Error";

      this.Message = string.Empty;

      do {
        this.Message += " " + ex.Message;
        ex = ex.InnerException;
      } while (ex != null);
    }
  }
}

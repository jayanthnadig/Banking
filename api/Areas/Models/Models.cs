using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;

namespace ASNRTech.CoreService.Core.Models {
  public class ErrorResponse : ResponseBase {

    [JsonProperty("errors")]
    public IList<ModelError> ModelErrors { get; set; }
  }

  public class ErrorResponse<T> : ResponseBase<T> {

    [JsonProperty("errors")]
    public IList<ModelError> ModelErrors { get; set; }
  }

  public class DataWithCount<T> {
    public List<T> Data { get; set; }
    public int DataCount { get; set; }
  }

  public class PagedResponse<T> : ResponseBase {

    public PagedResponse() {
    }

    public PagedResponse(int rowCount, IEnumerable<T> data) {
      this.RowCount = rowCount;
      this.DataEntries = data;
    }

    public PagedResponse(List<T> data) {
      if (data == null) {
        throw new ArgumentNullException(nameof(data));
      }
      this.RowCount = data.Count;
      this.DataEntries = data;
    }

    public IEnumerable<T> DataEntries { get; set; }

    public int RowCount { get; set; }
  }

  public class ResponseBase {
    public HttpStatusCode Code { get; set; }
    public dynamic Data { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }

    public string RequestRef { get; set; }
  }

  public class ResponseBase<T> {
    public HttpStatusCode Code { get; set; }
    public T Data { get; set; }
    public string Error { get; set; }
    public string RequestRef { get; set; }
  }
}

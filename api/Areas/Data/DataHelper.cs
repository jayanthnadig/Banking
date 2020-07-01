using Npgsql;
using System.Collections.Generic;
using System.Globalization;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Data;
using ASNRTech.CoreService.Utilities;

namespace ASNRTech.CoreService.Core {
  public static class DataHelper {

    private static NpgsqlDataReader Get(TeamHttpContext teamContext, string sql, List<string> sortableColumns, DataRequest dataRequest) {
      dataRequest.SortBy = dataRequest.SortBy.ToUpper(CultureInfo.InvariantCulture).Trim();
      dataRequest.SortOrder = dataRequest.SortOrder.ToUpper(CultureInfo.InvariantCulture).Trim();

      if (!sortableColumns.Contains(dataRequest.SortBy)) {
        dataRequest.SortBy = Constants.SORT_BY_ID;
      }
      if (dataRequest.SortOrder != "ASC" && dataRequest.SortOrder != "DESC") {
        dataRequest.SortOrder = "ASC";
      }

      sql += $" order by {dataRequest.SortBy} {dataRequest.SortOrder} ";
      sql += " limit @limit offset @offset";

      PostgresService dl = new PostgresService();

      if (dataRequest.HasDateFiltering) {
        dl.AddDateParam("@startDate", dataRequest.From);
        dl.AddDateParam("@endDate", dataRequest.To);
      }

      dl.AddParam("@blankQuery", dataRequest.IsBlankQuery);
      dl.AddLikeParam("@query", dataRequest.Query);
      dl.AddIntParam("@offset", dataRequest.Offset);
      dl.AddIntParam("@limit", dataRequest.Limit);

      return dl.ExecuteSqlReturnReader(Utility.ConnString, sql);
    }

    private static int GetDataCount(TeamHttpContext teamContext, string sql, DataRequest pagingRequest) {
      PostgresService dl = new PostgresService();

      if (pagingRequest.HasDateFiltering) {
        dl.AddDateParam("@startDate", pagingRequest.From);
        dl.AddDateParam("@endDate", pagingRequest.To);
      }

      dl.AddParam("@blankQuery", pagingRequest.IsBlankQuery);
      dl.AddLikeParam("@query", pagingRequest.Query);
      dl.AddIntParam("@offset", pagingRequest.Offset);
      dl.AddIntParam("@limit", pagingRequest.Limit);

      return dl.ExecuteSqlReturnScalar<int>(Utility.ConnString, sql);
    }
  }
}

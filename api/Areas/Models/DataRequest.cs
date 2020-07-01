using System;
using ASNRTech.CoreService.Utilities;

namespace ASNRTech.CoreService.Core.Models {
    public class DataRequest {
        private string _sortBy;
        private string _sortOrder;
        private DateTime? _from;
        private DateTime? _to;
        private int _limit;
        private int _offset;
        private DateRange dateRange;

        public bool IsBlankQuery {
            get {
                return !this.Query.HasValue();
            }
        }

        public bool FilterByQuery { get; set; }

        public int Limit {
            get {
                return _limit <= 0 ? Constants.DEFAULT_PAGE_SIZE : _limit;
            }
            set { _limit = value; }
        }

        public int Offset {
            get {
                return _offset < 0 ? 0 : _offset;
            }
            set { _offset = value; }
        }

        public string Query { get; set; }

        public string SortBy {
            get {
                return _sortBy.HasValue() ? _sortBy : Constants.SORT_BY_ID;
            }
            set { _sortBy = value; }
        }

        public DateTime? From {
            get {
                return _from == null || _from.Value == DateTime.MinValue ? Constants.MIN_DATE : _from.Value;
            }
            set { _from = value; }
        }

        public DateTime? To {
            get {
                return _to == null || _to.Value == DateTime.MinValue ? Constants.MAX_DATE : _to.Value;
            }
            set { _to = value; }
        }

        public string SortOrder {
            get {
                return _sortOrder.HasValue() ? _sortOrder : "asc";
            }
            set { _sortOrder = value; }
        }

        public DateRange TimeFrame {
            get {
                return dateRange ?? new DateRange(this.From.Value, this.To.Value);
            }
            private set {
                dateRange = value;
            }
        }

        public bool HasDateFiltering { get; set; }

        public DataRequest() {
            this.HasDateFiltering = true;
            this.FilterByQuery = false;
        }

        public DataRequest(int year, int month, int offset, int limit) {
            if (year == 0) {
                year = DateTime.Today.Year;
            }
            if (month == 0) {
                month = DateTime.Today.Month;
            }
            this.dateRange = DateUtility.GetMonthDateRange(year, month);

            this.From = dateRange.StartDate;
            this.To = dateRange.EndDate;
            this.Offset = offset;
            this.Limit = limit;
            this.HasDateFiltering = true;
        }
    }
}

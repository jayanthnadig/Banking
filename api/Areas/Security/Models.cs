using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASNRTech.CoreService.Core.Models;
using ASNRTech.CoreService.Enums;

namespace ASNRTech.CoreService.Security {
    [Serializable]
    [Table("users", Schema = "public")]
    public class User : BaseModel {

        [Column("first_name")]
        [Required]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("last_logged_in")]
        public DateTime? LastLoggedIn { get; set; }

        [Column("source")]
        public string Source { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("user_id")]
        public string UserId { get; set; }

        [Column("user_type")]
        public UserType UserType { get; set; }

        [Column("password")]
        [JsonIgnore]
        [Required]
        public string Password { get; set; }

        [Column("user_status")]
        [Required]
        public UserStatus Status { get; set; }

        [Column("profile_img_url")]
        public string ProfileImageUrl { get; set; }

        [Column("phonenumber")]
        public string PhoneNumber { get; set; }

        public string Name() {
            return $"{this.FirstName} {this.LastName}";
        }

        public User() {
            this.Status = UserStatus.Active;
            //this.Clients = new List<ClientDto>();
        }
    }

    [Table("user_sessions", Schema = "public")]
    public class UserSession : BaseModel {

        [Column("user_id")]
        public string UserId { get; set; }

        [Column("session_id")]
        public string SessionId { get; set; }

        [Column("last_api_call")]
        public DateTime LastApiCall { get; set; }

        [Column("active")]
        public bool Active { get; set; }

        public UserSession() {
            this.Active = true;
        }
    }

    public class LoginModel {
        public string UserId { get; set; }
        public string Password { get; set; }
        //public string LoginAccessKey { get; set; }
    }

    public class LoginResponseModel {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public UserType Type { get; set; }
        public string SessionId { get; set; }
        public long ExpiryTimestamp { get; set; }
        public LoginStatus Status { get; set; }
    }

    [Table("upload_log", Schema = "public")]
    public class UploadLog : BaseModel {

        [Column("client_id")]
        public string ClientId { get; set; }

        [Column("am_id")]
        public string AmId { get; set; }

        [Column("associatename")]
        public string associateName { get; set; }

        [Column("upload_type")]
        public int type { get; set; }

        [Column("log_message")]
        public string ErrorMessage { get; set; }

        [Column("upload_id")]
        public int jobID { get; set; }
    }

    //[Table("Transactions", Schema = "public")]
    //public class Transactions : BaseModel {
    //    [Column("id")]
    //    public int TransactionId { get; set; }

    //    [Column("branchname")]
    //    public string BranchName { get; set; }

    //    [Column("branchcode")]
    //    public string BranchCode { get; set; }

    //    [Column("total_transactions")]
    //    public int TotalTransactions { get; set; }

    //    [Column("makerid")]
    //    public string MakerId { get; set; }

    //    [Column("assignedto")]
    //    public string AssignedTo { get; set; }

    //    [Column("postingdate")]
    //    public DateTime? PostingDate { get; set; }

    //    [Column("functionid")]
    //    public string FunctionId { get; set; }

    //    [Column("transaction_status")]
    //    public string TransactionStatus { get; set; }

    //    [Column("branch_type")]
    //    public string BranchType { get; set; }

    //    [Column("status")]
    //    public string Status { get; set; }
    //}
}

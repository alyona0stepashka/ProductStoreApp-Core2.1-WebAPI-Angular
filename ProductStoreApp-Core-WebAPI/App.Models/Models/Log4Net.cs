using System;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class Log4Net
    {
        [Key]
        public int Id { get; set; }

        public DateTime RequestTime { get; set; }
        public string RequestURL { get; set; }
        public string RequestUserName { get; set; }
        public string RequestHeaders { get; set; }
        public string RequestBody { get; set; }
        public string RequestQueryString { get; set; }
        public string RequestHttpVerb { get; set; }

        public DateTime ResponseTime { get; set; }
        public string RequestURI_r { get; set; }
        public string ResponseUserName { get; set; }
        public string ResponseHeaders { get; set; }
        public string ResponseStatusCode { get; set; }

        public string ExceptionMessage { get; set; }
        public string StackTrace { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace App.Models
{
    public class EventLog
    {
        [Key]
        public int Id { get; set; }

        public int EventId { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
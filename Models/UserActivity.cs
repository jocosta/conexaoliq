using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CTX.Bot.ConexaoLiq.Models
{
    public class UserActivity
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Channel { get; set; }
        public string Activity { get; set; }
        public DateTime Data { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SignalRAPI.Models
{
    [Route("api/[controller]")]
    [ApiController]
    public class Message : ControllerBase
    {
        public string MyUserName { get; set; }
        public string UserName { get; set; }
        public string ConnectionId { get; set; }
        public string MessageText { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRAPI.Hubs;
using SignalRAPI.Models;
using SignalRAPI.Models.Tabel;
using static SignalRAPI.Hubs.MessageHub;

namespace SignalRAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private IHubContext<MessageHub> _messageHubContext;
        private AuthenticationContext _context;

        public MessageController(IHubContext<MessageHub> messageHubContext , AuthenticationContext context)
        {
            _messageHubContext = messageHubContext;
            _context = context;
        }

        [HttpGet]
        [Route("Test")]
        public object TestGet()
        {
            return Ok("True");
        }

        [HttpPost]
        [Route("AddToChat")]
        public object PostUserChat()
        {
            Chat chat = new Chat
            {
                UserName = "Osama",
            };

            _context.Chat.Add(chat);
            _context.SaveChanges();

            return Ok("True");
        }

        [HttpPost("{UserName}/{MessageText}")]
        [Route("Post")]
        public IActionResult Post([FromQuery] string UserName, [FromQuery] string MessageText)
        {
            _messageHubContext.Clients.All.SendAsync("send", "Hello From The Service" + UserName + " " + MessageText);
            return Ok();
        }

        [HttpPost]
        [Route("PostModel")]
        public IActionResult PostModel(Message model)
        {
            _messageHubContext.Clients.All.SendAsync("send", model.UserName + " : " + model.MessageText);
            return Ok();
        }

        [HttpPost]
        [Route("UpdateIdConnection")]
        public async Task<IActionResult> UpdateIdConnection(Message model)
        {


            return Ok(model.ConnectionId);
        }

        [HttpPost]
        [Route("PostModelCaller")]
        public async Task<IActionResult> Post(Message model)
        {
            var User = _context.Chat.FirstOrDefault(a => a.UserName == model.MyUserName);

            var SendToUser = _context.Chat.FirstOrDefault(a => a.UserName == model.UserName);

            _context.SaveChanges();
            
            await _messageHubContext.Clients.Clients(SendToUser.ConnectionId).SendAsync("sendMessageToUser", User.UserName + model.MessageText);
            await _messageHubContext.Clients.Clients(User.ConnectionId).SendAsync("sendMessageToUser", User.UserName + model.MessageText);
            return Ok(User.ConnectionId + "    " + SendToUser.ConnectionId);
        }

        [HttpGet]
        [Route("GetALLConnetctionId")]
        public object GetALLConnetctionId()
        {
            return ConnectedUser.Ids;
        }
    }
}

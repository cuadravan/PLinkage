using Microsoft.AspNetCore.Mvc;
using PLinkageAPI.Interfaces;
using PLinkageShared.DTOs;

namespace PLinkageAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("summaries/{userId}")]
        public async Task<ActionResult<IEnumerable<ChatSummaryDto>>> GetChatSummaries(Guid userId)
        {
            var summaries = await _chatService.GetChatSummariesAsync(userId);
            return Ok(summaries);
        }


        [HttpGet("{chatId}/messages/{viewingUserId}")]
        public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetChatMessages(Guid chatId, Guid viewingUserId)
        {
            var messages = await _chatService.GetChatMessagesAsync(chatId, viewingUserId);
            return Ok(messages);
        }

        [HttpPost("send/{senderId}")]
        public async Task<ActionResult<ChatMessageDto>> SendMessage(Guid senderId, [FromBody] SendMessageDto messageDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newMessage = await _chatService.SendMessageAsync(messageDto, senderId);
                return Ok(newMessage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

using Forge.OpenAI.Authentication;
using Forge.OpenAI.Models.ChatCompletions;
using Forge.OpenAI.Models.Models;
using Forge.OpenAI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPlayer.UIComponents.ViewModels
{
    public class AIService
    {
        //private readonly ChatCompletionService _chatService;

        //public AIService(string apiKey)
        //{
        //    var auth = new OpenAIAuthentication(apiKey);
        //    _chatService = new ChatCompletionService(auth);
        //}

        //public async Task<string> AskAsync(string prompt)
        //{
        //    var request = new ChatCompletionRequest
        //    {
        //        Messages = new List<ChatMessage>
        //        {
        //            new ChatMessage(ChatRole.User, prompt)
        //        },
        //        Model = Model.Gpt3_5Turbo
        //    };

        //    var response = await _chatService.CreateCompletionAsync(request);

        //    return response.Choices.FirstOrDefault()?.Message?.Content ?? "";
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Services.IServices.IModel
{
    public interface IModelProvider
    {
        /// <summary>
        /// Returns the assistant reply text for the given context.
        /// </summary>
        /// <param name="systemPrompt">agent system instruction (optional)</param>
        /// <param name="messages">sequence of (role, content) messages (user/assistant)</param>
        /// <param name="modelName">provider/model name</param>
        Task<string> GetCompletionAsync(string systemPrompt, IEnumerable<(string Role, string Content)> messages, string modelName, CancellationToken ct = default);
    }
}

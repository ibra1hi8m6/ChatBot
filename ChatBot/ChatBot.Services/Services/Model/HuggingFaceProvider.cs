using ChatBot.Services.IServices.IModel;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatBot.Services.Services.Model
{
    public class HuggingFaceProvider : IModelProvider
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;

        public HuggingFaceProvider(HttpClient client, IConfiguration config)
        {
            _client = client;
            _apiKey = config["HuggingFace:ApiKey"] ?? throw new InvalidOperationException("HF API key missing");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> GetCompletionAsync(string systemPrompt, IEnumerable<(string Role, string Content)> messages, string modelName, CancellationToken ct = default)
        {
            // Build a single textual prompt from system + messages
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(systemPrompt))
            {
                sb.AppendLine($"[SYSTEM]: {systemPrompt}");
                sb.AppendLine();
            }

            foreach (var m in messages)
            {
                var role = m.Role?.ToLowerInvariant() ?? "user";
                sb.AppendLine(role == "assistant" ? $"[ASSISTANT]: {m.Content}" : $"[USER]: {m.Content}");
            }

            sb.AppendLine();
            sb.Append("[ASSISTANT]:"); // model should continue this

            var payload = new
            {
                inputs = sb.ToString(),
                parameters = new
                {
                    max_new_tokens = 512,
                    temperature = 0.7
                }
            };

            var json = JsonSerializer.Serialize(payload);
            using var req = new HttpRequestMessage(HttpMethod.Post, $"https://api-inference.huggingface.co/models/{modelName}")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var resp = await _client.SendAsync(req, ct);
            resp.EnsureSuccessStatusCode();
            var respText = await resp.Content.ReadAsStringAsync(ct);

            // HF returns JSON that may vary by model. A common format is [{"generated_text":"..."}] or {"error": "..."}
            // Parse naive: try to find generated_text
            try
            {
                using var doc = JsonDocument.Parse(respText);
                if (doc.RootElement.ValueKind == JsonValueKind.Array && doc.RootElement[0].TryGetProperty("generated_text", out var gen))
                {
                    return gen.GetString() ?? string.Empty;
                }

                if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("generated_text", out var gen2))
                {
                    return gen2.GetString() ?? string.Empty;
                }

                // fallback: return whole response
                return respText;
            }
            catch
            {
                return respText;
            }
        }
    }
}

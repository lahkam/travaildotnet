using System.Net.Http;
using System.Threading.Tasks;

namespace isgasoir.Services.ServiceApi
{
    // Simple wrapper service that would call an LLM endpoint in production.
    // For the student project we provide a basic in-process generator.
    public class LLMApiImpl
    {
        private readonly HttpClient _httpClient;

        public LLMApiImpl(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<string> GenerateActivityAsync(string chapitreTitle, string chapitreContent)
        {
            // Simple deterministic generator for the project assignment.
            var promptTitle = $"Activité pour le chapitre: {chapitreTitle}";
            var instructions = $"Lire le chapitre '{chapitreTitle}' et répondre aux questions:\n1) Résumez les points clés.\n2) Donnez un exemple d'application.\n3) Proposez un exercice pratique (30-60 minutes).\n\nContexte: {chapitreContent}";
            var combined = $"{promptTitle}\n\n{instructions}";
            return Task.FromResult(combined);
        }
    }
}

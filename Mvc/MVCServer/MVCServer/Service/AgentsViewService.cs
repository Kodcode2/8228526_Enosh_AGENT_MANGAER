using MVCServer.Dto;
using MVCServer.ViewModels;
using System.Text.Json;

namespace MVCServer.Service
{
    public class AgentsViewService(IHttpClientFactory clientFactory) : IAgentsViewService
    {
        private readonly string baseUrl = "https://localhost:7058/Agents";

        public async Task<List<AgentVM>> GetAllAgents()
        {
            try
            {
                HttpClient httpClient = clientFactory.CreateClient();
                HttpResponseMessage httpResponse = await httpClient.GetAsync($"{baseUrl}");

                if (!httpResponse.IsSuccessStatusCode) { throw new Exception("Failed to fetch Agents."); }

                string content = await httpResponse.Content.ReadAsStringAsync();
                List<AgentVM> agents = JsonSerializer.Deserialize<List<AgentVM>>(
                    content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];

                return agents;
            }
            catch (Exception ex)
            {
                // logger.LogError(ex, "An error occurred while fetching Agents.");
                throw new Exception(ex.Message);
            }
        }
    }
}

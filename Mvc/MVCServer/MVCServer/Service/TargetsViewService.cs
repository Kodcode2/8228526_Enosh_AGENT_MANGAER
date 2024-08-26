using MVCServer.ViewModels;
using System.Text.Json;

namespace MVCServer.Service
{
    public class TargetsViewService(IHttpClientFactory clientFactory) : ITargetsViewService
    {
        private readonly string baseUrl = "https://localhost:7058/Targets";

        public async Task<List<TargetVM>> GetAllTargets()
        {
            try
            {
                HttpClient httpClient = clientFactory.CreateClient();
                HttpResponseMessage httpResponse = await httpClient.GetAsync($"{baseUrl}");

                if (!httpResponse.IsSuccessStatusCode) { throw new Exception("Failed to fetch Agents."); }

                string content = await httpResponse.Content.ReadAsStringAsync();
                List<TargetVM> targets = JsonSerializer.Deserialize<List<TargetVM>>(
                    content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];

                return targets;
            }
            catch (Exception ex)
            {
                // logger.LogError(ex, "An error occurred while fetching Agents.");
                throw new Exception(ex.Message);
            }
        }
    }
}

using MVCServer.ViewModels;
using System.Text;
using System.Text.Json;

namespace MVCServer.Service
{
    public class MissionsViewService(IHttpClientFactory clientFactory) : IMissionsViewService
    {
        private readonly string baseUrl = "https://localhost:7058/Missions";

        public async Task<List<MissionVM>> GetAllMissions()
        {
            try
            {
                HttpClient httpClient = clientFactory.CreateClient();
                HttpResponseMessage httpResponse = await httpClient.GetAsync($"{baseUrl}");

                if (!httpResponse.IsSuccessStatusCode) { throw new Exception("Failed to fetch Agents."); }

                string content = await httpResponse.Content.ReadAsStringAsync();
                List<MissionVM> missions = JsonSerializer.Deserialize<List<MissionVM>>(
                    content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];

                return missions;
            }
            catch (Exception ex)
            {
                // logger.LogError(ex, "An error occurred while fetching Agents.");
                throw new Exception(ex.Message);
            }
        }

        public async Task<MissionVM> AssignMission(int id)
        {
            try
            {
                HttpClient httpClient = clientFactory.CreateClient();
                HttpContent requestContent = new StringContent(JsonSerializer.Serialize(id), Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = await httpClient.PutAsync($"{baseUrl}/{id}", requestContent);

                if (!httpResponse.IsSuccessStatusCode) { throw new Exception("Failed to fetch Agents."); }

                string responseContent = await httpResponse.Content.ReadAsStringAsync();
                MissionVM? mission = JsonSerializer.Deserialize<MissionVM>(
                    responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return mission;
            }
            catch (Exception ex)
            {
                // logger.LogError(ex, "An error occurred while fetching Agents.");
                throw new Exception(ex.Message);
            }
        }
    }
}

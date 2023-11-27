using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MicrosoftIdentity_Learning.DTO;

namespace MicrosoftIdentity_Learning.Pages.HR
{
    [Authorize(Policy = "HRManagementOnly")]
    public class HRManagementModel : PageModel
    {
        private readonly IHttpClientFactory httpClientFactory;

        [BindProperty]
        public List<WeatherForecastDTO> weatherForecastsItems { get; set; } = new List<WeatherForecastDTO>();

        public HRManagementModel(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }


        public async Task OnGetAsync()
        {
            var httpClient = httpClientFactory.CreateClient("WebApiForecastClient");
            weatherForecastsItems = await httpClient.GetFromJsonAsync<List<WeatherForecastDTO>>("WeatherForecast") ?? new List<WeatherForecastDTO>();
        }
    }
}

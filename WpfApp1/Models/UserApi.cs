using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WpfApp1.DTO;

namespace WpfApp1.Models
{
    public class UserApi : RestApi
    {
        private string userToken;

        public void SetToken(string token)
        {
            userToken = token;
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userToken);
        }

        public async Task<UserDTO> GetUserInfoAsync(int id)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"{id}");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                return JsonSerializer.Deserialize<UserDTO>(responseBody, options);
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"사용자 정보를 가져오는 데 실패했습니다: {response.StatusCode}, 내용: {errorResponse}");
            }
        }
    }
}

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
    public class UnitApi : RestApi
    {
        private string userToken;

        public void SetToken(string token)
        {
            userToken = token;
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userToken);
        }
        public async Task<List<UnitDTO>> GetUnitDataAsync(int id)
        {
            HttpResponseMessage response = await httpClient.GetAsync($"Unit/house/{id}");

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody); // JSON 데이터 출력

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                // 배열로 역직렬화
                return JsonSerializer.Deserialize<List<UnitDTO>>(responseBody, options);
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"상세 정보를 가져오는 데 실패했습니다: {response.StatusCode}, 내용: {errorResponse}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WpfApp1.DTO;

namespace WpfApp1.Models
{
    public class HouseApi : RestApi
    {
        // House API에서 모든 데이터 가져오기
        public async Task<List<HouseDTO>> GetAllHousesAsync()
        {
            // 인증 헤더에 토큰 설정
            string token = TokenSave.GetToken(); // TokenSave에서 토큰 가져오기
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // "/House/all" 경로로 요청
            var response = await httpClient.GetAsync("House/all");

            // 요청 성공 확인
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<List<HouseDTO>>(responseBody, options);
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"API 호출 실패: {response.StatusCode}, 내용: {errorResponse}");
            }
        }
    }
}

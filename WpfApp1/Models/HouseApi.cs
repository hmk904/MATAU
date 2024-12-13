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
        private string userToken;

        // 사용자 토큰 설정
        public void SetToken(string token)
        {
            userToken = token;
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", userToken);
        }

        // House API에서 모든 데이터 가져오기
        public async Task<List<HouseDTO>> GetAllHousesAsync()
        {
            // GET 요청
            var response = await httpClient.GetAsync("House/all");

            // 요청 성공 확인
            if (response.IsSuccessStatusCode)
            {
                // 응답 데이터 읽기
                var responseBody = await response.Content.ReadAsStringAsync();

                // JSON -> 객체 변환
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<List<HouseDTO>>(responseBody, options);
            }
            else
            {
                // 오류 처리
                string errorResponse = await response.Content.ReadAsStringAsync();
                throw new Exception($"API 호출 실패: {response.StatusCode}, 내용: {errorResponse}");
            }
        }
    }
}

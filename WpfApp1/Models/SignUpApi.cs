using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class SignUpApi : RestApi
    {

        public async Task<bool> RegisterUser(SignUpDTO user)
        {
            try
            {
                // JSON 직렬화
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase // camelCase로 변환
                };
                string jsonData = JsonSerializer.Serialize(user, options);

                // HTTP 요청 콘텐츠 생성
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // POST 요청 전송
                HttpResponseMessage response = await httpClient.PostAsync("/api/User/register", content);

                // 응답 상태 확인
                if (response.IsSuccessStatusCode)
                {
                    return true; // 요청 성공
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    throw new Exception($"서버 응답 실패: {response.StatusCode}, 내용: {errorResponse}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"REST API 호출 오류: {ex.Message}");
            }
        }

    }
}

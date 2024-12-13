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
    public class LoginApi : RestApi
    {
        public async Task<LoginResponseDTO> LoginUser(LoginDTO user)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                string jsonData = JsonSerializer.Serialize(user, options);

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("login", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<LoginResponseDTO>(responseBody, options);
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

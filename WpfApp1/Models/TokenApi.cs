using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WpfApp1.DTO;

namespace WpfApp1.Models
{
    public class TokenApi
    {
        private string loginToekn;
        private readonly LoginApi loginApi;

        public TokenApi()
        {
            loginApi = new LoginApi();
        }

        // 토큰 저장
        public void StoreToken(string token)
        {
            loginToekn = token;
        }

        // 저장된 토큰 반환
        public string GetToken()
        {
            if (string.IsNullOrEmpty(loginToekn))
                throw new Exception("토큰이 설정되지 않았습니다.");
            return loginToekn;
        }

        // HTTP 요청에 토큰 추가
        public void AddAuthorizationHeader(HttpClient client)
        {
            if (string.IsNullOrEmpty(loginToekn))
                throw new Exception("유효한 토큰이 없습니다.");
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginToekn);
        }

        // 로그인하고 토큰 저장
        public async Task LoginAndStoreToken(LoginDTO user)
        {
            var response = await loginApi.LoginUser(user); // 로그인 API 호출
            if (!string.IsNullOrEmpty(response.Token))
            {
                StoreToken(response.Token); // 토큰 저장
            }
            else
            {
                throw new Exception("로그인 실패: 유효한 토큰을 받지 못했습니다.");
            }
        }
    }
}

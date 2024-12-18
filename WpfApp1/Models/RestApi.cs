using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public class RestApi
    {
        protected static readonly HttpClient httpClient;

        static RestApi()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://3.38.45.83/api/") // 공통 API 기본 URL 설정
            };
        }
    }
}

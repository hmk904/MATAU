using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    
    public class RestApi
    {
        public readonly HttpClient httpClient;


        public RestApi()
        {
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://3.38.255.138/dev/api/User/") // API 기본 URL 설정
            };
        }
        
    }
}

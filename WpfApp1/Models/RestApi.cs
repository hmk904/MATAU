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
                BaseAddress = new Uri("http://mataju-296158602.ap-northeast-2.elb.amazonaws.com/api/") // 공통 API 기본 URL 설정
            };
        }
    }
}

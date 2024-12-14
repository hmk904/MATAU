using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Models
{
    public static class TokenSave
    {
        private static string tokenSave;
        private static int loginUserId;

        public static void SetToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("토큰 값이 유효하지 않습니다.", nameof(token));

            tokenSave = token;
            Console.WriteLine($"토큰 설정: {token}");
        }

        public static string GetToken()
        {
            if (string.IsNullOrEmpty(tokenSave))
            {
                Console.WriteLine("토큰이 설정되지 않았습니다.");
                throw new InvalidOperationException("토큰이 설정되지 않았습니다.");
            }

            Console.WriteLine($"토큰 반환: {tokenSave}");
            return tokenSave;
        }

        // 사용자 ID 저장
        public static void SetUserId(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("사용자 ID는 0보다 커야 합니다.", nameof(userId));
            }

            loginUserId = userId;
            Console.WriteLine($"사용자 ID 설정: {loginUserId}");
        }

        // 사용자 ID 반환
        public static int GetUserId()
        {
            if (loginUserId <= 0)
            {
                Console.WriteLine("사용자 ID가 설정되지 않았습니다.");
                throw new InvalidOperationException("사용자 ID가 설정되지 않았습니다.");
            }

            Console.WriteLine($"사용자 ID 반환: {loginUserId}");
            return loginUserId;
        }

    }
}

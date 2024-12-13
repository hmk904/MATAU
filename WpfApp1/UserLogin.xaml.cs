using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Controls;

namespace WpfApp1
{
    /// <summary>
    /// LoginTest.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserLogin : Window
    {
        public UserLogin()
        {
            InitializeComponent();

            // 초기 화면으로 로그인 컨트롤 로드
            RightContent.Content = new loginControl(); // loginControl은 로그인 화면입니다.
        }

        private void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            // ContentControl에 회원가입 화면 로드
            RightContent.Content = new signUpControl(); // signUpControl이 회원가입 화면
        }
    }
}

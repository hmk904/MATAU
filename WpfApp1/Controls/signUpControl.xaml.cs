using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.Models;

namespace WpfApp1.Controls
{
    /// <summary>
    /// signUpControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class signUpControl : UserControl
    {
        public signUpControl()
        {
            InitializeComponent();
        }

        private void BackToLogin(object sender, RoutedEventArgs e)
        {
            DependencyObject parent = this;
            while (parent != null && !(parent is UserLogin))
            {
                parent = LogicalTreeHelper.GetParent(parent);
            }

            if (parent is UserLogin loginTest)
            {
                loginTest.RightContent.Content = new loginControl();
            }
        }

        private async void registButton_Click(object sender, RoutedEventArgs e)
        {
            string name = txtID.Text;
            string password = txtPW.Password;
            string confirmPassword = txtRePW.Password; // 비밀번호 확인 필드
            string nickname = txtNickName.Text;

            // signUp 객체 생성
            var user = new SignUpDTO
            {
                Name = name,
                Password = password,
                NickName = nickname
            };

            var api = new SignUpApi();

            try
            {
                // signUp 객체를 RegisterUser에 전달
                bool success = await api.RegisterUser(user);

                if (success)
                {
                    MessageBox.Show("회원가입이 완료되었습니다!");

                    DependencyObject parent = this;
                    while (parent != null && !(parent is UserLogin))
                    {
                        parent = LogicalTreeHelper.GetParent(parent);
                    }

                    if (parent is UserLogin loginTest)
                    {
                        loginTest.RightContent.Content = new loginControl();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"회원가입 실패: {ex.Message}");
            }
        }

        // 비밀번호 확인 실시간 검증
        private void txtPW_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordsEquals();
        }

        private void txtRePW_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordsEquals();
        }

        private void PasswordsEquals()
        {
            string password = txtPW.Password;
            string confirmPassword = txtRePW.Password;

            // 비밀번호 확인 결과를 UI에 표시
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                lblPasswordCheck.Text = string.Empty; // 아무 메시지도 표시하지 않음
                btnRegister.IsEnabled = false; // 회원가입 버튼 비활성화
            }
            else if (password == confirmPassword)
            {
                lblPasswordCheck.Text = "비밀번호가 일치합니다.";
                lblPasswordCheck.Foreground = new SolidColorBrush(Colors.Green);
                btnRegister.IsEnabled = true; // 회원가입 버튼 활성화
            }
            else
            {
                lblPasswordCheck.Text = "비밀번호가 일치하지 않습니다.";
                lblPasswordCheck.Foreground = new SolidColorBrush(Colors.Red);
                btnRegister.IsEnabled = false; // 회원가입 버튼 비활성화
            }
        }
    }
}

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
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            string name = txtName.Text;
            string password = txtPW.Password;
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"회원가입 실패: {ex.Message}");
            }
        }

    }
}

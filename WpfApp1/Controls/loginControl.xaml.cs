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
using WpfApp1.DTO;
using WpfApp1.Models;
using MaterialDesignThemes.Wpf; // MaterialDesign 추가

namespace WpfApp1.Controls
{
    /// <summary>
    /// loginControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class loginControl : UserControl
    {
        public loginControl()
        {
            InitializeComponent();
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            var loginApi = new LoginApi();
            var loginInfo = new LoginDTO
            {
                Name = txtID.Text,
                Password = txtPW.Password
            };

            try
            {
                // 로그인 요청
                var response = await loginApi.LoginUser(loginInfo);
                TokenSave.SetToken(response.Token);
                TokenSave.SetUserId(response.UserId);

                // 모달 메시지 설정
                ModalMessage.Text = $"로그인 성공!";

                // DialogHost 모달 열기
                await LoginDialogHost.ShowDialog(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"로그인 실패: {ex.Message}");
            }
        }

        private void CloseDialog_Click(object sender, RoutedEventArgs e)
        {
            // 모달 창 닫기
            DialogHost.CloseDialogCommand.Execute(null, null);

            // MainView 열기
            var mainView = new Views.MainView();
            mainView.Show();

            // 현재 창 닫기
            Window.GetWindow(this)?.Close();
        }




    }
}

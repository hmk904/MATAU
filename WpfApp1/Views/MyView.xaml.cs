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
using WpfApp1.DTO;
using WpfApp1.Models;


namespace WpfApp1.Views
{
    public partial class MyView : Window
    {
        private int userId;

        public MyView(int userId)
        {
            InitializeComponent();
            this.userId = userId;

            // ID를 이용해 사용자 정보 로드
            LoadUserInfo();
        }

        private async void LoadUserInfo()
        {
            try
            {
                var userApi = new UserApi();
                userApi.SetToken(TokenSave.GetToken());
                var userInfo = await userApi.GetUserInfoAsync(userId);

                DataContext = userInfo;

                // 사용자 정보 확인
                Console.WriteLine($"사용자 정보:\nNickname: {userInfo.Nickname}\nRoles: {userInfo.Roles}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"사용자 정보를 로드하는 데 실패했습니다: {ex.Message}");
            }
        }
        private void MyViewButton_Click(object sender, RoutedEventArgs e)
        {
            // 뒤로가기 버튼 클릭 시 창 닫기
            this.Hide(); // 창 숨기기

            MainView mainView = new MainView();
            Application.Current.MainWindow = mainView;
            mainView.Show();

            this.Close(); // 완전히 닫기
        }

    }

}


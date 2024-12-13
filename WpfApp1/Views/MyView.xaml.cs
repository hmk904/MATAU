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

namespace WpfApp1.Views
{
    /// <summary>
    /// MainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MyView : Window
    {
        public MyView()
        {
            InitializeComponent();

            // 데이터 바인딩 확인 (테스트용)
            if (DataContext is UserDTO user)
            {
                MessageBox.Show($"사용자 정보: \nID: {user.Roles}\nName: {user.Name}\nNickname: {user.Nickname}");
            }
        }
    }
}

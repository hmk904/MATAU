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

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // 로그인 창 실행
            UserLogin loginWindow = new UserLogin();
            bool? dialogResult = loginWindow.ShowDialog();

            // 로그인 성공 여부에 따른 처리
            if (dialogResult == true)
            {
                InitializeComponent(); // MainWindow를 초기화하여 표시
            }
            else
            {
                Application.Current.Shutdown(); // 로그인 실패 시 애플리케이션 종료
            }
        }
    }


}

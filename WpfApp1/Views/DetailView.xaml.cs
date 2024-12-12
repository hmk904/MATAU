using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1.Views
{
    public partial class DetailView : Window
    {
        public DetailView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 버튼 초기 스타일
            ResetButtonStyle(smallButton);
            ResetButtonStyle(middleButton);
            ResetButtonStyle(largeButton);

            // 클릭된 버튼 스타일 변경
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                ApplySelectedStyle(clickedButton);
            }
        }

        private void ResetButtonStyle(Button button)
        {
            button.Background = Brushes.White;
            button.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"));
            button.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"));
        }

        private void ApplySelectedStyle(Button button)
        {
            button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"));
            button.Foreground = Brushes.White;
            button.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4CAF50"));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApp1.DTO;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp1.Views
{
    public partial class DetailView : Window
    {
        private HouseDTO houseInfo;

        public DetailView(HouseDTO houseDetail)
        {
            InitializeComponent();
            houseInfo = houseDetail;
            this.DataContext = houseInfo;

            // 이미지 로드
            LoadImages(houseInfo.Province);
        }

        private void LoadImages(string province)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = Path.Combine(basePath, @"..\..\..\WpfApp1\Resources");

            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show($"이미지 경로를 찾을 수 없습니다: {folderPath}");
                return;
            }

            List<string> imageFiles = new List<string>();
            foreach (string file in Directory.GetFiles(folderPath))
            {
                string fileName = Path.GetFileName(file);
                if (fileName.StartsWith(province) && (fileName.Contains("-1") || fileName.Contains("-2") || fileName.Contains("-3")))
                {
                    imageFiles.Add(file);
                }
            }
            imageFiles.Sort();

            if (imageFiles.Count > 0) image1.Source = new BitmapImage(new System.Uri(imageFiles.ElementAtOrDefault(0)));
            if (imageFiles.Count > 1) image2.Source = new BitmapImage(new System.Uri(imageFiles.ElementAtOrDefault(1)));
            if (imageFiles.Count > 2) image3.Source = new BitmapImage(new System.Uri(imageFiles.ElementAtOrDefault(2)));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResetButtonStyle(smallButton);
            ResetButtonStyle(middleButton);
            ResetButtonStyle(largeButton);

            if (sender is Button clickedButton)
            {
                ApplySelectedStyle(clickedButton);

                switch (clickedButton.Name)
                {
                    case "smallButton":
                        priceBlock.Text = $"가격: {houseInfo.PriceS}원";
                        break;
                    case "middleButton":
                        priceBlock.Text = $"가격: {houseInfo.PriceM}원";
                        break;
                    case "largeButton":
                        priceBlock.Text = $"가격: {houseInfo.PriceL}원";
                        break;
                }
            }
        }

        private void UnitList(object sender , RoutedEventArgs e)
        {
            List<UnitDTO> unitList = new List<UnitDTO>();

        }

        private void ResetButtonStyle(Button button)
        {
            button.Background = Brushes.White;
            button.Foreground = Brushes.Black;
            button.BorderBrush = Brushes.Gray;
        }

        private void ApplySelectedStyle(Button button)
        {
            button.Background = Brushes.LightGreen;
            button.Foreground = Brushes.White;
            button.BorderBrush = Brushes.Green;
        }
    }
}

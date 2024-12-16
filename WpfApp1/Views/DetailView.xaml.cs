using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfApp1.DTO;
using WpfApp1.Models;

namespace WpfApp1.Views
{
    public partial class DetailView : Window
    {
        private HouseDTO houseInfo;
        private List<UnitDTO> unitList;

        public DetailView(HouseDTO houseDetail, List<UnitDTO> units)
        {
            InitializeComponent();
            houseInfo = houseDetail;

            unitList = units;
            UnitDataList.ItemsSource = unitList;

            DataContext = houseInfo;

            LoadImages(houseInfo.Province);
        }

        // 이미지 로드
        private void LoadImages(string province)
        {
            string folderPath = GetResourceFolderPath();

            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show($"이미지 경로를 찾을 수 없습니다: {folderPath}");
                return;
            }

            List<string> imageFiles = GetImageFiles(folderPath, province);
            SetImageSources(imageFiles);
        }

        private string GetResourceFolderPath()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(basePath, @"..\..\..\WpfApp1\Resources");
        }

        private List<string> GetImageFiles(string folderPath, string province)
        {
            List<string> filteredFiles = new List<string>();
            string[] allFiles = Directory.GetFiles(folderPath);

            foreach (string file in allFiles)
            {
                string fileName = Path.GetFileName(file);
                if (fileName.StartsWith(province) &&
                    (fileName.Contains("-1") || fileName.Contains("-2") || fileName.Contains("-3")))
                {
                    filteredFiles.Add(file);
                }
            }

            filteredFiles.Sort();
            return filteredFiles;
        }

        private void SetImageSources(List<string> imageFiles)
        {
            if (imageFiles.Count > 0) image1.Source = new BitmapImage(new Uri(imageFiles[0]));
            if (imageFiles.Count > 1) image2.Source = new BitmapImage(new Uri(imageFiles[1]));
            if (imageFiles.Count > 2) image3.Source = new BitmapImage(new Uri(imageFiles[2]));
        }

        // 버튼 클릭 이벤트
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResetButtonStyles();

            if (sender is Button clickedButton)
            {
                ApplySelectedStyle(clickedButton);
                UpdatePriceBlock(clickedButton.Name);
            }
        }

        private void ResetButtonStyles()
        {
            ResetButtonStyle(smallButton);
            ResetButtonStyle(middleButton);
            ResetButtonStyle(largeButton);
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

        private void UpdatePriceBlock(string buttonName)
        {
            switch (buttonName)
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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // 뒤로가기 버튼 클릭 시 창 닫기
            this.Hide(); // 창 숨기기

            MainView mainView = new MainView();
            Application.Current.MainWindow = mainView;
            mainView.Show();

            this.Close(); // 완전히 닫기
        }

        private void MyPageButton_Click(object sender, RoutedEventArgs e)
        {
            // 뒤로가기 버튼 클릭 시 창 닫기
            this.Hide(); // 창 숨기기
            // 마이페이지 버튼 클릭 시 마이페이지 창 열기
            int loginUserId = TokenSave.GetUserId();
            Console.WriteLine(loginUserId);
            MyView myView = new MyView(loginUserId);
            myView.Show();

            this.Close(); // 완전히 닫기
        }

    }
}

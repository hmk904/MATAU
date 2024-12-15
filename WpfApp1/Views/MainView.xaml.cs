using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.DTO;
using WpfApp1.Models;

namespace WpfApp1.Views
{
    public partial class MainView : Window
    {
        public ObservableCollection<CardDTO> Cards { get; set; } // UI에 표시할 데이터 리스트
        private readonly HouseApi houseApi; // HouseApi 인스턴스

        private bool isDataLoaded = false; // 데이터 중복 로드 방지 플래그

        public MainView()
        {
            InitializeComponent();

            // HouseApi 초기화
            houseApi = new HouseApi();

            // 카드 리스트 초기화
            Cards = new ObservableCollection<CardDTO>();
            this.DataContext = this;

            // 데이터 로드
            LoadData();
            
        }

        private async void LoadData()
        {

            if (isDataLoaded)
            {
                return;
            }

            isDataLoaded = true;
            Cards.Clear(); // 기존 데이터를 제거하여 중복 추가 방지

            try
            {
                var houseData = await houseApi.GetAllHousesAsync();

                var imagePaths = GetImagePaths();

                // 상위 15개 항목만 처리
                var limitedHouseData = houseData.Take(16).ToList();

                for (int i = 0; i < limitedHouseData.Count; i++)
                {
                    var house = limitedHouseData[i];
                    var imagePath = i < imagePaths.Count ? imagePaths[i] : string.Empty;

                    // 이미지 경로가 비어 있으면 추가하지 않음
                    if (string.IsNullOrEmpty(imagePath))
                    {
                        continue;
                    }

                    var card = new CardDTO
                    {
                        Province = house.Province,
                        ImagePath = imagePath
                    };

                    Cards.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"데이터 로드 실패: {ex.Message}");
            }

        }



        // Resources 폴더의 이미지 경로 리스트 가져오기
        private List<string> GetImagePaths()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            // 두 단계 상위로 이동 후 정확한 폴더 접근
            string folderPath = Path.GetFullPath(Path.Combine(basePath, @"..\..\..\WpfApp1\Resources"));

            var files = Directory.GetFiles(folderPath, "*-1.*").OrderBy(f => f).ToList();

            return files;
        }

        private void MyPageButton_Click(object sender, RoutedEventArgs e)
        {
            int loginUserId = TokenSave.GetUserId();
            // 마이페이지 View 열기
            var myPage = new MyView(loginUserId);
            myPage.Show();
        }

    }

}

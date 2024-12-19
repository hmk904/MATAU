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
        private readonly UnitApi unitApi;
        private readonly UserApi userApi;
        private bool isDataLoaded = false; // 데이터 중복 로드 방지 플래그

        public MainView()
        {
            InitializeComponent();

            // HouseApi 초기화
            houseApi = new HouseApi();

            // UnitApi
            unitApi = new UnitApi();

            // UserApi
            userApi = new UserApi();

            // 카드 리스트 초기화
            Cards = new ObservableCollection<CardDTO>();
            this.DataContext = this;

            // 데이터 로드
            LoadData();

            LoadNickname();
        
        }

        private async void LoadNickname()
        {
            try
            {
                // 로그인한 사용자 ID 가져오기
                int userId = TokenSave.GetUserId();

                // UserApi를 통해 사용자 정보 요청
                var userInfo = await userApi.GetUserInfoAsync(userId);

                // 닉네임 설정
                if (userInfo != null && !string.IsNullOrEmpty(userInfo.Nickname))
                {
                    NicknameTextBlock.Text = $"{userInfo.Nickname} 님";
                }
                else
                {
                    NicknameTextBlock.Text = "게스트 님"; // 기본값
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"닉네임을 가져오는 데 실패했습니다: {ex.Message}");
                NicknameTextBlock.Text = "게스트 님"; // 기본값
            }
        }

        private async void LoadData()
        {
            if (isDataLoaded)
                return;

            isDataLoaded = true;
            Cards.Clear(); // 기존 데이터를 제거하여 중복 추가 방지

            try
            {
                // 서버에서 HouseDTO 리스트 가져오기
                var houseData = await houseApi.GetAllHousesAsync();

                // 상위 16개 항목만 처리
                var limitedHouseData = houseData.Take(16).ToList();

                for (int i = 0; i < limitedHouseData.Count; i++)
                {
                    var house = limitedHouseData[i];

                    // Province에 맞는 -1 이미지 가져오기
                    var imagePaths = GetImagePaths(house.Province);

                    // 이미지가 없으면 스킵
                    if (imagePaths.Count == 0)
                        continue;

                    var card = new CardDTO
                    {
                        Id = house.Id,            // HouseDTO의 ID 설정
                        Province = house.Province,
                        ImagePath = imagePaths[0] // 첫 번째 이미지 경로만 사용
                    };

                    // 카드 리스트에 추가
                    Cards.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"데이터 로드 실패: {ex.Message}");
            }
        }

        // Resources 폴더의 이미지 경로 리스트 가져오기
        private List<string> GetImagePaths(string province)
        {
            // 1. 기본 경로 설정
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string folderPath = Path.GetFullPath(Path.Combine(basePath, @"..\..\..\WpfApp1\Resources"));

            // 2. 폴더 유효성 검사
            if (!Directory.Exists(folderPath))
            {
                MessageBox.Show($"Resources 폴더를 찾을 수 없습니다: {folderPath}");
                return new List<string>();
            }

            // 3. 파일 필터링
            string[] allFiles = Directory.GetFiles(folderPath);
            List<string> filteredFiles = new List<string>();

            foreach (string file in allFiles)
            {
                string fileName = Path.GetFileName(file);

                // 파일명이 province와 -1 패턴을 포함하는지 확인
                if (fileName.StartsWith(province) && fileName.Contains("-1"))
                {
                    filteredFiles.Add(file);
                }
            }

            // 4. 파일 정렬
            filteredFiles.Sort();

            return filteredFiles;
        }

        private async void Card_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement frameworkElement && frameworkElement.DataContext is CardDTO card)
            {
                Console.WriteLine($"클릭된 카드 ID: {card.Id}");

                try
                {
                    var detailApi = new DetailApi();
                    var houseDetail = await detailApi.GetHouseDetailAsync(card.Id);

                    // UnitDTO에서 HouseId와 일치하는 데이터 가져오기
                    List<UnitDTO> unitData = await unitApi.GetUnitDataAsync(card.Id); // UnitDTO 데이터를 가져오는 메서드



                    foreach (var unit in unitData)
                    {
                        Console.WriteLine($"ID: {unit.Id}, HouseId: {unit.HouseId}, Size: {unit.Size}, Price: {unit.Price}");
                    }
                    // DetailView에 houseDetail 전달
                    var detailView = new DetailView(houseDetail, unitData);
                    detailView.Show();

                    this.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"상세 정보를 가져오는 데 실패했습니다: {ex.Message}");
                }
            }
        }

        // 마이페이지 가기
        private void MyPageButton_Click(object sender, RoutedEventArgs e)
        {
            int loginUserId = TokenSave.GetUserId();
            var myPage = new MyView(loginUserId);
            myPage.Show();

            this.Close();
        }
    }
}

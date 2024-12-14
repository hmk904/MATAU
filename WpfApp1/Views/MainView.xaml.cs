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
            Console.WriteLine("LoadData 호 출~!~!~!~!~!~!~!~!~!");

            if (isDataLoaded)
            {
                Console.WriteLine("LoadData() 이미 실행됨, 호출 종료");
                return;
            }

            isDataLoaded = true;
            Cards.Clear(); // 기존 데이터를 제거하여 중복 추가 방지

            try
            {
                var houseData = await houseApi.GetAllHousesAsync();
                Console.WriteLine($"HouseApi 데이터 로드 완료, 총 {houseData.Count}개 항목");

                var imagePaths = GetImagePaths();
                Console.WriteLine($"이미지 경로 로드 완료, 총 {imagePaths.Count}개 파일");

                // 상위 15개 항목만 처리
                var limitedHouseData = houseData.Take(16).ToList();

                for (int i = 0; i < limitedHouseData.Count; i++)
                {
                    var house = limitedHouseData[i];
                    var imagePath = i < imagePaths.Count ? imagePaths[i] : string.Empty;

                    // 이미지 경로가 비어 있으면 추가하지 않음
                    if (string.IsNullOrEmpty(imagePath))
                    {
                        Console.WriteLine($"Card Skipped: Province = {house.Province}, ImagePath is empty");
                        continue;
                    }

                    var card = new CardDTO
                    {
                        Province = house.Province,
                        ImagePath = imagePath
                    };

                    Console.WriteLine($"Card Added: Province = {card.Province}, ImagePath = {card.ImagePath}");
                    Cards.Add(card);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"데이터 로드 중 오류 발생: {ex.Message}");
                MessageBox.Show($"데이터 로드 실패: {ex.Message}");
            }

            Console.WriteLine("LoadData() 호출 종료 !~!~!~!~!~!~!~!~");
        }



        // Resources 폴더의 이미지 경로 리스트 가져오기
        private List<string> GetImagePaths()
        {
            string folderPath = Path.Combine("C:\\Users\\user\\source\\repos\\hmk904\\MATAU\\WpfApp1", "Resources");
            var files = Directory.GetFiles(folderPath, "*-1.*").OrderBy(f => f).ToList();

            Console.WriteLine($"GetImagePaths(): 총 {files.Count}개 파일 로드");

            foreach (var file in files)
            {
                Console.WriteLine($"파일 경로: {file}");
            }


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

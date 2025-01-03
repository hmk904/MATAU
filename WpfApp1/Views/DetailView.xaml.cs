﻿using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            // 중복 제거
            unitList = units.ToList(); // 그대로 리스트 사용
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
            //string basePath = AppDomain.CurrentDomain.BaseDirectory;
            //return Path.Combine(basePath, @"..\..\..\WpfApp1\Resources");

            // 실행 파일 기준 Resources 폴더 경로 설정
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string resourceFolderPath = Path.Combine(basePath, "Resources");

            return resourceFolderPath;
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

        private void FilterUnitsBySize(string size)
        {
            // 선택된 사이즈에 따라 리스트 필터링
            var filteredUnits = unitList.Where(unit => unit.Size == size).ToList();

            // 필터링된 데이터를 데이터그리드에 적용
            UnitDataList.ItemsSource = filteredUnits;
        }

        // 버튼 클릭 이벤트
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ResetButtonStyles();

            if (sender is Button clickedButton)
            {
                ApplySelectedStyle(clickedButton);
                // 버튼 이름에 따라 필터링 적용
                if (clickedButton == smallButton)
                    FilterUnitsBySize("S");
                else if (clickedButton == middleButton)
                    FilterUnitsBySize("M");
                else if (clickedButton == largeButton)
                    FilterUnitsBySize("L");
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

        // 전체 선택 체크박스 상태 처리
        private void HeaderCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox headerCheckBox)
            {
                bool isChecked = headerCheckBox.IsChecked ?? false;

                foreach (var unit in unitList)
                {
                    // Status가 InUse인 항목은 선택 상태를 변경하지 않음
                    if (unit.Status != "InUse")
                    {
                        unit.IsSelected = isChecked;
                    }
                }

                UnitDataList.Items.Refresh(); // UI 새로고침
                UpdatePriceBlock();           // 가격 업데이트
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // 개별 체크박스 변경 시 가격 업데이트
            if (!_isUpdating)
            {
                UpdatePriceBlock();
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (startDatePicker.SelectedDate.HasValue && endDatePicker.SelectedDate.HasValue)
            {
                DateTime startDate = startDatePicker.SelectedDate.Value;
                DateTime endDate = endDatePicker.SelectedDate.Value;

                Console.WriteLine($"StartDate: {startDate}, EndDate: {endDate}");

                if (endDate >= startDate)
                {
                    int days = (endDate - startDate).Days + 1;
                    DurationDays.Text = $"기간 : {days}일";
                }
                else
                {
                    DurationDays.Text = "종료일은 시작일 이후여야 합니다.";
                }
            }
            else
            {
                Console.WriteLine("StartDate 또는 EndDate가 null입니다.");
                DurationDays.Text = "기간 : 0일";
            }

            UpdatePriceBlock();
        }



        // 기간 계산 메서드
        private int CalculateDurationDays(DateTime? startDate, DateTime? endDate)
        {
            if (startDate.HasValue && endDate.HasValue)
            {
                int days = (endDate.Value - startDate.Value).Days + 1; // 동일한 날짜도 1일로 간주
                return Math.Max(1, days);
            }
            return 1; // 기본값 1일
        }


        private bool _isUpdating = false;

        private void UpdatePriceBlock()
        {
            if (_isUpdating) return; // 중복 호출 방지
            _isUpdating = true;

            try
            {
                // 체크된 항목 필터링
                var selectedUnits = unitList.Where(unit => unit.IsSelected).ToList();

                if (!selectedUnits.Any())
                {
                    priceBlock.Text = "총 가격: 0원";
                    return;
                }

                // 기간 계산
                int duration = CalculateDurationDays(startDatePicker.SelectedDate, endDatePicker.SelectedDate);

                // 체크된 항목들의 총 가격 계산
                int totalPrice = selectedUnits
                    .Sum(unit => Pricing.CalculateBookingCharge(unit.Price, duration));

                // 결과 표시
                priceBlock.Text = $"총 가격: {totalPrice:N0}원";

            }
            finally
            {
                _isUpdating = false;
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

        private async void BookUnitReqButton_Click(object sender, RoutedEventArgs e)
        {
            // 체크된 항목 필터링
            var selectedUnits = unitList.Where(unit => unit.IsSelected).ToList();

            if (!selectedUnits.Any())
            {
                MessageBox.Show("예약할 항목을 선택해주세요.", "알림", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                BookingsApi bookingsApi = new BookingsApi();

                foreach (var unit in selectedUnits)
                {
                    int durationDays = CalculateDurationDays(unit.StartDate, unit.EndDate);

                    // DatePicker의 값을 가져와 unit에 업데이트
                    unit.StartDate = startDatePicker.SelectedDate ?? DateTime.Now;
                    unit.EndDate = (endDatePicker.SelectedDate ?? unit.StartDate.Value).AddDays(0);

                    Console.WriteLine($"Unit StartDate: {unit.StartDate}, Unit EndDate: {unit.EndDate}");

                    var bookingRequest = new BookUnitReqDTO
                    {
                        UserId = TokenSave.GetUserId(), // 로그인된 사용자 ID
                        HouseId = unit.HouseId,
                        UnitSize = unit.Size,
                        StartDate = unit.StartDate ?? DateTime.Now, // 시작일이 없으면 현재 날짜
                        DurationDays = CalculateDurationDays(unit.StartDate, unit.EndDate),
                        UserNote = userNote.Text // 기본 메모
                    };

                    Console.WriteLine($"StartDate: {bookingRequest.StartDate},  DurationDays: {bookingRequest.DurationDays}");


                    // API 호출
                    var response = await bookingsApi.BookUnitReq(bookingRequest);
                    Console.WriteLine($"서버 응답: {response}");
                }

                // 예약 성공 시 모달 창 열기
                await ReservationDialogHost.ShowDialog(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"예약 중 오류 발생: {ex.Message}", "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UserNote_GotFocus(object sender, RoutedEventArgs e)
        {
            if (userNote.Text == "요청사항 적어주세요")
            {
                userNote.Text = "";
                userNote.Foreground = Brushes.Black; // 입력 시 텍스트 색상 변경
            }
        }

        private void UserNote_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(userNote.Text))
            {
                userNote.Text = "요청사항 적어주세요";
                userNote.Foreground = Brushes.LightGray; // 다시 Placeholder 색상 적용
            }
        }

        private void CloseDialog_Click(object sender, RoutedEventArgs e)
        {
            // 모달 창 닫기
            DialogHost.CloseDialogCommand.Execute(null, ReservationDialogHost);
        }




    }
}

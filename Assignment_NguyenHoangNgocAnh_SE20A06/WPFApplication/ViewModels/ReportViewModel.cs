using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using BusinessLogicLayer;
using BusinessObjects;

namespace WPFApplication.ViewModels
{
    public class ReportViewModel : ViewModelBase
    {
        private readonly IBookingService _bookingService;
        private DateTime _startDate = DateTime.Now.AddMonths(-1);
        private DateTime _endDate = DateTime.Now;
        private string _errorMessage = string.Empty;
        private ObservableCollection<BookingReservation> _bookings = new();

        public DateTime StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ObservableCollection<BookingReservation> Bookings
        {
            get => _bookings;
            set { _bookings = value; OnPropertyChanged(); }
        }

        public ICommand GenerateReportCommand { get; }

        public ReportViewModel()
        {
            _bookingService = new BookingService();
            GenerateReportCommand = new RelayCommand(ExecuteGenerateReport);
            
            // Load initial report
            ExecuteGenerateReport(null);
        }

        private void ExecuteGenerateReport(object? parameter)
        {
            try
            {
                ErrorMessage = string.Empty;

                if (StartDate > EndDate)
                {
                    ErrorMessage = "Start Date cannot be later than End Date.";
                    Bookings.Clear();
                    return;
                }

                var report = _bookingService.GetStatisticsReport(StartDate, EndDate);
                Bookings = new ObservableCollection<BookingReservation>(report);
                
                if (Bookings.Count == 0)
                {
                    ErrorMessage = "No booking records found for the selected date range.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to generate report: {ex.Message}";
            }
        }
    }
}

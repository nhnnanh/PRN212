using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BusinessLogicLayer;
using BusinessObjects;

namespace WPFApplication.ViewModels
{
    public class CustomerViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;
        private readonly IBookingService _bookingService;
        private readonly IRoomService _roomService;
        private readonly Customer _currentCustomer;

        private string _fullName = string.Empty;
        private string _telephone = string.Empty;
        private string _email = string.Empty;
        private DateTime? _birthday;
        private string _password = string.Empty;
        private string _successMessage = string.Empty;
        private string _errorMessage = string.Empty;

        private ObservableCollection<BookingReservation> _bookingHistory = new();

        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(); }
        }

        public string Telephone
        {
            get => _telephone;
            set { _telephone = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public DateTime? Birthday
        {
            get => _birthday;
            set { _birthday = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string SuccessMessage
        {
            get => _successMessage;
            set { _successMessage = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ObservableCollection<BookingReservation> BookingHistory
        {
            get => _bookingHistory;
            set { _bookingHistory = value; OnPropertyChanged(); }
        }

        private ObservableCollection<RoomInformation> _availableRooms = new();
        private RoomInformation? _selectedRoom;
        private DateTime _bookingStartDate = DateTime.Now;
        private DateTime _bookingEndDate = DateTime.Now.AddDays(1);
        private string _searchRoomText = string.Empty;
        private string _bookingFeedback = string.Empty;
        private string _bookingError = string.Empty;

        public ObservableCollection<RoomInformation> AvailableRooms
        {
            get => _availableRooms;
            set { _availableRooms = value; OnPropertyChanged(); }
        }

        public RoomInformation? SelectedRoom
        {
            get => _selectedRoom;
            set { _selectedRoom = value; OnPropertyChanged(); }
        }

        public DateTime BookingStartDate
        {
            get => _bookingStartDate;
            set { _bookingStartDate = value; OnPropertyChanged(); }
        }

        public DateTime BookingEndDate
        {
            get => _bookingEndDate;
            set { _bookingEndDate = value; OnPropertyChanged(); }
        }

        public string SearchRoomText
        {
            get => _searchRoomText;
            set
            {
                _searchRoomText = value;
                OnPropertyChanged();
                LoadAvailableRooms();
            }
        }

        public string BookingFeedback
        {
            get => _bookingFeedback;
            set { _bookingFeedback = value; OnPropertyChanged(); }
        }

        public string BookingError
        {
            get => _bookingError;
            set { _bookingError = value; OnPropertyChanged(); }
        }

        public ICommand UpdateProfileCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand BookRoomCommand { get; }

        public CustomerViewModel(Customer customer)
        {
            _customerService = new CustomerService();
            _bookingService = new BookingService();
            _roomService = new RoomService();
            _currentCustomer = customer;

            // Load properties
            FullName = customer.CustomerFullName ?? string.Empty;
            Telephone = customer.Telephone ?? string.Empty;
            Email = customer.EmailAddress;
            Birthday = customer.CustomerBirthday.HasValue ? customer.CustomerBirthday.Value.ToDateTime(TimeOnly.MinValue) : null;
            Password = customer.Password ?? string.Empty;

            UpdateProfileCommand = new RelayCommand(ExecuteUpdateProfile);
            LogoutCommand = new RelayCommand(ExecuteLogout);
            BookRoomCommand = new RelayCommand(ExecuteBookRoom, CanBookRoom);
 
            LoadBookingHistory();
            LoadAvailableRooms();
        }

        private void LoadBookingHistory()
        {
            try
            {
                var history = _bookingService.GetBookingsByCustomerId(_currentCustomer.CustomerId);
                BookingHistory = new ObservableCollection<BookingReservation>(history);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load booking history: {ex.Message}";
            }
        }

        private void LoadAvailableRooms()
        {
            try
            {
                var activeRooms = _roomService.GetAllRooms().Where(r => r.RoomStatus == 1).ToList();
                if (!string.IsNullOrWhiteSpace(SearchRoomText))
                {
                    activeRooms = activeRooms
                        .Where(r => r.RoomNumber != null && r.RoomNumber.Contains(SearchRoomText, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                AvailableRooms = new ObservableCollection<RoomInformation>(activeRooms);
            }
            catch (Exception ex)
            {
                BookingError = $"Failed to load rooms: {ex.Message}";
            }
        }

        private bool CanBookRoom(object? parameter) => SelectedRoom != null;

        private void ExecuteBookRoom(object? parameter)
        {
            try
            {
                BookingError = string.Empty;
                BookingFeedback = string.Empty;

                if (SelectedRoom == null)
                {
                    BookingError = "Please select a room to book.";
                    return;
                }

                if (BookingStartDate >= BookingEndDate)
                {
                    BookingError = "Start date must be earlier than End date.";
                    return;
                }

                if (BookingStartDate < DateTime.Today)
                {
                    BookingError = "Start date cannot be in the past.";
                    return;
                }

                int days = (BookingEndDate - BookingStartDate).Days;
                decimal roomPrice = SelectedRoom.RoomPricePerDay ?? 0;
                decimal totalPrice = roomPrice * days;

                var confirm = MessageBox.Show($"Confirm booking Room {SelectedRoom.RoomNumber} from {BookingStartDate:yyyy-MM-dd} to {BookingEndDate:yyyy-MM-dd}?\nTotal Price: {totalPrice:C}", 
                                              "Confirm Reservation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Yes)
                {
                    // Create Booking
                    var booking = new BookingReservation
                    {
                        CustomerId = _currentCustomer.CustomerId,
                        BookingDate = DateOnly.FromDateTime(DateTime.Today),
                        TotalPrice = totalPrice,
                        BookingStatus = 1 // Active
                    };

                    bool success = _bookingService.AddBooking(booking);
                    if (success)
                    {
                        var detail = new BookingDetail
                        {
                            BookingReservationId = booking.BookingReservationId,
                            RoomId = SelectedRoom.RoomId,
                            StartDate = DateOnly.FromDateTime(BookingStartDate),
                            EndDate = DateOnly.FromDateTime(BookingEndDate),
                            ActualPrice = totalPrice
                        };

                        _bookingService.AddBookingDetail(detail);

                        BookingFeedback = "Booking completed successfully!";
                        LoadBookingHistory();
                        LoadAvailableRooms();
                    }
                    else
                    {
                        BookingError = "Failed to place reservation.";
                    }
                }
            }
            catch (Exception ex)
            {
                BookingError = $"Reservation failed: {ex.Message}";
            }
        }

        private void ExecuteUpdateProfile(object? parameter)
        {
            try
            {
                SuccessMessage = string.Empty;
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(FullName))
                {
                    ErrorMessage = "Full Name cannot be empty.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Telephone))
                {
                    ErrorMessage = "Telephone cannot be empty.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Password cannot be empty.";
                    return;
                }

                // Update entity properties
                _currentCustomer.CustomerFullName = FullName;
                _currentCustomer.Telephone = Telephone;
                _currentCustomer.CustomerBirthday = Birthday.HasValue ? DateOnly.FromDateTime(Birthday.Value) : null;
                _currentCustomer.Password = Password;

                bool success = _customerService.UpdateCustomer(_currentCustomer);
                if (success)
                {
                    SuccessMessage = "Profile updated successfully!";
                }
                else
                {
                    ErrorMessage = "Failed to update profile details.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error updating profile: {ex.Message}";
            }
        }

        private void ExecuteLogout(object? parameter)
        {
            var currentWindow = parameter as Window;
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            currentWindow?.Close();
        }
    }
}

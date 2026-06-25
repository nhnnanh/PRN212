using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BusinessLogicLayer;
using BusinessObjects;

namespace WPFApplication.ViewModels
{
    public class AdminViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;
        private readonly IRoomService _roomService;
        private readonly IBookingService _bookingService;

        private ObservableCollection<Customer> _customers = new();
        private Customer? _selectedCustomer;

        private ObservableCollection<RoomInformation> _rooms = new();
        private RoomInformation? _selectedRoom;

        private ObservableCollection<BookingReservation> _bookings = new();
        private BookingReservation? _selectedBooking;

        private string _searchCustomerText = string.Empty;
        private string _searchRoomText = string.Empty;

        public string SearchCustomerText
        {
            get => _searchCustomerText;
            set
            {
                _searchCustomerText = value;
                OnPropertyChanged();
                FilterCustomers();
            }
        }

        public string SearchRoomText
        {
            get => _searchRoomText;
            set
            {
                _searchRoomText = value;
                OnPropertyChanged();
                FilterRooms();
            }
        }

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set
            {
                _customers = value;
                OnPropertyChanged();
            }
        }

        public Customer? SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RoomInformation> Rooms
        {
            get => _rooms;
            set
            {
                _rooms = value;
                OnPropertyChanged();
            }
        }

        public RoomInformation? SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<BookingReservation> Bookings
        {
            get => _bookings;
            set
            {
                _bookings = value;
                OnPropertyChanged();
            }
        }

        public BookingReservation? SelectedBooking
        {
            get => _selectedBooking;
            set
            {
                _selectedBooking = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand AddCustomerCommand { get; }
        public ICommand EditCustomerCommand { get; }
        public ICommand DeleteCustomerCommand { get; }

        public ICommand AddRoomCommand { get; }
        public ICommand EditRoomCommand { get; }
        public ICommand DeleteRoomCommand { get; }

        public ICommand OpenReportCommand { get; }
        public ICommand LogoutCommand { get; }

        public ICommand AddBookingCommand { get; }
        public ICommand EditBookingCommand { get; }
        public ICommand DeleteBookingCommand { get; }

        public AdminViewModel()
        {
            _customerService = new CustomerService();
            _roomService = new RoomService();
            _bookingService = new BookingService();

            AddCustomerCommand = new RelayCommand(ExecuteAddCustomer);
            EditCustomerCommand = new RelayCommand(ExecuteEditCustomer, CanEditOrDeleteCustomer);
            DeleteCustomerCommand = new RelayCommand(ExecuteDeleteCustomer, CanEditOrDeleteCustomer);

            AddRoomCommand = new RelayCommand(ExecuteAddRoom);
            EditRoomCommand = new RelayCommand(ExecuteEditRoom, CanEditOrDeleteRoom);
            DeleteRoomCommand = new RelayCommand(ExecuteDeleteRoom, CanEditOrDeleteRoom);

            OpenReportCommand = new RelayCommand(ExecuteOpenReport);
            LogoutCommand = new RelayCommand(ExecuteLogout);

            AddBookingCommand = new RelayCommand(ExecuteAddBooking);
            EditBookingCommand = new RelayCommand(ExecuteEditBooking, CanEditOrDeleteBooking);
            DeleteBookingCommand = new RelayCommand(ExecuteDeleteBooking, CanEditOrDeleteBooking);

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                FilterCustomers();
                FilterRooms();
                Bookings = new ObservableCollection<BookingReservation>(_bookingService.GetAllBookings());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterCustomers()
        {
            try
            {
                var all = _customerService.GetAllCustomers();
                if (string.IsNullOrWhiteSpace(SearchCustomerText))
                {
                    Customers = new ObservableCollection<Customer>(all);
                }
                else
                {
                    var filtered = all.Where(c => c.CustomerFullName != null && c.CustomerFullName.Contains(SearchCustomerText, StringComparison.OrdinalIgnoreCase)).ToList();
                    Customers = new ObservableCollection<Customer>(filtered);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching customers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterRooms()
        {
            try
            {
                var all = _roomService.GetAllRooms();
                if (string.IsNullOrWhiteSpace(SearchRoomText))
                {
                    Rooms = new ObservableCollection<RoomInformation>(all);
                }
                else
                {
                    var filtered = all.Where(r => r.RoomNumber != null && r.RoomNumber.Contains(SearchRoomText, StringComparison.OrdinalIgnoreCase)).ToList();
                    Rooms = new ObservableCollection<RoomInformation>(filtered);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching rooms: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Customer CRUD Operations
        private void ExecuteAddCustomer(object? parameter)
        {
            var dialog = new CustomerDialog(null);
            if (dialog.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ExecuteEditCustomer(object? parameter)
        {
            if (SelectedCustomer == null) return;
            var dialog = new CustomerDialog(SelectedCustomer);
            if (dialog.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private bool CanEditOrDeleteCustomer(object? parameter) => SelectedCustomer != null;

        private void ExecuteDeleteCustomer(object? parameter)
        {
            if (SelectedCustomer == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete/disable Customer '{SelectedCustomer.CustomerFullName}'?",
                                         "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    bool deleted = _customerService.DeleteCustomer(SelectedCustomer.CustomerId);
                    if (deleted)
                    {
                        MessageBox.Show("Customer processed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete customer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Room CRUD Operations
        private void ExecuteAddRoom(object? parameter)
        {
            var dialog = new RoomDialog(null);
            if (dialog.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ExecuteEditRoom(object? parameter)
        {
            if (SelectedRoom == null) return;
            var dialog = new RoomDialog(SelectedRoom);
            if (dialog.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private bool CanEditOrDeleteRoom(object? parameter) => SelectedRoom != null;

        private void ExecuteDeleteRoom(object? parameter)
        {
            if (SelectedRoom == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete/disable Room '{SelectedRoom.RoomNumber}'?",
                                         "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    bool success = _roomService.DeleteRoom(SelectedRoom.RoomId);
                    if (success)
                    {
                        MessageBox.Show("Room deletion/disabling completed.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete room.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Booking CRUD Operations
        private void ExecuteAddBooking(object? parameter)
        {
            var dialog = new BookingDialog(null);
            if (dialog.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private void ExecuteEditBooking(object? parameter)
        {
            if (SelectedBooking == null) return;
            var dialog = new BookingDialog(SelectedBooking);
            if (dialog.ShowDialog() == true)
            {
                LoadData();
            }
        }

        private bool CanEditOrDeleteBooking(object? parameter) => SelectedBooking != null;

        private void ExecuteDeleteBooking(object? parameter)
        {
            if (SelectedBooking == null) return;

            var result = MessageBox.Show($"Are you sure you want to delete Booking ID '{SelectedBooking.BookingReservationId}'?",
                                         "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    bool deleted = _bookingService.DeleteBooking(SelectedBooking.BookingReservationId);
                    if (deleted)
                    {
                        MessageBox.Show("Booking deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete booking.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Report & Navigation Operations
        private void ExecuteOpenReport(object? parameter)
        {
            var reportWindow = new ReportWindow();
            reportWindow.ShowDialog();
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

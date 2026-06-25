using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using BusinessLogicLayer;
using BusinessObjects;

namespace WPFApplication.ViewModels
{
    public class BookingDialogViewModel : ViewModelBase
    {
        private readonly IBookingService _bookingService;
        private readonly ICustomerService _customerService;
        private readonly IRoomService _roomService;
        private readonly BookingReservation? _originalBooking;

        private string _title = "Add Booking";
        private int _selectedCustomerId;
        private int _selectedRoomId;
        private DateTime _bookingDate = DateTime.Now;
        private DateTime _startDate = DateTime.Now;
        private DateTime _endDate = DateTime.Now.AddDays(1);
        private int _statusIndex = 0; // 0 for Active, 1 for Canceled
        private string _errorMessage = string.Empty;

        private List<Customer> _customers = new();
        private List<RoomInformation> _rooms = new();

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public int SelectedCustomerId
        {
            get => _selectedCustomerId;
            set { _selectedCustomerId = value; OnPropertyChanged(); }
        }

        public int SelectedRoomId
        {
            get => _selectedRoomId;
            set { _selectedRoomId = value; OnPropertyChanged(); }
        }

        public DateTime BookingDate
        {
            get => _bookingDate;
            set { _bookingDate = value; OnPropertyChanged(); }
        }

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

        public int StatusIndex
        {
            get => _statusIndex;
            set { _statusIndex = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public List<Customer> Customers
        {
            get => _customers;
            set { _customers = value; OnPropertyChanged(); }
        }

        public List<RoomInformation> Rooms
        {
            get => _rooms;
            set { _rooms = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public BookingDialogViewModel(BookingReservation? booking)
        {
            _bookingService = new BookingService();
            _customerService = new CustomerService();
            _roomService = new RoomService();
            _originalBooking = booking;

            // Load Customers and Rooms
            try
            {
                Customers = _customerService.GetAllCustomers().Where(c => c.CustomerStatus == 1).ToList();
                Rooms = _roomService.GetAllRooms().Where(r => r.RoomStatus == 1).ToList();

                if (Customers.Count > 0) SelectedCustomerId = Customers[0].CustomerId;
                if (Rooms.Count > 0) SelectedRoomId = Rooms[0].RoomId;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Initialization error: {ex.Message}";
            }

            if (booking != null)
            {
                Title = "Edit Booking";
                SelectedCustomerId = booking.CustomerId;
                BookingDate = booking.BookingDate.HasValue ? booking.BookingDate.Value.ToDateTime(TimeOnly.MinValue) : DateTime.Now;
                StatusIndex = (booking.BookingStatus == 2) ? 1 : 0;

                // Load first room from details if exists
                var details = _bookingService.GetBookingDetailsByReservationId(booking.BookingReservationId);
                if (details.Count > 0)
                {
                    SelectedRoomId = details[0].RoomId;
                    StartDate = details[0].StartDate.ToDateTime(TimeOnly.MinValue);
                    EndDate = details[0].EndDate.ToDateTime(TimeOnly.MinValue);
                }
            }
            else
            {
                Title = "Add Booking";
            }

            SaveCommand = new RelayCommand(ExecuteSave);
            CancelCommand = new RelayCommand(ExecuteCancel);
        }

        private void ExecuteSave(object? parameter)
        {
            try
            {
                var window = parameter as Window;
                if (window == null) return;

                if (SelectedCustomerId <= 0)
                {
                    ErrorMessage = "Please select a customer.";
                    return;
                }

                if (SelectedRoomId <= 0)
                {
                    ErrorMessage = "Please select a room.";
                    return;
                }

                if (StartDate >= EndDate)
                {
                    ErrorMessage = "Start Date must be earlier than End Date.";
                    return;
                }

                var selectedRoom = _roomService.GetRoomById(SelectedRoomId);
                if (selectedRoom == null)
                {
                    ErrorMessage = "Invalid room selected.";
                    return;
                }

                decimal pricePerDay = selectedRoom.RoomPricePerDay ?? 0;
                int days = (EndDate - StartDate).Days;
                decimal totalPrice = pricePerDay * days;
                byte statusVal = (byte)(StatusIndex == 1 ? 2 : 1);

                if (_originalBooking == null)
                {
                    // Add Mode
                    var newBooking = new BookingReservation
                    {
                        CustomerId = SelectedCustomerId,
                        BookingDate = DateOnly.FromDateTime(BookingDate),
                        TotalPrice = totalPrice,
                        BookingStatus = statusVal
                    };

                    // Add booking first
                    bool bookingSuccess = _bookingService.AddBooking(newBooking);
                    if (bookingSuccess)
                    {
                        // Add detail
                        var detail = new BookingDetail
                        {
                            BookingReservationId = newBooking.BookingReservationId,
                            RoomId = SelectedRoomId,
                            StartDate = DateOnly.FromDateTime(StartDate),
                            EndDate = DateOnly.FromDateTime(EndDate),
                            ActualPrice = totalPrice
                        };

                        _bookingService.AddBookingDetail(detail);

                        window.DialogResult = true;
                        window.Close();
                    }
                    else
                    {
                        ErrorMessage = "Failed to create booking.";
                    }
                }
                else
                {
                    // Edit Mode
                    _originalBooking.CustomerId = SelectedCustomerId;
                    _originalBooking.BookingDate = DateOnly.FromDateTime(BookingDate);
                    _originalBooking.TotalPrice = totalPrice;
                    _originalBooking.BookingStatus = statusVal;

                    bool bookingSuccess = _bookingService.UpdateBooking(_originalBooking);
                    if (bookingSuccess)
                    {
                        // Update details: remove old details, add new
                        var oldDetails = _bookingService.GetBookingDetailsByReservationId(_originalBooking.BookingReservationId);
                        foreach (var det in oldDetails)
                        {
                            _bookingService.DeleteBookingDetail(det.BookingReservationId, det.RoomId);
                        }

                        var detail = new BookingDetail
                        {
                            BookingReservationId = _originalBooking.BookingReservationId,
                            RoomId = SelectedRoomId,
                            StartDate = DateOnly.FromDateTime(StartDate),
                            EndDate = DateOnly.FromDateTime(EndDate),
                            ActualPrice = totalPrice
                        };

                        _bookingService.AddBookingDetail(detail);

                        window.DialogResult = true;
                        window.Close();
                    }
                    else
                    {
                        ErrorMessage = "Failed to update booking.";
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Save failed: {ex.Message}";
            }
        }

        private void ExecuteCancel(object? parameter)
        {
            var window = parameter as Window;
            if (window != null)
            {
                window.DialogResult = false;
                window.Close();
            }
        }
    }
}

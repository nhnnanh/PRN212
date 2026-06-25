using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using BusinessLogicLayer;
using BusinessObjects;

namespace WPFApplication.ViewModels
{
    public class RoomDialogViewModel : ViewModelBase
    {
        private readonly IRoomService _roomService;
        private readonly RoomInformation? _originalRoom;

        private string _title = "Add Room";
        private string _roomNumber = string.Empty;
        private string _detailDescription = string.Empty;
        private string _maxCapacity = string.Empty;
        private string _pricePerDay = string.Empty;
        private int _statusIndex = 0; // 0 for Active, 1 for Inactive
        private int _selectedRoomTypeId;
        private List<RoomType> _roomTypes = new();
        private string _errorMessage = string.Empty;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public string RoomNumber
        {
            get => _roomNumber;
            set { _roomNumber = value; OnPropertyChanged(); }
        }

        public string DetailDescription
        {
            get => _detailDescription;
            set { _detailDescription = value; OnPropertyChanged(); }
        }

        public string MaxCapacity
        {
            get => _maxCapacity;
            set { _maxCapacity = value; OnPropertyChanged(); }
        }

        public string PricePerDay
        {
            get => _pricePerDay;
            set { _pricePerDay = value; OnPropertyChanged(); }
        }

        public int StatusIndex
        {
            get => _statusIndex;
            set { _statusIndex = value; OnPropertyChanged(); }
        }

        public int SelectedRoomTypeId
        {
            get => _selectedRoomTypeId;
            set { _selectedRoomTypeId = value; OnPropertyChanged(); }
        }

        public List<RoomType> RoomTypes
        {
            get => _roomTypes;
            set { _roomTypes = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public RoomDialogViewModel(RoomInformation? room)
        {
            _roomService = new RoomService();
            _originalRoom = room;

            // Load room types
            try
            {
                RoomTypes = _roomService.GetRoomTypes();
                if (RoomTypes.Count > 0)
                {
                    SelectedRoomTypeId = RoomTypes[0].RoomTypeId;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Failed to load room types: {ex.Message}";
            }

            if (room != null)
            {
                Title = "Edit Room";
                RoomNumber = room.RoomNumber;
                DetailDescription = room.RoomDetailDescription ?? string.Empty;
                MaxCapacity = room.RoomMaxCapacity?.ToString() ?? string.Empty;
                PricePerDay = room.RoomPricePerDay?.ToString("0.00") ?? string.Empty;
                byte statusVal = room.RoomStatus ?? 1;
                StatusIndex = (statusVal == 2) ? 1 : 0;
                SelectedRoomTypeId = room.RoomTypeId;
            }
            else
            {
                Title = "Add Room";
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

                if (string.IsNullOrWhiteSpace(RoomNumber))
                {
                    ErrorMessage = "Room Number is required.";
                    return;
                }

                if (!int.TryParse(MaxCapacity, out int capacity) || capacity <= 0)
                {
                    ErrorMessage = "Max Capacity must be a positive integer.";
                    return;
                }

                if (!decimal.TryParse(PricePerDay, out decimal price) || price <= 0)
                {
                    ErrorMessage = "Price per Day must be a positive decimal number.";
                    return;
                }

                if (SelectedRoomTypeId <= 0)
                {
                    ErrorMessage = "Please select a room type.";
                    return;
                }

                if (_originalRoom == null)
                {
                    byte statusVal = (byte)(StatusIndex == 1 ? 2 : 1);

                    // Add Mode
                    var newRoom = new RoomInformation
                    {
                        RoomNumber = RoomNumber,
                        RoomDetailDescription = DetailDescription,
                        RoomMaxCapacity = capacity,
                        RoomPricePerDay = price,
                        RoomStatus = statusVal,
                        RoomTypeId = SelectedRoomTypeId
                    };

                    bool success = _roomService.AddRoom(newRoom);
                    if (success)
                    {
                        window.DialogResult = true;
                        window.Close();
                    }
                    else
                    {
                        ErrorMessage = "Failed to add room.";
                    }
                }
                else
                {
                    // Edit Mode
                    byte statusVal = (byte)(StatusIndex == 1 ? 2 : 1);

                    _originalRoom.RoomNumber = RoomNumber;
                    _originalRoom.RoomDetailDescription = DetailDescription;
                    _originalRoom.RoomMaxCapacity = capacity;
                    _originalRoom.RoomPricePerDay = price;
                    _originalRoom.RoomStatus = statusVal;
                    _originalRoom.RoomTypeId = SelectedRoomTypeId;

                    bool success = _roomService.UpdateRoom(_originalRoom);
                    if (success)
                    {
                        window.DialogResult = true;
                        window.Close();
                    }
                    else
                    {
                        ErrorMessage = "Failed to update room.";
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

using System;
using System.Windows;
using System.Windows.Input;
using BusinessLogicLayer;
using BusinessObjects;

namespace WPFApplication.ViewModels
{
    public class CustomerDialogViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;
        private readonly Customer? _originalCustomer;
        
        private string _title = "Add Customer";
        private string _fullName = string.Empty;
        private string _telephone = string.Empty;
        private string _email = string.Empty;
        private DateTime? _birthday = DateTime.Now.AddYears(-20);
        private int _statusIndex = 0; // 0 for Active, 1 for Inactive
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

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

        public int StatusIndex
        {
            get => _statusIndex;
            set { _statusIndex = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public CustomerDialogViewModel(Customer? customer)
        {
            _customerService = new CustomerService();
            _originalCustomer = customer;

            if (customer != null)
            {
                Title = "Edit Customer";
                FullName = customer.CustomerFullName ?? string.Empty;
                Telephone = customer.Telephone ?? string.Empty;
                Email = customer.EmailAddress;
                Birthday = customer.CustomerBirthday.HasValue ? customer.CustomerBirthday.Value.ToDateTime(TimeOnly.MinValue) : null;
                byte statusVal = customer.CustomerStatus ?? 1;
                StatusIndex = (statusVal == 2) ? 1 : 0;
                Password = customer.Password ?? string.Empty;
            }
            else
            {
                Title = "Add Customer";
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

                if (string.IsNullOrWhiteSpace(FullName))
                {
                    ErrorMessage = "Full Name is required.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Telephone))
                {
                    ErrorMessage = "Telephone is required.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Email))
                {
                    ErrorMessage = "Email Address is required.";
                    return;
                }

                if (!Email.Contains("@") || !Email.Contains("."))
                {
                    ErrorMessage = "Please enter a valid email address.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Password is required.";
                    return;
                }

                if (_originalCustomer == null)
                {
                    // Check duplicate email
                    var existing = _customerService.GetCustomerByEmail(Email);
                    if (existing != null)
                    {
                        ErrorMessage = "Email address is already in use.";
                        return;
                    }

                    byte statusVal = (byte)(StatusIndex == 1 ? 2 : 1);

                    // Add Mode
                    var newCustomer = new Customer
                    {
                        CustomerFullName = FullName,
                        Telephone = Telephone,
                        EmailAddress = Email,
                        CustomerBirthday = Birthday.HasValue ? DateOnly.FromDateTime(Birthday.Value) : null,
                        CustomerStatus = statusVal,
                        Password = Password
                    };

                    bool success = _customerService.AddCustomer(newCustomer);
                    if (success)
                    {
                        window.DialogResult = true;
                        window.Close();
                    }
                    else
                    {
                        ErrorMessage = "Failed to add customer.";
                    }
                }
                else
                {
                    // Edit Mode
                    // If email changes, check duplicate
                    if (!Email.Equals(_originalCustomer.EmailAddress, StringComparison.OrdinalIgnoreCase))
                    {
                        var existing = _customerService.GetCustomerByEmail(Email);
                        if (existing != null)
                        {
                            ErrorMessage = "Email address is already in use.";
                            return;
                        }
                    }

                    byte statusVal = (byte)(StatusIndex == 1 ? 2 : 1);

                    _originalCustomer.CustomerFullName = FullName;
                    _originalCustomer.Telephone = Telephone;
                    _originalCustomer.EmailAddress = Email;
                    _originalCustomer.CustomerBirthday = Birthday.HasValue ? DateOnly.FromDateTime(Birthday.Value) : null;
                    _originalCustomer.CustomerStatus = statusVal;
                    _originalCustomer.Password = Password;

                    bool success = _customerService.UpdateCustomer(_originalCustomer);
                    if (success)
                    {
                        window.DialogResult = true;
                        window.Close();
                    }
                    else
                    {
                        ErrorMessage = "Failed to update customer.";
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

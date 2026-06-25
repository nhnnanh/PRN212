using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BusinessLogicLayer;
using BusinessObjects;
using Microsoft.Extensions.Configuration;

namespace WPFApplication.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ICustomerService _customerService;
        private string _email = string.Empty;
        private string _errorMessage = string.Empty;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _customerService = new CustomerService();
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private void ExecuteLogin(object? parameter)
        {
            try
            {
                var passwordBox = parameter as PasswordBox;
                if (passwordBox == null)
                {
                    ErrorMessage = "Password control binding error.";
                    return;
                }

                string password = passwordBox.Password;

                if (string.IsNullOrWhiteSpace(Email))
                {
                    ErrorMessage = "Email cannot be empty.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    ErrorMessage = "Password cannot be empty.";
                    return;
                }

                // Read appsettings.json
                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                var configuration = builder.Build();

                string adminEmail = configuration["AdminAccount:Email"] ?? "admin@FUMiniHotelSystem.com";
                string adminPassword = configuration["AdminAccount:Password"] ?? "@@abc123@@";

                Window currentWindow = Window.GetWindow(passwordBox);

                // 1. Check Admin Account
                if (Email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase) && password == adminPassword)
                {
                    ErrorMessage = string.Empty;
                    // Open Admin Window
                    AdminWindow adminWindow = new AdminWindow();
                    adminWindow.Show();
                    currentWindow?.Close();
                    return;
                }

                // 2. Check Customer Account in Database
                var customer = _customerService.Login(Email, password);
                if (customer != null)
                {
                    ErrorMessage = string.Empty;
                    // Open Customer Window (passing customer object)
                    CustomerWindow customerWindow = new CustomerWindow(customer);
                    customerWindow.Show();
                    currentWindow?.Close();
                }
                else
                {
                    ErrorMessage = "Invalid email or password, or account is disabled.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"System error: {ex.Message}";
            }
        }
    }
}

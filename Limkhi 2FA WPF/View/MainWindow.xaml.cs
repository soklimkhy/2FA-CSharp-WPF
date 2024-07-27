using System;
using System.Windows;
using System.Windows.Threading;
using OtpNet;
using Limkhi_2FA_WPF.Model;
using System.Text;

namespace Limkhi_2FA_WPF.View
{
    public partial class MainWindow : Window
    {
        


        public MainWindow()
        {

            InitializeComponent();

            CountDownTime();

        }
        
        private void CountDownTime()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tickevent;
            timer.Start();
        }
        private void tickevent(object sender, EventArgs e)
        {
            

            //CountdownText.Text = DateTime.Now.ToString($"[ Expire in {@"ss"}s ]");
            
            if (DateTime.Now.Second >= 0 && DateTime.Now.Second < 30)
            {
                CountdownText.Text = $"[ Expire in {(30 - DateTime.Now.Second)}s ]";
            }
            if (DateTime.Now.Second >= 30 && DateTime.Now.Second < 60)
            {
                CountdownText.Text = $"[ Expire in {(60 - DateTime.Now.Second)}s ]";

            }


        }

        private void generateKey(object sender, RoutedEventArgs e)
        {
            // Get the secret key from the TextBox
            string secretKeyInput = getSecretKey.Text;

            // Remove spaces from the input (if any)
            string secretKeyBase32 = secretKeyInput.Replace(" ", "");

            // Validate input: Check if the secret key is not null or empty
            if (string.IsNullOrWhiteSpace(secretKeyBase32))
            {
                MessageBox.Show("Please enter a valid secret key.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Exit the method if input is invalid
            }

            // Decode the base32 secret key to bytes
            byte[] secretKeyBytes;
            try
            {
                secretKeyBytes = Base32Encoding.ToBytes(secretKeyBase32);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error decoding secret key: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Generate a 6-digit key using TOTP
            var totp = new Totp(secretKeyBytes);
            string generatedKey = totp.ComputeTotp();

            // Display the generated key
            displayKey.Content = $"6 Numbers is: {generatedKey}";
        }








        private void copyKey(object sender, RoutedEventArgs e)
        {
            // Get the generated key from the display
            string generatedKey = displayKey.Content.ToString().Split(':')[1].Trim(); // Extract the key part

            // Copy the generated key to the clipboard
            Clipboard.SetText(generatedKey);
        }

    }
}

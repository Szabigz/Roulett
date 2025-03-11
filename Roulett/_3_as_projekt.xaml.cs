using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net.Http;
using Newtonsoft.Json;

namespace Roulett
{
    /// <summary>
    /// Interaction logic for _3_as_projekt.xaml
    /// </summary>
    /// 
    class LoginResponse
    {
        public string Token { get; set; }
        public double osszeg { get; set; }
    }

    public partial class _3_as_projekt : Window
    {
        private static readonly HttpClient client = new HttpClient();

        public _3_as_projekt()
        {
            InitializeComponent();

        }
        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;
            await LoginAsync(username, password);
        }

        private async Task LoginAsync(string username, string password)
        {
            var loginData = new { loginUsername = username, loginPassword = password };
            var json = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("http://localhost:5555/login", content);

            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<LoginResponse>(responseString);


                string token = responseData.Token;
                double playerMoney = 0;

                if (responseData != null && responseData.osszeg != null)
                {
                    double.TryParse(responseData.osszeg.ToString(), out playerMoney);
                }

                MessageBox.Show($"Kapott pénz: {playerMoney}"); // Ellenőrzés


                MessageBox.Show("Sikeres login", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow mainwindow = new MainWindow(username, playerMoney); // Pénz átadása

                mainwindow.Show();
                this.Close();

            }
            else
            {
                MessageBox.Show("Sikertelen login", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void register(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://localhost:5555");
        }

    }
}

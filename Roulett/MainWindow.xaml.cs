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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Timers;
using Newtonsoft.Json;
using System.Net.Http;

namespace Roulett
{
    public partial class MainWindow : Window
    {
        private Random _random;
        private RouletteWheel _rouletteWheel;
        private double redBet = 0;
        private double blackBet = 0;
        private double oddBet = 0;
        private double evenBet = 0;
        private double zeroBet = 0;

        public double playerMoney;
        public double houseMoney = 999999;
        private double betAmount = 0;
        public MainWindow(string username, double playerMoney)
        {
            InitializeComponent();
            this.Hide();
            _random = new Random();
            _rouletteWheel = new RouletteWheel();

            this.playerMoney = playerMoney; // Beállítjuk a kapott pénzösszeget

            usernameLabel.Text = "Username: " + username;
            PlayerMoney.Text = "Money: " + this.playerMoney.ToString();
            HouseMoney.Text = "HouseMoney: " + houseMoney.ToString();
        }




        // Kattintás: Bet on Red
        private void BetOnRedButton_Click(object sender, RoutedEventArgs e)
        {
            redBet += 100;
            betAmount += 100;
            RedBetAmount.Text = $"${redBet}";
        }

        // Kattintás: Bet on Black
        private void BetOnBlackButton_Click(object sender, RoutedEventArgs e)
        {
            blackBet += 100;
            betAmount += 100;
            BlackBetAmount.Text = $"${blackBet}";
        }

        // Kattintás: Bet on Odd
        private void BetOnOddButton_Click(object sender, RoutedEventArgs e)
        {
            oddBet += 50;
            betAmount += 50;
            OddBetAmount.Text = $"${oddBet}";
        }

        // Kattintás: Bet on Even
        private void BetOnEvenButton_Click(object sender, RoutedEventArgs e)
        {
            evenBet += 50;
            betAmount += 50;
            EvenBetAmount.Text = $"${evenBet}";
        }

        // Kattintás: Bet on Zero
        private void BetOnZeroButton_Click(object sender, RoutedEventArgs e)
        {
            zeroBet += 1000;
            betAmount += 1000;
            ZeroBetAmount.Text = $"${zeroBet}";
        }

        // Fogadások nullázása
        private void ResetBets()
        {
            betAmount = 0;
            redBet = 0;
            blackBet = 0;
            oddBet = 0;
            evenBet = 0;
            zeroBet = 0;

            RedBetAmount.Text = "0";
            BlackBetAmount.Text = "0";
            OddBetAmount.Text = "0";
            EvenBetAmount.Text = "0";
            ZeroBetAmount.Text = "0";
        }

        private void UpdatePlayerAndHouseMoney(int winningNumber)
        {
            bool playerWins = CheckIfPlayerWins(winningNumber);

            if (playerWins)
            {
                houseMoney -= betAmount;
                playerMoney += betAmount;
                MessageBox.Show($"You win! You have won ${betAmount}");
                ResetBets();
            }
            else
            {
                houseMoney += betAmount;
                playerMoney -= betAmount;
                MessageBox.Show($"You lose! You have lost ${betAmount}");
                ResetBets();
            }

            HouseMoney.Text = $"House's Money: ${houseMoney}";
            PlayerMoney.Text = $"Player Money: ${playerMoney}";
        }

        // KEREK PÖRGETÉSE
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ButtonStart.IsEnabled = false;
            z.IsEnabled = false;
            r.IsEnabled = false;
            b.IsEnabled = false;
            ee.IsEnabled = false;
            o.IsEnabled = false;

            RotateWheel();
        }

        // A kerék forgatásának animálása
        private void RotateWheel()
        {
            Random rand = new Random();
            int randomRounds = rand.Next(3, 6); // Véletlenszerűen 3-6 teljes kört pöröghet
            double rotationAngle = 360 * randomRounds + (rand.NextDouble() * 360); // Plusz véletlenszerű extra szög

            var rotateAnimation = new DoubleAnimation
            {
                From = rotateTransform.Angle,
                To = rotateTransform.Angle + rotationAngle,
                Duration = new Duration(TimeSpan.FromSeconds(5)),
                EasingFunction = new QuadraticEase()
            };

            rotateAnimation.Completed += (sender, e) =>
            {
                double finalAngle = rotateTransform.Angle % 360; // Az aktuális forgatás szögének kiszámítása
                double arrowAngle = (360 - (finalAngle % 360)) % 360; // A nyíl pozíciója (óramutatóval ellentétes irány)

                int winningNumber = _rouletteWheel.GetNumberByAngle(arrowAngle);  // A szám kiszámítása
                ShowWinningNumber(winningNumber);

                // Pénz frissítése és játék vége
                UpdatePlayerAndHouseMoney(winningNumber);
                EnableButtons();
            };

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
        }


       


        private int GetWinningNumber(double finalAngle)
        {
            // A rulett számokat szögeikkel együtt tároljuk
            // Az animáció végén a nyíl pozícióját meghatározhatjuk a végső szög alapján
            foreach (var number in _rouletteWheel.Numbers)
            {
                // Ha az animáció végén a szög közel van a szám szögéhez, akkor azt adjuk vissza
                if (finalAngle >= number.Angle && finalAngle < number.Angle + (360 / 37))
                {
                    return number.Number;
                }
            }

            // Ha nincs találat, akkor visszaadjuk a 0-t (nem várható, hogy ide érünk)
            return 0;
        }


        private void ShowWinningNumber(int winningNumber)
        {
            MessageBox.Show($"Winning number: {winningNumber}");
        }

        private bool CheckIfPlayerWins(int winningNumber)
        {
            bool playerWins = false;

            if (redBet > 0 && _rouletteWheel.Numbers[winningNumber].Color == "Red") playerWins = true;
            if (blackBet > 0 && _rouletteWheel.Numbers[winningNumber].Color == "Black") playerWins = true;
            if (oddBet > 0 && _rouletteWheel.Numbers[winningNumber].Parity == "Odd") playerWins = true;
            if (evenBet > 0 && _rouletteWheel.Numbers[winningNumber].Parity == "Even") playerWins = true;
            if (zeroBet > 0 && winningNumber == 0) playerWins = true;

            return playerWins;
        }
        private void EnableButtons()
        {
            ButtonStart.IsEnabled = true;
            z.IsEnabled = true;
            r.IsEnabled = true;
            b.IsEnabled = true;
            ee.IsEnabled = true;
            o.IsEnabled = true;
        }
       
        private async Task SavePlayerMoneyAsync()
        {
            if (string.IsNullOrEmpty(usernameLabel.Text))
            {
                MessageBox.Show("Nincs bejelentkezett felhasználó!");
                return;
            }

            var saveData = new
            {
                username = usernameLabel.Text.Replace("Username: ", ""), // Csak a név
                money = playerMoney
            };

            var json = JsonConvert.SerializeObject(saveData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsync("http://localhost:5555/update", content);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Pénz mentése sikeres!");
                    }
                    else
                    {
                        MessageBox.Show("Hiba történt a pénz mentése közben!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hálózati hiba történt: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private async void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            await SavePlayerMoneyAsync();  // Pénz mentése a szerverre
            Application.Current.Shutdown(); // Alkalmazás bezárása
        }


    }
}

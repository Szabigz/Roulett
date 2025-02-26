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


namespace Roulett
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random _random;
        private RouletteWheel _rouletteWheel;

        private double redBet = 0;
        private double blackBet = 0;
        private double oddBet = 0;
        private double evenBet = 0;
        private double zeroBet = 0;

        public double playerMoney = 10000;
        public double houseMoney = 999999;
        private double betAmount = 0;

        public MainWindow()
        {
            InitializeComponent();
            _random = new Random();
            _rouletteWheel = new RouletteWheel();
            
            username.Text = "Usename: ";
            PlayerMoney.Text = "Money: "+ playerMoney.ToString();
            HouseMoney.Text = "HouseMoney: "+ houseMoney.ToString();
            foreach (var rouletteNumber in _rouletteWheel.Numbers)
            {
                Console.WriteLine($"Number: {rouletteNumber.Number}, Color: {rouletteNumber.Color}, Parity: {rouletteNumber.Parity}, Angle: {rouletteNumber.Angle}");
            }
        }

        //FOGADAS
        private void BetOnRedButton_Click(object sender, RoutedEventArgs e)
        {
            // A fogadás összegét növeljük, ha újra kattintanak
            redBet += 10;
            betAmount += 10;
            RedBetAmount.Text = $"${redBet}";
           // Fogadott összeg megjelenítése
        }

        // Kattintás: Bet on Black
        private void BetOnBlackButton_Click(object sender, RoutedEventArgs e)
        {
            // A fogadás összegét növeljük, ha újra kattintanak
            blackBet += 10;
            betAmount += 10;
            BlackBetAmount.Text = $"${blackBet}";  // Fogadott összeg megjelenítése
        }

        // Kattintás: Bet on Odd
        private void BetOnOddButton_Click(object sender, RoutedEventArgs e)
        {
            // A fogadás összegét növeljük, ha újra kattintanak
            oddBet += 10;
            betAmount += 10;
            OddBetAmount.Text = $"${oddBet}";  // Fogadott összeg megjelenítése
        }

        // Kattintás: Bet on Even
        private void BetOnEvenButton_Click(object sender, RoutedEventArgs e)
        {
            // A fogadás összegét növeljük, ha újra kattintanak
            evenBet += 10;
            betAmount += 10;
            EvenBetAmount.Text = $"${evenBet}";  // Fogadott összeg megjelenítése
        }

        // Kattintás: Bet on Zero
        private void BetOnZeroButton_Click(object sender, RoutedEventArgs e)
        {
            // A fogadás összegét növeljük, ha újra kattintanak
            zeroBet += 10;
            betAmount += 10;
            ZeroBetAmount.Text = $"${zeroBet}";  // Fogadott összeg megjelenítése
        }

        // Kattintás: Spin
       

        // Játék logika (sima példával, hogy a játékos vagy a ház nyer)
        private bool SimulateRoulette()
        {
            Random rand = new Random();
            int result = rand.Next(0, 37); // 0-36 közötti számot generálunk (a rulett kerékről)

            // Például egyszerűsített logika: ha páros szám, akkor a játékos nyer
            return result % 2 == 0; // Ha páros számot pörgettek, akkor nyert a játékos
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

        //KEREK

        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            RotateWheel();
        }
        private void RotateWheel()
        {
            // Véletlenszerű forgás szög generálása
            Random rand = new Random();
            double rotationAngle = rand.Next(360, 1080);  // 360 és 1080 fok között véletlen számot generálunk (3 - 6 teljes pörgetés)

            // Forgás animáció
            var rotateAnimation = new DoubleAnimation
            {
                From = 0,  // Kezdő szög
                To = rotationAngle,  // Cél szög
                Duration = new Duration(TimeSpan.FromSeconds(3)),  // 3 másodperces animáció
                EasingFunction = new QuadraticEase()  // Simább animáció
            };

            // Az animáció befejeződésekor a Completed eseményhez kötjük a kódot
            rotateAnimation.Completed += (sender, e) =>
            {
                // Pörgetés befejezése után a pénzösszegek frissítése és MessageBox megjelenítése
                bool playerWins = SimulateRoulette(); // Játék logikája, hogy a játékos nyert-e vagy sem.

                // Nyertes állapot kezelése
                if (playerWins)
                {
                    houseMoney -= betAmount;  // A ház pénzéből levonjuk a nyereményt
                    playerMoney += betAmount;  // A játékos pénzéhez hozzáadjuk a nyereményt
                    MessageBox.Show($"You win! You have won ${betAmount}");
                    ResetBets();
                }
                else
                {
                    houseMoney += betAmount;  // A ház pénzéhez adjuk a veszteséget
                    playerMoney -= betAmount;  // A játékos pénzéből levonjuk a veszteséget
                    MessageBox.Show($"You lose! You have lost ${betAmount}");
                    ResetBets();

                }

                // Pénzek frissítése
                HouseMoney.Text = $"House Money: ${houseMoney}";
                PlayerMoney.Text = $"Player Money: ${playerMoney}";

                // Visszaállítjuk a gombot
                ButtonStart.IsEnabled = true;
            };

            // Alkalmazzuk az animációt a képre
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
        }


    }
}

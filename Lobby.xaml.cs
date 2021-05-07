using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace GameOfLife
{
    public partial class Lobby : Window
    {
        public Lobby()
        {
            InitializeComponent();
            this.ResizeMode = System.Windows.ResizeMode.CanMinimize;
        }

        public void Button_start_game(object sender, RoutedEventArgs e)
        {
            
            Regex regex = new Regex("[^0-9]+");
            if (regex.IsMatch(tbxFieldWidth.Text) || regex.IsMatch(tbxfieldHeight.Text))
            {
                MessageBox.Show("Bitte geben Sie für die Feldgröße eine Ganzzahl an!");
            }
            

            MainWindow game = new MainWindow((bool)chkUseTorus.IsChecked, Convert.ToInt32(tbxfieldHeight.Text), Convert.ToInt32(tbxFieldWidth.Text));
            game.Show();
            game.InitializeComponent();
        }
    }
}
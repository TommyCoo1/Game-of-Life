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
using System.Windows.Threading;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Random würfel = new Random();

            spielfeld.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            spielfeld.Arrange(new Rect(0.0, 0.0, spielfeld.DesiredSize.Width, spielfeld.DesiredSize.Height));

            for (int height = 0; height < CellNumberWidth; height++)
            {
                for (int width = 0; width < CellNumberHeight; width++)
                {
                    Rectangle r = new Rectangle();
                    r.Width = (spielfeld.ActualWidth / CellNumberWidth) - 1.0;
                    r.Height = (spielfeld.ActualHeight / CellNumberHeight) - 1.0;
                    r.Fill = Brushes.MediumAquamarine;
                    spielfeld.Children.Add(r);
                    Canvas.SetLeft(r, width * spielfeld.ActualWidth / CellNumberWidth);
                    Canvas.SetTop(r, height * spielfeld.ActualHeight / CellNumberHeight);
                    r.MouseDown += R_MouseDown;

                    Felder[height, width] = r;
                }
            }

            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            {
                int[,] anzahlNachbarn = new int[CellNumberHeight, CellNumberWidth]; //Array für jede Zelle/Rechteck Nachbarn merken ==> Zelle - Anzahlnachbarn

                for (int height = 0; height < CellNumberWidth; height++)
                {
                    for (int width = 0; width < CellNumberHeight; width++)
                    {
                        int topCell = height - 1;
                        if (topCell < 0)
                            topCell = CellNumberHeight - 1;
                        int botCell = height + 1;
                        if (botCell >= CellNumberHeight)
                            botCell = 0;
                        int leftCell = width - 1;
                        if (leftCell < 0)
                            leftCell = CellNumberWidth - 1;
                        int rightCell = width + 1;
                        if (rightCell >= CellNumberWidth)
                            rightCell = 0;

                        int nachbarn = 0;

                        if (Felder[topCell, leftCell].Fill == Brushes.DeepPink)
                        { nachbarn++; }
                        if (Felder[topCell, width].Fill == Brushes.DeepPink)
                        { nachbarn++; }
                        if (Felder[topCell, rightCell].Fill == Brushes.DeepPink)
                        { nachbarn++; }
                        if (Felder[height, leftCell].Fill == Brushes.DeepPink)
                        { nachbarn++; }
                        if (Felder[height, rightCell].Fill == Brushes.DeepPink)
                        { nachbarn++; }
                        if (Felder[botCell, leftCell].Fill == Brushes.DeepPink)
                        { nachbarn++; }
                        if (Felder[botCell, width].Fill == Brushes.DeepPink)
                        { nachbarn++; }
                        if (Felder[botCell, rightCell].Fill == Brushes.DeepPink)
                        { nachbarn++; }

                        anzahlNachbarn[height, width] = nachbarn;


                    }
                }

                for (int height = 0; height < CellNumberWidth; height++)
                {
                    for (int width = 0; width < CellNumberHeight; width++)
                    {
                        if (anzahlNachbarn[height, width] < 2 || anzahlNachbarn[height, width] > 3)
                        {
                            Felder[height, width].Fill = Brushes.MediumAquamarine;
                        }
                        else if (anzahlNachbarn[height, width] == 3)
                        {
                            Felder[height, width].Fill = Brushes.DeepPink;
                        }
                        // Felder[height, width].Fill = (anzahlNachbarn[height, width] < 2 || anzahlNachbarn[height, width] > 3)  ? Brushes.Cyan : (anzahlNachbarn[height, width] == 3) ? Felder[height, width].Fill = Brushes.Red : ---;
                    }
                }
            }
        }

        /*public int CellNumberWidth { get; set; } = 30;
          public int CellNumberHeight { get; set; } = 30;
          Rectangle[,] Felder => new Rectangle[CellNumberWidth, CellNumberHeight]; */
        const int CellNumberWidth = 30;
        const int CellNumberHeight = 30;
        DispatcherTimer timer = new DispatcherTimer();

        Rectangle[,] Felder = new Rectangle[CellNumberWidth, CellNumberHeight];
        List<List<Rectangle>> Felder2 = new List<List<Rectangle>>();

        public static List<List<Rectangle>> adjustList(List<List<Rectangle>> list, int numberoflists)
        {
            for (int i = 0; i < numberoflists; i++)
            {
                list.Add(new List<Rectangle>());
            }
            return list;
        }


        private void start_button_Click(object sender, RoutedEventArgs e)
        {
            Felder2 = adjustList(Felder2, CellNumberWidth);

            for (int height = 0; height < CellNumberWidth; height++)
            {
                for (int width = 0; width < CellNumberHeight; width++)
                {
                    Rectangle r = new Rectangle();
                    r.Width = (spielfeld.ActualWidth / CellNumberWidth) - 1.0;
                    r.Height = (spielfeld.ActualHeight / CellNumberHeight) - 1.0;
                    r.Fill = Brushes.MediumAquamarine;
                    spielfeld.Children.Add(r);
                    Canvas.SetLeft(r, width * spielfeld.ActualWidth / CellNumberWidth);
                    Canvas.SetTop(r, height * spielfeld.ActualHeight / CellNumberHeight);
                    r.MouseDown += R_MouseDown;

                    Felder[height, width] = r;
                }
            }
        }

        private void R_MouseDown(object sender, MouseButtonEventArgs e) // TODO: maybe Methoden Namen ändern
        {
            ((Rectangle)sender).Fill =
                (((Rectangle)sender).Fill == Brushes.MediumAquamarine) ? Brushes.DeepPink : Brushes.MediumAquamarine;
        }

        private void Button_Naester_Schritt_Click(object sender, RoutedEventArgs e)
        {
            int[,] anzahlNachbarn = new int[CellNumberHeight, CellNumberWidth]; //Array für jede Zelle/Rechteck Nachbarn merken ==> Zelle - Anzahlnachbarn

            for (int height = 0; height < CellNumberWidth; height++)
            {
                for (int width = 0; width < CellNumberHeight; width++)
                {
                    int topCell = height - 1;
                    if (topCell < 0)
                        topCell = CellNumberHeight - 1;
                    int botCell = height + 1;
                    if (botCell >= CellNumberHeight)
                        botCell = 0;
                    int leftCell = width - 1;
                    if (leftCell < 0)
                        leftCell = CellNumberWidth - 1;
                    int rightCell = width + 1;
                    if (rightCell >= CellNumberWidth)
                        rightCell = 0;

                    int nachbarn = 0;

                    if (Felder[topCell, leftCell].Fill == Brushes.DeepPink)
                    { nachbarn++; }
                    if (Felder[topCell, width].Fill == Brushes.DeepPink)
                    { nachbarn++; }
                    if (Felder[topCell, rightCell].Fill == Brushes.DeepPink)
                    { nachbarn++; }
                    if (Felder[height, leftCell].Fill == Brushes.DeepPink)
                    { nachbarn++; }
                    if (Felder[height, rightCell].Fill == Brushes.DeepPink)
                    { nachbarn++; }
                    if (Felder[botCell, leftCell].Fill == Brushes.DeepPink)
                    { nachbarn++; }
                    if (Felder[botCell, width].Fill == Brushes.DeepPink)
                    { nachbarn++; }
                    if (Felder[botCell, rightCell].Fill == Brushes.DeepPink)
                    { nachbarn++; }

                    anzahlNachbarn[height, width] = nachbarn;


                }
            }

            for (int height = 0; height < CellNumberWidth; height++)
            {
                for (int width = 0; width < CellNumberHeight; width++)
                {
                    if (anzahlNachbarn[height, width] < 2 || anzahlNachbarn[height, width] > 3)
                    {
                        Felder[height, width].Fill = Brushes.MediumAquamarine;
                    }
                    else if (anzahlNachbarn[height, width] == 3)
                    {
                        Felder[height, width].Fill = Brushes.DeepPink;
                    }
                    // Felder[height, width].Fill = (anzahlNachbarn[height, width] < 2 || anzahlNachbarn[height, width] > 3)  ? Brushes.Cyan : (anzahlNachbarn[height, width] == 3) ? Felder[height, width].Fill = Brushes.Red : ---;
                }
            }
        }

        private void start_stop_button(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                startstop.Content = "Starte Animation!";
            }
            else
            {
                timer.Start();
                startstop.Content = "Stoppe Animation!";
            }
        }


    }
}

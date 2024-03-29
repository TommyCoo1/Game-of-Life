﻿using System;
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
        DispatcherTimer timer = new DispatcherTimer();
        List<List<Rectangle>> Felder = new List<List<Rectangle>>();
        int timerticks = 0;
        private GOLService service;
        
        public bool useTorus
        { get; set; }
        public int CellNumberWidth
        { get; set; }
        public int CellNumberHeight
        { get; set; }

        public MainWindow(bool useTorus, int cellNumberHeight, int cellNumberWidth)
        {
            InitializeComponent();
            this.useTorus = useTorus;
            this.CellNumberHeight = cellNumberHeight;
            this.CellNumberWidth = cellNumberWidth;
            service = new GOLService(CellNumberWidth, CellNumberHeight, Felder, this.useTorus);
            spielfeld.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            spielfeld.Arrange(new Rect(0.0, 0.0, spielfeld.DesiredSize.Width, spielfeld.DesiredSize.Height));
            Felder = service.adjustList(Felder, CellNumberHeight);

            for (int height = 0; height < CellNumberHeight; height++)
            {
                for (int width = 0; width < CellNumberWidth; width++)
                {
                    if (!useTorus && (height == CellNumberHeight - 1 || width == CellNumberWidth - 1 || height == 0 || width == 0))
                    {
                        Rectangle r = new Rectangle();
                        r.Width = (spielfeld.ActualWidth / CellNumberWidth) - 1.0;
                        r.Height = (spielfeld.ActualHeight / CellNumberHeight) - 1.0;
                        r.Fill = Brushes.Gray;
                        spielfeld.Children.Add(r);
                        Canvas.SetLeft(r, width * spielfeld.ActualWidth / CellNumberWidth);
                        Canvas.SetTop(r, height * spielfeld.ActualHeight / CellNumberHeight);

                        Felder[height].Add(r);
                    }
                    else
                    {
                        Rectangle r = new Rectangle();
                        r.Width = (spielfeld.ActualWidth / CellNumberWidth) - 1.0;
                        r.Height = (spielfeld.ActualHeight / CellNumberHeight) - 1.0;
                        r.Fill = Brushes.MediumAquamarine;
                        spielfeld.Children.Add(r);
                        Canvas.SetLeft(r, width * spielfeld.ActualWidth / CellNumberWidth);
                        Canvas.SetTop(r, height * spielfeld.ActualHeight / CellNumberHeight);
                        r.MouseDown += R_MouseDown;

                        Felder[height].Add(r);

                    }
                }
            }

            TransformGroup group = new TransformGroup();
            group.Children.Add(new TranslateTransform(-20, -10));
            spielfeld.RenderTransform = group;
            
            timer.Tick += Timer_Tick;
            this.ResizeMode = System.Windows.ResizeMode.CanMinimize;

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Felder = service.updateCells();
            tbxTimerTicks.Text = Convert.ToString(++timerticks);
        }

        private void start_button_Click(object sender, RoutedEventArgs e)
        {

            for (int height = 0; height < CellNumberHeight; height++)
            {
                for (int width = 0; width < CellNumberWidth; width++)
                {
                    Rectangle r = new Rectangle();
                    r.Width = (spielfeld.ActualWidth / CellNumberWidth) - 1.0;
                    r.Height = (spielfeld.ActualHeight / CellNumberHeight) - 1.0;
                    r.Fill = Brushes.MediumAquamarine;
                    spielfeld.Children.Add(r);
                    Canvas.SetLeft(r, width * spielfeld.ActualWidth / CellNumberWidth);
                    Canvas.SetTop(r, height * spielfeld.ActualHeight / CellNumberHeight);
                    r.MouseDown += R_MouseDown;

                    Felder[height].Add(r);
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
            Felder = service.updateCells();
            tbxTimerTicks.Text = Convert.ToString(++timerticks);
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

        private void random_button(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            
            for (int height = 0; height < CellNumberHeight; height++)
            {
                for (int width = 0; width < CellNumberWidth; width++)
                {
                    if (!useTorus && (height == CellNumberHeight - 1 || width == CellNumberWidth - 1 || height == 0 || width == 0))
                    {
                        Felder[height][width].Fill = Brushes.Gray;
                    }
                    else
                    {
                        Felder[height][width].Fill = (random.Next(0, 2) == 1) ? Brushes.MediumAquamarine : Brushes.DeepPink;
                    }
                }
            }
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double zoom = e.NewValue * 110;

            ZoomViewbox.Height = zoom;
            ZoomViewbox.Width = zoom;
        }

        private void Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            for (int height = 0; height < CellNumberHeight; height++)
            {
                for (int width = 0; width < CellNumberWidth; width++)
                {
                    if (Felder[height][width].Fill == Brushes.DeepPink)
                        Felder[height][width].Fill = Brushes.MediumAquamarine;
                }
            }
            timerticks = 0;
            tbxTimerTicks.Text = Convert.ToString(timerticks);
        }

        private void Slider_TimerConfig(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            timer.Interval = TimeSpan.FromSeconds(1 / (e.NewValue));
        }
    }
}

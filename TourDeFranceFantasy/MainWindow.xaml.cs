using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using Telerik.Windows.Controls.ChartView;
using ViewModel;

namespace TourDeFranceFantasy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TourVM Tour;
        public MainWindow()
        {
            InitializeComponent();
            Tour = new TourVM();
            DataContext = Tour;
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog _openFile = new OpenFileDialog();

            _openFile.Filter = " (*.tdft)|*" + ".tdft";

            if (_openFile.ShowDialog() == true)
            {
                FileInfo _selectedFile = new FileInfo(_openFile.FileName);
                FileInfo[] files = _selectedFile.Directory.GetFiles("*.*");

                int i = 0;
                foreach (FileInfo _fi in files)
                {
                    if (_fi.Extension == ".tdft") 
                        Tour.Teams.Add(new TeamVM(_fi.FullName, Tour, TeamColors[i++]));
                }
                TourVM.TotalNumberOfTeams = i;
                foreach (FileInfo _fi in files)
                {
                    if (_fi.Extension == ".tdfs")
                        Tour.LoadStages(_fi.FullName);
                }
                Tour.StageNumber = 0;
            }
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (Tour.StageNumber < Tour.Teams[0].Stages.Count - 1)
                Tour.StageNumber++;
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            if (Tour.StageNumber > 0)
                Tour.StageNumber--;
        }

        private void ChartSeriesProvider_SeriesCreated(object sender, Telerik.Windows.Controls.ChartView.ChartSeriesCreatedEventArgs e)
        {
            ((LineSeries)e.Series).Stroke = ((StageScoreVM)e.Context).Color;
        }

        public SolidColorBrush[] TeamColors =
        {
            new SolidColorBrush(Colors.Blue),
            new SolidColorBrush(Colors.Green),
            new SolidColorBrush(Colors.Red),
            new SolidColorBrush(Colors.Purple),
            new SolidColorBrush(Colors.Teal),
            new SolidColorBrush(Colors.Orange),
            new SolidColorBrush(Colors.DeepPink),
            new SolidColorBrush(Colors.BlanchedAlmond)
        };
    }
}

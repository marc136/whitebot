using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WhiteBotLogTool.LogParsing;

namespace WhiteBotLogTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<PathTimeInformation> KnownPaths { get { return parser.KnownPaths; } }

        LogParser parser = null;
        string logFileFolder;

        public MainWindow()
        {
            InitializeComponent();

            logFileFolder = System.AppDomain.CurrentDomain.BaseDirectory;
            logFileFolder = @"C:\Users\marc\Documents\HdM\WS 14-15\RoboterProjekt\program\WhiteBot\LogTool\Logs\sample benchmark";

            InitializeParser();
            
            this.DataContext = this;

            //ListKnownPaths.ItemsSource = parser.KnownPaths;
            ListKnownPaths.SelectionMode = System.Windows.Controls.SelectionMode.Single;
        }

        private void InitializeParser()
        {
            parser = new LogParsing.LogParser(logFileFolder, 0);
            parser.UpdateListOfKnownPaths();
            ListKnownPaths.ItemsSource = parser.KnownPaths;
        }


        private void ButtonAnalyze_Click(object sender, RoutedEventArgs e)
        {
            var selectedPathTimeInformation = ListKnownPaths.SelectedItem as PathTimeInformation;

            if (selectedPathTimeInformation == null)
            {
                //if no element selected in Listbox, abort
                MessageBoxResult result = System.Windows.MessageBox.Show("You need to select a path in the listbox above.", "Selection needed", MessageBoxButton.OK, MessageBoxImage.Warning);
                /*if (result == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }/**/
                return;
            }

            parser.ParsePath(selectedPathTimeInformation);
            PaintImage(parser.CurrentPathPoints);

            parser.ParseDesiredPath(selectedPathTimeInformation);
            var a = parser.CurrentDesiredPathPoints;

            PaintDesiredPath(parser.CurrentDesiredPathPoints);
        }

        private void PaintImage(ObservableCollection<Point> pointCollection)
        {
            //DrivenPathCanvas.
            PointCollection points = new PointCollection();
            foreach (Point p in pointCollection)
            {
                points.Add(p);
                DrawPoint(p);
                //DrawPointIntoGeometryObject(p);
            }

          /*  var movedPoints = new Polyline();
            
            movedPoints.Stroke = Brushes.Red;
            movedPoints.StrokeThickness = 2;
            movedPoints.Points = points;

            DrivenPathCanvas.Children.Add(movedPoints);            
           */
            
            //DrivenPathCanvas.Children.Add((Geometry)allMovedPoints);
        }

        private void DrawPoint(Point p)
        {
            var painted = new Ellipse();
            painted.Width = 2;
            painted.Height = 2;
            painted.Fill = Brushes.Red;

            Canvas.SetLeft(painted, p.X - 1); Canvas.SetTop(painted, p.Y - 1);

            DrivenPathCanvas.Children.Add(painted);
        }

        private void PaintDesiredPath(ObservableCollection<Point> passedPoints)
        {
            PointCollection points = new PointCollection();

            foreach (Point point in passedPoints)
            {
                points.Add(point);
            }

            var movedPoints = new Polyline();

            movedPoints.Stroke = Brushes.Black;
            movedPoints.StrokeThickness = 1;
            movedPoints.StrokeDashArray = new DoubleCollection() { 4 };

            movedPoints.Points = points;

            DrivenPathCanvas.Children.Add(movedPoints);            
        }



        GeometryGroup allMovedPoints = new GeometryGroup();

        private void DrawPointIntoGeometryObject(Point p)
        {
            allMovedPoints.Children.Add(
                new EllipseGeometry {
                    Center = p,
                    RadiusX = 3,
                    RadiusY = 3
            });
        }

        private void ButtonChooseFolder_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            dlg.SelectedPath = logFileFolder;

            System.Windows.Forms.DialogResult result = dlg.ShowDialog(this.GetIWin32Window());
            if (result != System.Windows.Forms.DialogResult.OK) return;

            logFileFolder = dlg.SelectedPath;

            InitializeParser();
        }
    }
}

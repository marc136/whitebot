using System;
using System.Collections.Generic;
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
using System.IO;
using System.Collections.ObjectModel;
using System.Threading;

namespace LearnBenchmark
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<String> knownFilepaths = new ObservableCollection<string>();
        private string observedFolder;
        public ObservableCollection<LearnedResult> KnownResults {get; private set; }
        
        FileSystemWatcher watcher;

        public MainWindow()
        {
            InitializeComponent();
            observedFolder = System.AppDomain.CurrentDomain.BaseDirectory;
            KnownResults = new ObservableCollection<LearnedResult>();

            watcher = null;
            //DataContext = knownResults;
            this.DataContext = this;
        }

        private void InitializeFileSystemWatcher()
        {
            if (watcher != null) watcher.Created -= watcher_Created;
            watcher = new FileSystemWatcher(observedFolder, "*.json");
            watcher.Created += watcher_Created;
            watcher.EnableRaisingEvents = true;
        }

        void watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (!knownFilepaths.Contains(e.FullPath))
            {
                Dispatcher.Invoke((Action) (() => {
                    AddFileToLists(e.FullPath); 
                }));
            }
        }
        

        private void BtnOpenFolder_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            dlg.SelectedPath = observedFolder;

            System.Windows.Forms.DialogResult result = dlg.ShowDialog(this.GetIWin32Window());
            if (result != System.Windows.Forms.DialogResult.OK) return;
            
            observedFolder = dlg.SelectedPath;

            knownFilepaths.Clear();
            KnownResults.Clear();
            LoadResultsFromFolder();

            //this.DataContext = KnownResults;
            
            InitializeFileSystemWatcher();
        }

        private void LoadResultsFromFolder()
        {
            foreach (string filepath in Directory.EnumerateFiles(observedFolder, "*.json"))
            {
                if (!knownFilepaths.Contains(filepath))
                {
                    AddFileToLists(filepath);
                }
            }
        }

        private void AddFileToLists(string filepath)
        {
            knownFilepaths.Add(filepath);

            //create learnedResult
            //get file basename
            var title = System.IO.Path.GetFileName(filepath);
            //add new result to list
            var result = LearnedResult.LoadFromFile(filepath);

            KnownResults.Add(result);
        }


        private void BtnAktualisieren_Click(object sender, RoutedEventArgs e)
        {
            if (watcher == null) InitializeFileSystemWatcher();
            LoadResultsFromFolder();
        }
    }

    public class Person
    {
        public string Name { set; get; }
        public int Age { set; get; }
    }
}

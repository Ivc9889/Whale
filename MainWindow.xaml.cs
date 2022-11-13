using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Threading;
using System.Reflection.Emit;

namespace whale
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FolderBrowserDialog folderBrowserDialog;
        private Microsoft.Win32.OpenFileDialog fileDialog;
        private List<int> id;
        private string[] second;
        public MainWindow()
        {
            InitializeComponent();
            TxtFolderPath.Visibility = Visibility.Hidden;
            BtnProcessing.Visibility = Visibility.Hidden;
            LogoImg_Copy.Visibility = Visibility.Hidden;
            BtnAddMore.Visibility = Visibility.Hidden;
            Lblid.Visibility = Visibility.Hidden;
            fileDialog = new Microsoft.Win32.OpenFileDialog();
            folderBrowserDialog = new FolderBrowserDialog();
            id = new List<int>();
            string[] second;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TxtFolderPath.Text = folderBrowserDialog.SelectedPath;
                LogoImg.Visibility = Visibility.Hidden;
                LblChoose.Visibility = Visibility.Hidden;
                BtnChoose.Visibility = Visibility.Hidden;
                BtnFolder.Visibility = Visibility.Hidden;
                Display_img(Directory.GetFiles(TxtFolderPath.Text));
                string startupPath = System.IO.Directory.GetCurrentDirectory();
            }
            else System.Windows.MessageBox.Show("Файл не выбран");
        }
        private void Display_img(string[] str)
        {
            int y = 0;
            second = str;
            foreach (var file in second)
            {
                BtnProcessing.Visibility = Visibility.Visible;
                BtnAddMore.Visibility = Visibility.Visible;
                LogoImg_Copy.Visibility = Visibility.Visible;
                Lblid.Visibility = Visibility.Visible;
                Image image = new();
                System.Windows.Controls.Label label = new System.Windows.Controls.Label();
                label.Margin = new Thickness(0, 237 + y, 36, 0);
                label.Content = file.Replace(TxtFolderPath.Text + "\\", "");
                label.FontSize = 20;
                label.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                var uriImageSource = new Uri(file, UriKind.RelativeOrAbsolute);
                image.Source = new BitmapImage(uriImageSource);
                image.Margin = new Thickness(0, 140 + y, 36, 0);
                image.Height = 100;
                image.VerticalAlignment = VerticalAlignment.Top;
                image.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                grid.Children.Add(label);
                grid.Children.Add(image);
                y = y + 130;
            }
        }
        private async void run_cmd()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Users\mamir\AppData\Local\Programs\Python\Python39\python.exe");
            Process process = new Process();

            string directory = @"C:\Users\mamir\source\repos\whale\whale\";
            string script = "pred.py";
            string path = TxtFolderPath.Text;

            startInfo.WorkingDirectory = directory;
            startInfo.Arguments = string.Format("{0} --path {1}", script, path);
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;

            process.StartInfo = startInfo;

            process.Start();
            var s = process.StandardOutput.ReadToEnd();
           
            s = s.Replace('\r', ' ');
            s = s.Replace('\n', ' ');
            var sd = s.Split(" ");

            foreach (var item in sd)
            {
                if (int.TryParse(item, out var number))
                {
                    id.Add(number);
                }
            }
            process.WaitForExit();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            run_cmd();
            

            int y = 0;
            Dictionary<string, int> whale = new Dictionary<string, int>();
            for (int i = 0; i < second.Length; i++)
            {
                whale.Add(second[i], id[i]);
            }
            for (int i = 0; i < second.Length; i++)
            {
                System.Windows.Controls.Label label12 = new System.Windows.Controls.Label();
                label12.HorizontalAlignment = System.Windows.HorizontalAlignment.Right;
                label12.Margin = new Thickness(0, 160+y, 400, 0);
                label12.Content = whale[second[i]];
                label12.FontSize = 26;
                grid.Children.Add(label12);
                y = y + 130;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
            
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.DefaultExt = ".jpg";
            fileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"; 
            if (fileDialog.ShowDialog() == true)
            {
               var FilePath = fileDialog.FileNames;
                Display_img(FilePath);
            }
        }
    }
}

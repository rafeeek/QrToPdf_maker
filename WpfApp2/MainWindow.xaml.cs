using Grpc.Core;
using iTextSharp.text.pdf;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Policy;
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
using ZXing;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly PdfGenerate pdfGenerate = new PdfGenerate();
        

        public MainWindow()
        {

            InitializeComponent();
            pathCheck();
            FindImage();
        }




        private void pathCheck()
        {
            DirectoryInfo path = new DirectoryInfo(@"C:\qrbackup");
            if (path.Exists == false)
            {
                Directory.CreateDirectory(@"C:\qrbackup");
            }
            else
            {

                foreach (FileInfo file in path.GetFiles())
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    file.Delete();
                }
                foreach (DirectoryInfo dir in path.GetDirectories())
                {
                    dir.Delete(true);
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (IfStartZero.IsChecked == true)
            {
                ZeroNumber();
            }
            else
            {
                NotZero();     
            }
        }


        public void ZeroNumber()
        {
            string FirstNum = text1.Text;
            long FirstNumint = long.Parse(FirstNum);
            text1.Text = "";

            long SecNum = long.Parse(text2.Text);

            int Times = int.Parse(times.Text);

            if (SecNum > 0)
            {
                for (long z = FirstNumint; z <= SecNum; z++)
                {
                    if (Times > 0)
                    {
                        CreatQr("0" + z + "");
                        for (int i = 1; i < Times; i++)
                        {
                            var FinalNum = "0" + z + "00" + i;
                            CreatQr(FinalNum);
                        }
                    }
                    else
                    {
                        CreatQr(FirstNum);
                    }
                }
            }
            else
            {
                if (Times > 0)
                {
                    CreatQr(FirstNum);
                    for (int i = 1; i < Times; i++)
                    {
                        var FinalNum = FirstNum + "00" + i;
                        CreatQr(FinalNum);
                    }
                }
                else
                {
                    CreatQr(FirstNum);
                }
            }
            FindImage();
        }


        public void NotZero()
        {
            string FirstNum = text1.Text;
            long FirstNumint = long.Parse(FirstNum);
            text1.Text = "";

            long SecNum = long.Parse(text2.Text);

            int Times = int.Parse(times.Text);

            if (SecNum > 0)
            {
                for (long z = FirstNumint; z <= SecNum; z++)
                {
                    if (Times > 0)
                    {
                        CreatQr("" + z + "");
                        for (int i = 1; i < Times; i++)
                        {
                            var FinalNum = z + "00" + i;
                            CreatQr(FinalNum);
                        }
                    }
                    else
                    {
                        CreatQr(FirstNum);
                    }
                }
            }
            else
            {
                if (Times > 0)
                {
                    CreatQr(FirstNum);
                    for (int i = 1; i < Times; i++)
                    {
                        var FinalNum = FirstNum + "00" + i;
                        CreatQr(FinalNum);
                    }
                }
                else
                {
                    CreatQr(FirstNum);
                }
            }
            FindImage();
        }

        public void CreatQr(string FirstNum)
        {
            
            BarcodeWriter barcode = new BarcodeWriter();
            barcode.Format = BarcodeFormat.QR_CODE;
            var FolderName = @"C:\qrbackup\";
            barcode.Write(FirstNum).Save(FolderName + FirstNum + ".png");
        }

        public void FindImage()
        {
            DirectoryInfo path = new DirectoryInfo(@"C:\qrbackup");
            EmptyDisplay();
            foreach (FileInfo item in path.EnumerateFiles())
            {
                if (".jpg|.png|.jpeg".Contains(item.Extension.ToLower()))
                {
                    PrepareImage(item.FullName, item.Name);

                }
            }

        }

        public void PrepareImage(string Image, string name)
        {
            Image newImage = new Image();
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(Image, UriKind.Absolute);
            src.EndInit();

            newImage.Source = src;
            newImage.Stretch = Stretch.Uniform;
            newImage.Height = 100;
            ShowImage(name, newImage);
        }

        public void ShowImage(string name , Image newImage)
        {

            name = name.Substring(0, name.Length - 4);

            Label Text = new Label();
            Text.Content = name;
            Text.Margin = new Thickness(0, 130, 0, 0);
            Grid grid = new Grid();
            grid.Children.Add(newImage);
            grid.Children.Add(Text);
            grid.Margin = new Thickness(40, 0, 40, 0);

            imageshow.Children.Add(grid);
        }

        public void EmptyDisplay()
        {
            imageshow.Children.Clear();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (FileName.Text == "FileName" || FileName.Text == "")
            {
                MessageBox.Show("Please Enter a valid FileName!");
            }
            else
            {
                var NameFile = FileName.Text;
                EmptyDisplay();
                pdfGenerate.CreatPdf(NameFile);
            }

        }
    }
}

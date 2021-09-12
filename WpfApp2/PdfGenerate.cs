using DevExpress.Utils.CommonDialogs.Internal;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Ookii.Dialogs.Wpf;
using System.Threading;

namespace WpfApp2
{
    public class PdfGenerate
    {
        

        public void CreatPdf(string NameFile)
        {
            var pgSize = new Rectangle(400, 200);
            var pdfDoc = new Document(pgSize);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            DirectoryInfo pathFolder = new DirectoryInfo(@"C:\qrbackup");

            var dialog = new VistaFolderBrowserDialog();
            dialog.ShowDialog();


            
            var Text = dialog.SelectedPath;
            var time = DateTime.Now.ToString("yyyyMMddHHmmss");

            string path = Text+@"\"+NameFile+".pdf";
            PdfWriter.GetInstance(pdfDoc, new FileStream(path, FileMode.OpenOrCreate));
            pdfDoc.Open();

            if (pathFolder.GetFiles().Count() > 0)
            {
                foreach (FileInfo item in pathFolder.GetFiles())
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    var imagepath = item.FullName;
                    using (FileStream fs = new FileStream(imagepath, FileMode.Open))
                    {
                        var png = Image.GetInstance(System.Drawing.Image.FromStream(fs), ImageFormat.Png);
                        png.ScalePercent(170f);
                        png.SetAbsolutePosition((400 - png.ScaledWidth) / 2, (260 - png.ScaledHeight) / 2);
                        pdfDoc.Add(png);

                        var name = item.Name;
                        name = name.Substring(0, name.Length - 4);

                        //pdfDoc.Add(new Paragraph(String.Format(name)));


                        var refoo = new Paragraph()
                        {
                            String.Format(name)

                        };
                        refoo.Font.Size = 28f;

                        var spacer = new Paragraph("")
                        {

                            SpacingBefore = 110f,
                            SpacingAfter = 0f,
                        };
                        pdfDoc.Add(spacer);

                        refoo.Alignment = Element.ALIGN_CENTER;
                        pdfDoc.Add(refoo);

                        pdfDoc.NewPage();
                    }
                }
                pdfDoc.Close();
                DeletImages();
                MessageBox.Show("Saved In "+Text);

            }
            else
            {
                MessageBox.Show("There Is No QRCode To Save!");
            }
        }


        public void DeletImages()
        {
            DirectoryInfo di = new DirectoryInfo(@"C:\qrbackup");

            foreach (FileInfo file in di.GetFiles())
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

        }

    }
}

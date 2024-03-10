using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Logging;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms.Design;
using static ImageResizer.InformationPanel;
using static System.Net.Mime.MediaTypeNames;

namespace ImageResizer
{
    public partial class Form1 : Form
    {
        InformationPanel IPanel;
        //this path is the location of the logs
        private static string _logPath = "downscale_logs.txt";
        public static string? SelectedImageFullPath { get; set; }
        public static string? SelectedImageName { get; set; }
        public Bitmap CurrentResizedImage { get; set; }


        public Form1()
        {
            InitializeComponent();

            IPanel = new InformationPanel(this);
            IPanel.ShowTip();
        }

        #region Buttons
        private void ResizeBtn_Click(object sender, EventArgs e)
        {
            IPanel.Clear();

            double percentage;
            if (SelectedImageFullPath != null
                && double.TryParse(percentageTB.Text, out percentage)
                && percentage >= 10
                && percentage <= 99)
            {
                double scaleValue = percentage / 100.0;
                Bitmap originalImage = new Bitmap(pb.Image);

                //just for measuring the performance
                Stopwatch stopwatch = Stopwatch.StartNew();
                ResizeType typeOfResizing = TypeCB.SelectedItem?.ToString() == nameof(ResizeType.Sequential)
                    ? ResizeType.Sequential
                    : ResizeType.Parallel;
                if (typeOfResizing == ResizeType.Sequential)
                {
                    stopwatch.Start();
                    CurrentResizedImage = ResizeImageSequential(originalImage, scaleValue);
                    stopwatch.Stop();
                }
                else
                {
                    stopwatch.Start();
                    CurrentResizedImage = ResizeImageParallel(originalImage, scaleValue);
                    stopwatch.Stop();
                }
                RecordMeasurments($"{typeOfResizing} downscaling of {originalImage.Width}x{originalImage.Height} image to {percentage}% took: {stopwatch.ElapsedMilliseconds} ms");

                pb.Image = CurrentResizedImage;
                SaveBtn.Enabled = true;
                IPanel.ShowMessage($"Image '{SelectedImageName}' is resized succesfully to {percentage}% and the performance is logged.", MessageType.Success);
            }
            else
            {
                IPanel.ShowMessage("Please make sure that an image is selected and a percentage is entered between 10 and 99", MessageType.Error);
            }
        }
        private void SelectImageBtn_Click(object sender, EventArgs e)
        {
            SelectImageDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif;*.bmp)|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            SelectImageDialog.Title = "Select an Image";

            if (SelectImageDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    SelectedImageFullPath = SelectImageDialog.FileName;
                    byte[] imageBytes = File.ReadAllBytes(SelectedImageFullPath);
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        pb.Image = System.Drawing.Image.FromStream(ms);
                    }

                    SelectedImageName = Path.GetFileName(SelectedImageFullPath);
                    imgNameLbl.Text = SelectedImageName;

                    IPanel.ShowTip();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            using (FileStream fs = new FileStream(SelectedImageFullPath, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    DialogResult result = MessageBox.Show("The downsised image will overwrite the original. Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        CurrentResizedImage.Save(fs, ImageFormat.Jpeg);
                        SaveBtn.Enabled = false;
                        IPanel.ShowMessage($"Downsized image '{SelectedImageName}' is saved succesfully", MessageType.Success);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private void NextTipBtn_Click(object sender, EventArgs e)
        {
            IPanel.ShowTip();
        }
        #endregion


        #region Resizing Algorithms and Logging 
        private static Bitmap ResizeImageSequential(Bitmap originalImage, double scaleFactor)
        {
            int newWidth = (int)(originalImage.Width * scaleFactor);
            int newHeight = (int)(originalImage.Height * scaleFactor);

            Bitmap resizedImage = new Bitmap(newWidth, newHeight);

            Rectangle rect = new Rectangle(0, 0, originalImage.Width, originalImage.Height);
            BitmapData originalData = originalImage.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData resizedData = resizedImage.LockBits(new Rectangle(0, 0, newWidth, newHeight), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                byte* originalPtr = (byte*)originalData.Scan0;
                byte* resizedPtr = (byte*)resizedData.Scan0;

                // Iterate over each pixel in the resized image
                for (int y = 0; y < newHeight; y++)
                {
                    for (int x = 0; x < newWidth; x++)
                    {
                        int origX = (int)(x / scaleFactor);
                        int origY = (int)(y / scaleFactor);

                        byte* originalPixel = originalPtr + (origY * originalData.Stride) + (origX * 4);
                        byte* resizedPixel = resizedPtr + (y * resizedData.Stride) + (x * 4);

                        // Copy pixel values from original image to resized image
                        resizedPixel[0] = originalPixel[0]; // Blue
                        resizedPixel[1] = originalPixel[1]; // Green
                        resizedPixel[2] = originalPixel[2]; // Red
                        resizedPixel[3] = originalPixel[3]; // Alpha (transparency)
                    }
                }
            }

            originalImage.UnlockBits(originalData);
            resizedImage.UnlockBits(resizedData);

            return resizedImage;
        }
        private static Bitmap ResizeImageParallel(Bitmap originalImage, double scaleFactor)
        {
            int newWidth = (int)(originalImage.Width * scaleFactor);
            int newHeight = (int)(originalImage.Height * scaleFactor);

            Bitmap resizedImage = new Bitmap(newWidth, newHeight);

            Rectangle rect = new Rectangle(0, 0, originalImage.Width, originalImage.Height);
            BitmapData originalData = originalImage.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData resizedData = resizedImage.LockBits(new Rectangle(0, 0, newWidth, newHeight), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            unsafe
            {
                byte* originalPtr = (byte*)originalData.Scan0;
                byte* resizedPtr = (byte*)resizedData.Scan0;

                // Parallelize the resizing process using Parallel.For
                Parallel.For(0, newHeight, y =>
                {
                    for (int x = 0; x < newWidth; x++)
                    {
                        int origX = (int)(x / scaleFactor);
                        int origY = (int)(y / scaleFactor);

                        byte* originalPixel = originalPtr + (origY * originalData.Stride) + (origX * 4);
                        byte* resizedPixel = resizedPtr + (y * resizedData.Stride) + (x * 4);

                        // Copy pixel values from original image to resized image
                        resizedPixel[0] = originalPixel[0]; // Blue
                        resizedPixel[1] = originalPixel[1]; // Green
                        resizedPixel[2] = originalPixel[2]; // Red
                        resizedPixel[3] = originalPixel[3]; // Alpha (transparency)
                    }
                });
            }

            originalImage.UnlockBits(originalData);
            resizedImage.UnlockBits(resizedData);

            return resizedImage;
        }
        private static void RecordMeasurments(string text)
        {
            if (File.Exists(_logPath))
            {
                using (StreamWriter writer = File.AppendText(_logPath))
                {
                    writer.WriteLine(text);
                }
            }
            else
            {
                using (StreamWriter writer = File.CreateText(_logPath))
                {
                    writer.WriteLine(text);
                }
            }
        }
        public enum ResizeType
        {
            Sequential,
            Parallel
        }
        #endregion

        #region Information Panel
        //private void ShowMessage(string message, MessageType type)
        //{
        //    InfoPnl.Visible = true;
        //    InfoLbl2.Text = message;
        //    switch (type)
        //    {
        //        case MessageType.Info:
        //            InfoPnl.BackColor = Color.Orange;
        //            break;
        //        case MessageType.Error:
        //            InfoPnl.BackColor = Color.Tomato;
        //            break;
        //        case MessageType.Success:
        //            InfoPnl.BackColor = Color.MediumSeaGreen;
        //            break;
        //        default:
        //            break;
        //    }
        //}
        //private void Clear()
        //{
        //    InfoPnl.Visible = false;
        //    InfoLbl2.Text = string.Empty;
        //}
        //enum MessageType
        //{
        //    Info,
        //    Error,
        //    Success
        //}
        #endregion

    }
}

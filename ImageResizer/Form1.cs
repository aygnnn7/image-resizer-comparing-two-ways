using AForge.Imaging.Filters;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace ImageResizer
{
    public partial class Form1 : Form
    {
        InformationPanel IPanel;

        //this path is the location of the logs
        private static string _logPath = "../../../../downscale_logs.txt";
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

                stopwatch.Start();
                CurrentResizedImage = ResizeImage(originalImage, scaleValue, typeOfResizing);
                stopwatch.Stop();

                // Record measurements asynchronously
                Task.Run(() => RecordMeasurements(typeOfResizing, originalImage.Width, originalImage.Height, percentage, stopwatch.ElapsedMilliseconds));

                pb.Image = CurrentResizedImage;
                SaveBtn.Enabled = true;
                IPanel.ShowResizedSuccessfully(SelectedImageName, percentage);
            }
            else
            {
                IPanel.ShowInputWarning();
            }
        }
        private void SelectImageBtn_Click(object sender, EventArgs e)
        {
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

                    //make name suitable for UI
                    SelectedImageName = Path.GetFileName(SelectedImageFullPath);
                    int nameLenghtUI = 14;
                    if(SelectedImageName.Length > nameLenghtUI)
                        SelectedImageName = SelectedImageName.Substring(0, nameLenghtUI-3) + "...";
                    imgNameLbl.Text = SelectedImageName;

                    IPanel.ShowTip();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("The downsized image will overwrite the original. Are you sure?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Task.Run(() =>
                {
                    try
                    {
                        using (FileStream fs = new FileStream(SelectedImageFullPath, FileMode.Create, FileAccess.Write))
                        {
                            CurrentResizedImage.Save(fs, ImageFormat.Jpeg);
                        }

                        // This code runs on the UI thread after the save operation is complete.
                        this.Invoke(new Action(() =>
                        {
                            SaveBtn.Enabled = false;
                            IPanel.ShowSavedSuccessfully(SelectedImageName);
                        }));
                    }
                    catch (Exception ex)
                    {
                        // Marshal the exception handling to the UI thread as well.
                        this.Invoke(new Action(() =>
                        {
                            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }));
                    }
                });
            }
        }
        private void NextTipBtn_Click(object sender, EventArgs e)
        {
            IPanel.ShowTip();
        }
        #endregion

        #region Resizing Algorithms and Logging 
        private static Bitmap ResizeImage(Bitmap originalImage, double scaleFactor, ResizeType type)
        {
            //applying pre-blur for optimization
            originalImage = ApplyGaussianBlur(originalImage);

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

                if (type == ResizeType.Parallel)
                {
                    ParallelResize(newHeight, newWidth, scaleFactor, originalData, resizedData, originalPtr, resizedPtr);
                }
                else
                {
                    SequentialResize(newHeight, newWidth, scaleFactor, originalData, resizedData, originalPtr, resizedPtr);
                }
            }

            originalImage.UnlockBits(originalData);
            resizedImage.UnlockBits(resizedData);

            return resizedImage;
        }
        private static unsafe void SequentialResize(int newHeight, int newWidth, double scaleFactor, BitmapData originalData, BitmapData resizedData, byte* originalPtr, byte* resizedPtr)
        {
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    ResizePixel(x, y, scaleFactor, originalData, resizedData, originalPtr, resizedPtr);
                }
            }
        }
        private static unsafe void ParallelResize(int newHeight, int newWidth, double scaleFactor, BitmapData originalData, BitmapData resizedData, byte* originalPtr, byte* resizedPtr)
        {
            Parallel.For(0, newHeight, y =>
            {
                for (int x = 0; x < newWidth; x++)
                {
                    ResizePixel(x, y, scaleFactor, originalData, resizedData, originalPtr, resizedPtr);
                }
            });
        }
        private static unsafe void ResizePixel(int x, int y, double scaleFactor, BitmapData originalData, BitmapData resizedData, byte* originalPtr, byte* resizedPtr)
        {
            int origX = (int)(x / scaleFactor);
            int origY = (int)(y / scaleFactor);

            byte* originalPixel = originalPtr + (origY * originalData.Stride) + (origX * 4);
            byte* resizedPixel = resizedPtr + (y * resizedData.Stride) + (x * 4);

            resizedPixel[0] = originalPixel[0]; // Blue
            resizedPixel[1] = originalPixel[1]; // Green
            resizedPixel[2] = originalPixel[2]; // Red
            resizedPixel[3] = originalPixel[3]; // Alpha (transparency)
        }
        private static Bitmap ApplyGaussianBlur(Bitmap image)
        {
            GaussianBlur blurfilter = new GaussianBlur(4, 11);

            return blurfilter.Apply(image);
        }
        private static void RecordMeasurements(ResizeType typeOfResizing, int imgW, int imgH, double percentage, long elapsedMs)
        {
            string text = $"{typeOfResizing} downscaling of {imgW}x{imgH} image to {percentage}% took: {elapsedMs} ms";
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

    }
}

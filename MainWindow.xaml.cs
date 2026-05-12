using Microsoft.Win32;
using PixalLap.Services;
using System.Windows;
using System.Windows.Media.Imaging;
using System;


namespace PixalLap
{
    public partial class MainWindow : Window
    {
        private BitmapImage originalImage;
        private BitmapImage currentImage;

        private ImageService imageService = new ImageService();

        private ColorProcessingService colorProcessingService =
    new ColorProcessingService();

        public MainWindow()
        {
            InitializeComponent();
        }

        // فتح صورة
        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (dialog.ShowDialog() == true)
            {
                originalImage = imageService.LoadImage(dialog.FileName);

                currentImage = originalImage;

                MainImage.Source = currentImage;
            }
        }

        // حفظ الصورة
        private void SaveImage_Click(object sender, RoutedEventArgs e)
        {
            if (currentImage != null)
            {
                imageService.SaveImage(currentImage);
            }
        }

        // إعادة ضبط الصورة
        private void ResetImage_Click(object sender, RoutedEventArgs e)
        {
            if (originalImage != null)
            {
                currentImage = originalImage;

                MainImage.Source = currentImage;
            }
        }

        // السحب والإفلات
        private void ImageDropArea_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (files.Length > 0)
                {
                    originalImage = imageService.LoadImage(files[0]);

                    currentImage = originalImage;

                    MainImage.Source = currentImage;
                }
            }
        }

        // السماح بالسحب
        private void ImageDropArea_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;

            e.Handled = true;
        }

        private void RGBSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (currentImage == null)
                return;

            double red = RedSlider.Value;
            double green = GreenSlider.Value;
            double blue = BlueSlider.Value;

            RedValueText.Text = red.ToString("0.0");
            GreenValueText.Text = green.ToString("0.0");
            BlueValueText.Text = blue.ToString("0.0");

            BitmapSource processedImage =
                colorProcessingService.AdjustRGBChannels(
                    originalImage,
                    red,
                    green,
                    blue);

            currentImage = ConvertBitmapSourceToBitmapImage(processedImage);

            MainImage.Source = currentImage;
        }

        private BitmapImage ConvertBitmapSourceToBitmapImage(BitmapSource bitmapSource)
        {
            using (System.IO.MemoryStream memory =
                   new System.IO.MemoryStream())
            {
                BitmapEncoder encoder = new PngBitmapEncoder();

                encoder.Frames.Add(
                    BitmapFrame.Create(bitmapSource));

                encoder.Save(memory);

                memory.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.CacheOption =
                    BitmapCacheOption.OnLoad;

                bitmapImage.StreamSource = memory;

                bitmapImage.EndInit();

                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}
using Microsoft.Win32;
using PixalLap.Services;
using System.Windows;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Controls;
using System.IO;


namespace PixalLap
{
    public partial class MainWindow : Window
    {
        private BitmapImage originalImage;
        private BitmapImage currentImage;
        private bool isResetting = false;
        private string currentImagePath;

        private ImageService imageService = new ImageService();

        private ColorProcessingService colorProcessingService =
    new ColorProcessingService();

        private HSVProcessingService hsvProcessingService =
    new HSVProcessingService();

        private CMYKProcessingService cmykProcessingService =
    new CMYKProcessingService();

        private YCbCrProcessingService ycbcrProcessingService =
    new YCbCrProcessingService();

        private YUVProcessingService yuvProcessingService =
    new YUVProcessingService();

        private LABProcessingService labProcessingService =
    new LABProcessingService();

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
                currentImagePath = dialog.FileName;

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
        private void ResetImage_Click(
    object sender,
    RoutedEventArgs e)
        {
            if (originalImage == null)
                return;

            isResetting = true;

            // Reset RGB
            RedSlider.Value = 1;
            GreenSlider.Value = 1;
            BlueSlider.Value = 1;

            // Reset HSV
            HueSlider.Value = 0;
            SaturationSlider.Value = 1;
            ValueSlider.Value = 1;

            // Reset CMYK
            CyanSlider.Value = 1;
            MagentaSlider.Value = 1;
            YellowSlider.Value = 1;
            BlackSlider.Value = 1;

            // Reset YCbCr
            YSlider.Value = 1;
            CbSlider.Value = 1;
            CrSlider.Value = 1;

            // Reset YUV
            YUVYSlider.Value = 1;
            USlider.Value = 1;
            VSlider.Value = 1;

            // Reset LAB
            LSlider.Value = 1;
            ASlider.Value = 1;
            BSlider.Value = 1;

            currentImage = originalImage;

            MainImage.Source = currentImage;

            isResetting = false;
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
                    currentImagePath = files[0];

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

        private void RGBSlider_ValueChanged(
     object sender,
     RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded)
                return;

            if (RedValueText == null ||
                GreenValueText == null ||
                BlueValueText == null)
                return;

            RedValueText.Text =
                RedSlider.Value.ToString("0.0");

            GreenValueText.Text =
                GreenSlider.Value.ToString("0.0");

            BlueValueText.Text =
                BlueSlider.Value.ToString("0.0");

            RenderCurrentColorSpace();
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
        private void ColorSpaceComboBox_SelectionChanged(
     object sender,
     SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;

            if (ColorSpaceComboBox.SelectedItem == null)
                return;

            ComboBoxItem selectedItem =
                ColorSpaceComboBox.SelectedItem as ComboBoxItem;

            if (selectedItem == null)
                return;

            string selectedSpace =
                selectedItem.Content.ToString();

            RGBPanel.Visibility =
                Visibility.Collapsed;

            HSVPanel.Visibility =
                Visibility.Collapsed;

            CMYKPanel.Visibility =
                Visibility.Collapsed;

            YCbCrPanel.Visibility =
                 Visibility.Collapsed;

            YUVPanel.Visibility =
                Visibility.Collapsed;

            LABPanel.Visibility =
               Visibility.Collapsed;

            if (selectedSpace == "RGB")
            {
                RGBPanel.Visibility =
                    Visibility.Visible;
            }
            else if (selectedSpace == "HSV")
            {
                HSVPanel.Visibility =
                    Visibility.Visible;
            }
            else if (selectedSpace == "CMYK")
            {
                CMYKPanel.Visibility =
                    Visibility.Visible;
            }

            else if (selectedSpace == "YCbCr")
            {
                YCbCrPanel.Visibility =
                    Visibility.Visible;
            }

            else if (selectedSpace == "YUV")
            {
                YUVPanel.Visibility =
                    Visibility.Visible;
            }

            else if (selectedSpace == "LAB")
            {
                LABPanel.Visibility =
                    Visibility.Visible;
            }

            RenderCurrentColorSpace();
        }

        private void HSVSlider_ValueChanged(
     object sender,
     RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded)
                return;

            if (HueValueText == null ||
                SaturationValueText == null ||
                ValueValueText == null)
                return;

            HueValueText.Text =
                HueSlider.Value.ToString("0");

            SaturationValueText.Text =
                SaturationSlider.Value.ToString("0.0");

            ValueValueText.Text =
                ValueSlider.Value.ToString("0.0");

            RenderCurrentColorSpace();
        }

        private void CMYKSlider_ValueChanged(
    object sender,
    RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded)
                return;

            if (CyanValueText == null ||
                MagentaValueText == null ||
                YellowValueText == null ||
                BlackValueText == null)
                return;

            CyanValueText.Text =
                CyanSlider.Value.ToString("0.0");

            MagentaValueText.Text =
                MagentaSlider.Value.ToString("0.0");

            YellowValueText.Text =
                YellowSlider.Value.ToString("0.0");

            BlackValueText.Text =
                BlackSlider.Value.ToString("0.0");

            RenderCurrentColorSpace();
        }

        private void YCbCrSlider_ValueChanged(
    object sender,
    RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded)
                return;

            if (YValueText == null ||
                CbValueText == null ||
                CrValueText == null)
                return;

            YValueText.Text =
                YSlider.Value.ToString("0.0");

            CbValueText.Text =
                CbSlider.Value.ToString("0.0");

            CrValueText.Text =
                CrSlider.Value.ToString("0.0");

            RenderCurrentColorSpace();
        }

        private void YUVSlider_ValueChanged(
    object sender,
    RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded)
                return;

            if (YUVYValueText == null ||
                UValueText == null ||
                VValueText == null)
                return;

            YUVYValueText.Text =
                YUVYSlider.Value.ToString("0.0");

            UValueText.Text =
                USlider.Value.ToString("0.0");

            VValueText.Text =
                VSlider.Value.ToString("0.0");

            RenderCurrentColorSpace();
        }

        private void LABSlider_ValueChanged(
    object sender,
    RoutedPropertyChangedEventArgs<double> e)
        {
            if (!IsLoaded)
                return;

            if (LValueText == null ||
                AValueText == null ||
                BValueText == null)
                return;

            LValueText.Text =
                LSlider.Value.ToString("0.0");

            AValueText.Text =
                ASlider.Value.ToString("0.0");

            BValueText.Text =
                BSlider.Value.ToString("0.0");

            RenderCurrentColorSpace();
        }

        private void RenderCurrentColorSpace()
        {

            if (isResetting)
                return;
            // حماية
            if (!IsLoaded)
                return;

            if (originalImage == null)
                return;

            if (ColorSpaceComboBox.SelectedItem == null)
                return;

            ComboBoxItem selectedItem =
                ColorSpaceComboBox.SelectedItem as ComboBoxItem;

            if (selectedItem == null)
                return;

            string selectedSpace =
                selectedItem.Content.ToString();

            BitmapSource processedImage = null;

            // RGB
            if (selectedSpace == "RGB")
            {
                processedImage =
                    colorProcessingService.AdjustRGBChannels(
                        originalImage,
                        RedSlider.Value,
                        GreenSlider.Value,
                        BlueSlider.Value);
            }

            // HSV
            else if (selectedSpace == "HSV")
            {
                processedImage =
                    hsvProcessingService.AdjustHSV(
                        originalImage,
                        HueSlider.Value,
                        SaturationSlider.Value,
                        ValueSlider.Value);
            }
            // CMYK
            else if (selectedSpace == "CMYK")
            {
                processedImage =
                    cmykProcessingService.AdjustCMYK(
                        originalImage,
                        CyanSlider.Value,
                        MagentaSlider.Value,
                        YellowSlider.Value,
                        BlackSlider.Value);
            }
            //YCbCr
            else if (selectedSpace == "YCbCr")
            {
                processedImage =
                    ycbcrProcessingService.AdjustYCbCr(
                        originalImage,
                        YSlider.Value,
                        CbSlider.Value,
                        CrSlider.Value);
            }

            //YUV
            else if (selectedSpace == "YUV")
            {
                processedImage =
                    yuvProcessingService.AdjustYUV(
                        originalImage,
                        YUVYSlider.Value,
                        USlider.Value,
                        VSlider.Value);
            }

            //LAB 
            else if (selectedSpace == "LAB")
            {
                processedImage =
                    labProcessingService.AdjustLAB(
                        originalImage,
                        LSlider.Value,
                        ASlider.Value,
                        BSlider.Value);
            }
            // حماية
            if (processedImage == null)
                return;

            currentImage =
                ConvertBitmapSourceToBitmapImage(processedImage);

            MainImage.Source =
                currentImage;
        }


        private void ImageInfo_Click(
    object sender,
    RoutedEventArgs e)
        {
            if (originalImage == null ||
                string.IsNullOrEmpty(currentImagePath))
            {
                MessageBox.Show(
                    "No image loaded!",
                    "Image Information",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            FileInfo fileInfo =
                new FileInfo(currentImagePath);

            double sizeInKB =
                fileInfo.Length / 1024.0;

            string info =
                $"Name: {fileInfo.Name}\n\n" +

                $"Format: {fileInfo.Extension}\n\n" +

                $"Size: {sizeInKB:F2} KB\n\n" +

                $"Resolution: " +
                $"{originalImage.PixelWidth} x " +
                $"{originalImage.PixelHeight}\n\n" +

                $"DPI: " +
                $"{originalImage.DpiX} x " +
                $"{originalImage.DpiY}\n\n" +

                $"Path:\n{currentImagePath}";

            MessageBox.Show(
                info,
                "Image Information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}
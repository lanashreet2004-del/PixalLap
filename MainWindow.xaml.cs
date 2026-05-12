using Microsoft.Win32;
using PixalLap.Services;
using System.Windows;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Controls;


namespace PixalLap
{
    public partial class MainWindow : Window
    {
        private BitmapImage originalImage;
        private BitmapImage currentImage;

        private ImageService imageService = new ImageService();

        private ColorProcessingService colorProcessingService =
    new ColorProcessingService();

        private HSVProcessingService hsvProcessingService =
    new HSVProcessingService();

        private CMYKProcessingService cmykProcessingService =
    new CMYKProcessingService();

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

        private void RenderCurrentColorSpace()
        {
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

            // حماية
            if (processedImage == null)
                return;

            currentImage =
                ConvertBitmapSourceToBitmapImage(processedImage);

            MainImage.Source =
                currentImage;
        }
    }
}
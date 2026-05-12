using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace PixalLap.Services
{
    public class ImageService
    {
        public BitmapImage LoadImage(string path)
        {
            BitmapImage bitmap = new BitmapImage();

            bitmap.BeginInit();
            bitmap.UriSource = new Uri(path);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            return bitmap;
        }

        public void SaveImage(BitmapImage image)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg";

            if (dialog.ShowDialog() == true)
            {
                BitmapEncoder encoder;

                if (dialog.FilterIndex == 1)
                {
                    encoder = new PngBitmapEncoder();
                }
                else
                {
                    encoder = new JpegBitmapEncoder();
                }

                encoder.Frames.Add(BitmapFrame.Create(image));

                using (var stream = File.Create(dialog.FileName))
                {
                    encoder.Save(stream);
                }
            }
        }
    }
}
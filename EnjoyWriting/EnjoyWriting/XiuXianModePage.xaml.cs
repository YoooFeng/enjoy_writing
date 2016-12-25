using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EnjoyWriting
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class XiuXianModePage : Page
    {
                WriteableBitmap src;
        private Point currentPoint; //最新的，当前的点
        private Point oldPoint;//上一个点
        Point lpoint;
        private static bool isDown = false;
        bool Lisdown = false;
        int savew = 0;
        int saveh = 0;
        int correct = 0;
        int total = 0;
        BitmapDecoder decoder;
        bool isin = false;
        byte[] pixels;
        byte[] record = new byte[500*500*4];
        PixelDataProvider pixelProvider;
        int status = 0;
        Point leftpoint;
        bool music_on = true;
        TimeSpan recent_time;


        public XiuXianModePage()
        {
            this.InitializeComponent();
            DecodeImage();
            for (long i = 0; i < 250000*4; i++)
            {
                record[i] = 255;
            }
        }


        async void DecodeImage()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.FileTypeFilter.Add(".png");
            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                var bitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                await bitmapImage.SetSourceAsync(stream);
                myImage.Source = bitmapImage;
                decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
                pixelProvider = await decoder.GetPixelDataAsync(
                   Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8,
                    Windows.Graphics.Imaging.BitmapAlphaMode.Straight,
                  new Windows.Graphics.Imaging.BitmapTransform(),
                  Windows.Graphics.Imaging.ExifOrientationMode.RespectExifOrientation,
                    Windows.Graphics.Imaging.ColorManagementMode.ColorManageToSRgb);
                pixels = pixelProvider.DetachPixelData();

                int width = (int)decoder.PixelWidth;
                int height = (int)decoder.PixelHeight;
                saveh = height;
                savew = width;
                src = new WriteableBitmap(width, height);
                Stream pixelStream = src.PixelBuffer.AsStream();
                pixelStream.Write(pixels, 0, pixels.Length);
                pixelStream.Dispose();
                stream.Dispose();
            }
            else
            {
                this.Frame.Navigate(typeof(HomePage));
            }

        }

//        async void GetPixelData(Windows.Graphics.Imaging.BitmapDecoder decoder)
//        {
            //            pixelProvider = await decoder.GetPixelDataAsync(
            //                Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8,
            //                Windows.Graphics.Imaging.BitmapAlphaMode.Straight,
            //                new Windows.Graphics.Imaging.BitmapTransform(),
            //                Windows.Graphics.Imaging.ExifOrientationMode.RespectExifOrientation,
            //              Windows.Graphics.Imaging.ColorManagementMode.ColorManageToSRgb);
            //            pixels = pixelProvider.DetachPixelData();
            //            var width = decoder.OrientedPixelWidth;
            //            var height = decoder.OrientedPixelHeight;
            //            t1.Text = height.ToString();
            //            t2.Text = width.ToString();
            //            t1.Text = pixels[(x * height + y) * 4 + 0].ToString();
            //            t2.Text = pixels[(x * height + y) * 4 + 1].ToString();
            //            t1.Text = pixels[(y * width + x) * 4].ToString();
            //            t2.Text = pixels[(y * width + x) * 4+1].ToString();
            //            if (x > width) x = Convert.ToInt32(width);
            //            if (y > height) y = Convert.ToInt32(height);
            //            if (pixels[(y * width + x) * 4] <= 200)
            //                isin = true;
            //            else
            //                isin = false;
//        }

        void checkpixel(byte[] pixels, int x, int y)
        {
            var width = decoder.OrientedPixelWidth;
            var height = decoder.OrientedPixelHeight;
            if (y >= 500) y = 499;
            if (y < 0) y = 0;
            if (pixels[(y * width + x) * 4] <= 200)
                isin = true;
            else
                isin = false;

        }

        private void tp(object sender, TappedRoutedEventArgs e)
        {
            if (status == 0)
            {
                leftflash.Begin();
                status = 1;
            }
        }

        private void leftpm(object sender, PointerRoutedEventArgs e)
        {
            Point leftnowpoint;
            if (status == 1)
                if(Lisdown==false)
            {
                leftnowpoint.X = e.GetCurrentPoint(LeftCanvas).Position.X;
                leftnowpoint.Y = e.GetCurrentPoint(LeftCanvas).Position.Y;
                if (leftpoint.X - leftnowpoint.X > 100)
                {
                    status = 0;
                    leftflashbcak.Begin();
                    leftpoint.X = 0;
                    leftpoint.Y = 0;
                }
            }
        }

        private void leftpp(object sender, PointerRoutedEventArgs e)
        {
            if (status == 1)
            {
                leftpoint.X = e.GetCurrentPoint(LeftCanvas).Position.X;
                leftpoint.Y = e.GetCurrentPoint(LeftCanvas).Position.Y;
            }
        }


        private void canvaspointmove(object sender, PointerRoutedEventArgs e)
        {
            if (isDown == true)
            {
                currentPoint = e.GetCurrentPoint(MainCanvas).Position;
                checkpixel(pixels, Convert.ToInt32(currentPoint.X + 10), Convert.ToInt32(currentPoint.Y));
                if (isin == true)
                    correct++;
                checkpixel(pixels, Convert.ToInt32(currentPoint.X - 10), Convert.ToInt32(currentPoint.Y));
                if (isin == true)
                    correct++;
                checkpixel(pixels, Convert.ToInt32(currentPoint.X), Convert.ToInt32(currentPoint.Y + 10));
                if (isin == true)
                    correct++;
                checkpixel(pixels, Convert.ToInt32(currentPoint.X), Convert.ToInt32(currentPoint.Y - 10));
                if (isin == true)
                    correct++;

                //                    check = false;

                //                if (check)
                //                    correct++;

                total += 4;
                Line line = new Line() { X1 = currentPoint.X, Y1 = currentPoint.Y, X2 = oldPoint.X, Y2 = oldPoint.Y };


                line.Stroke = new SolidColorBrush(Colors.Blue);
                line.StrokeThickness = 25;
                line.StrokeLineJoin = PenLineJoin.Round;
                line.StrokeStartLineCap = PenLineCap.Round;
                line.StrokeEndLineCap = PenLineCap.Round;
                DrawCanvas.Children.Add(line);

                int dian1 = Convert.ToInt32(oldPoint.Y-currentPoint.Y );
                int dian2 = Convert.ToInt32(oldPoint.X-currentPoint.X);

                if (Convert.ToInt32(oldPoint.Y) > 12 && Convert.ToInt32(oldPoint.Y) < 478 
                    && Convert.ToInt32(oldPoint.X) > 12 && Convert.ToInt32(oldPoint.X) < 478 
                    && Convert.ToInt32(currentPoint.Y) > 12 && Convert.ToInt32(currentPoint.Y) < 478
                    && Convert.ToInt32(currentPoint.X) > 12 && Convert.ToInt32(currentPoint.X) < 478)
                {
                    for (int jk = 0; jk <= 10; jk++)
                    {
                        int thisY = Convert.ToInt32(currentPoint.Y) + dian1 * jk / 10;
                        int thisX = Convert.ToInt32(currentPoint.X) + dian2 * jk / 10;
                        for (int jk1 = -12; jk1 < 12; jk1++)
                        {
                            for (int jk2 = -12; jk2 < 12; jk2++)
                            {
                                if (jk1 * jk1 + jk2 * jk2 < 144)
                                {
                                    record[((thisY + jk1) * 500 + (thisX + jk2)) * 4] = 0;
                                    record[((thisY + jk1) * 500 + (thisX + jk2)) * 4 + 1] = 0;
                                    record[((thisY + jk1) * 500 + (thisX + jk2)) * 4 + 2] = 0;
                                    record[((thisY + jk1) * 500 + (thisX + jk2)) * 4 + 3] = 255;
                                    //Y坐标越界
                                }
                            }
                        }
                    }
                }

                oldPoint = currentPoint;
            }
        }

        private void canvaspointpress(object sender, PointerRoutedEventArgs e)
        {
            isDown = true;
            currentPoint = e.GetCurrentPoint(MainCanvas).Position;
            oldPoint = currentPoint;
        }

        private void canvaspointrelease(object sender, PointerRoutedEventArgs e)
        {
            isDown = false;
        }

        private void tudong2_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Lisdown = true;
            lpoint.X = e.GetCurrentPoint(LeftCanvas).Position.X;
            lpoint.Y = e.GetCurrentPoint(LeftCanvas).Position.Y;
        }

        private void tudong2_PointerR(object sender, PointerRoutedEventArgs e)
        {
            Lisdown = false;
        }

        private void tudong2_PointerM(object sender, PointerRoutedEventArgs e)
        {
            Point newp;
            newp.X = e.GetCurrentPoint(LeftCanvas).Position.X;
            if (Lisdown == true)
            {
                tuodong1.Opacity=(newp.X-150)/340;
                myImage.Opacity = (newp.X - 150) / 340;
            }
        }

        private void querenpp(object sender, PointerRoutedEventArgs e)
        {
            int result = 0;
            double estimate = (double)1 / 40 * findAllBlack();
            double tmp = 0;
            if (correct > estimate && total != 0)
            {
                tmp = (double)correct / total * 100;

            }
            else if (total != 0)
            {
                double tmp2 = (double)correct / estimate;
                double tmp1 = (double)correct / total;
                tmp = tmp2 * tmp1 * 100;
            }
            result = (int)tmp;
//            Windows.UI.Popups.MessageDialog a = new Windows.UI.Popups.MessageDialog("恭喜你获得了" + result.ToString() + " 分");
//            a.Title = "得分";
//            await a.ShowAsync();

            middle.TranslateY = 0;

            status = 0;
            leftflashbcak.Begin();
            leftpoint.X = 0;
            leftpoint.Y = 0;

            if (result < 33)
            {
                middleflash.Begin();
                chuxian3.Begin();
                gongxi.Text = "很遗憾你获得了 " + result.ToString() + " 分";
            }
            else if (result < 66)
            {
                middleflash.Begin();
                chuxian3.Begin();
                chuxian2.Begin();
                gongxi.Text = "恭喜你你获得了 " + result.ToString() + " 分";
            }
            else
            {
                middleflash.Begin();
                chuxian3.Begin();
                chuxian2.Begin();
                chuxian1.Begin();
                gongxi.Text = "太棒了，你获得了 " + result.ToString() + " 分";
            }
        }

        private int findAllBlack()
        {
            var width = decoder.OrientedPixelWidth;
            var height = decoder.OrientedPixelHeight;
            int black = 0;
            for (int i = 0; i < width * height * 4; i = i + 4)
            {
                if (pixels[i] <= 200)
                    black++;
            }
            return black;
        }//计算总的黑点数量

        private void querenpr(object sender, PointerRoutedEventArgs e)
        {
            queren1.Opacity = 1;
            queren2.Opacity = 2;
        }

        private void chonghuipp(object sender, PointerRoutedEventArgs e)
        {
            DrawCanvas.Children.Clear();
            correct = 0;
            total = 0;
            chonghui1.Opacity = 0;
            chonghui2.Opacity = 1;
            for (long i = 0; i < 250000*4; i++)
            {
                record[i] = 255;
            }

        }

        private void chonghuipr(object sender, PointerRoutedEventArgs e)
        {
            chonghui1.Opacity = 1;
            chonghui2.Opacity = 0;
        }

/*        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bmap = new BitmapImage();
            Stream stream = new MemoryStream(pixels);
            FileSavePicker save = new FileSavePicker();
            save.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            save.DefaultFileExtension = ".png";
            save.SuggestedFileName = "new image";
            save.FileTypeChoices.Add(".png", new List<string>() { ".png" });
            StorageFile savedItem = await save.PickSaveFileAsync();
            try
            {
                IRandomAccessStream fileStream = await savedItem.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, fileStream);
                //pixal format should convert to rgba8
                Stream pixelStream = src.PixelBuffer.AsStream();
                byte[] mypixels = new byte[pixelStream.Length];
                pixelStream.Read(mypixels, 0, mypixels.Length);
                encoder.SetPixelData(
                BitmapPixelFormat.Rgba8,
                BitmapAlphaMode.Straight,
                500,
                500,
                96, // Horizontal DPI
                96, // Vertical DPI
                record
                );
                await encoder.FlushAsync();
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }
*/
        private void baocunpp(object sender, PointerRoutedEventArgs e)
        {
            baocun1.Opacity = 0;
            baocun2.Opacity = 1;

        }

        private async void baocunpr(object sender, PointerRoutedEventArgs e)
        {
            baocun1.Opacity = 1;
            baocun2.Opacity = 0;
            BitmapImage bmap = new BitmapImage();
            Stream stream = new MemoryStream(pixels);
            FileSavePicker save = new FileSavePicker();
            save.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            save.DefaultFileExtension = ".png";
            save.SuggestedFileName = "new image";
            save.FileTypeChoices.Add(".png", new List<string>() { ".png" });
            StorageFile savedItem = await save.PickSaveFileAsync();
            if (savedItem != null)
            {
                try
                {
                    IRandomAccessStream fileStream = await savedItem.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                    BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, fileStream);
                    //pixal format should convert to rgba8
                    Stream pixelStream = src.PixelBuffer.AsStream();
                    byte[] mypixels = new byte[pixelStream.Length];
                    pixelStream.Read(mypixels, 0, mypixels.Length);
                    encoder.SetPixelData(
                    BitmapPixelFormat.Rgba8,
                    BitmapAlphaMode.Straight,
                    500,
                    500,
                    96, // Horizontal DPI
                    96, // Vertical DPI
                    record
                    );
                    await encoder.FlushAsync();
                }
                catch (Exception exc)
                {
                    throw exc;
                }
            }

        }

        private void fanhui_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            fanhuiStoryboard.Begin();
        }

        private void fanhui_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomePage));

        }
        private void musictap(object sender, TappedRoutedEventArgs e)
        {
            if (music_on)
            {
                music_on = false;
                music1.Opacity = 0;
                music2.Opacity = 0.8;
                recent_time = media1.Position;
                media1.Stop();
            }
            else
            {
                music_on = true;
                music1.Opacity = 0.8;
                music2.Opacity = 0;
                media1.Position = recent_time;
                media1.Play();
            }
        }

        private void musicend(object sender, RoutedEventArgs e)
        {
            media1.Position = new TimeSpan(0);
            media1.Play();
        }

        private void queren2pp(object sender, PointerRoutedEventArgs e)
        {
            queren3.Opacity = 0;
            queren4.Opacity = 1;

        }

        private void queren2pr(object sender, PointerRoutedEventArgs e)
        {
            queren3.Opacity = 1;
            queren4.Opacity = 0;
            DrawCanvas.Children.Clear();
            middle.TranslateY = 768;

            correct = 0;
            total = 0;

            DecodeImage();
            for (long i = 0; i < 250000 * 4; i++)
            {
                record[i] = 255;
            }

            wujiaoxinfill_1.Opacity = 0;
            wujiaoxinfill_2.Opacity = 0;
            wujiaoxinfill_3.Opacity = 0;

        }

    }
}

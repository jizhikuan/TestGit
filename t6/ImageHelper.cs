using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media.Imaging;
using VideoTest.Data;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace VideoTest.Utils
{
    public static class ImageHelper
    {
        public static ImageInfo ReadBMPFromImage(Image image, ImageInfo imageInfo)
        {
            //将Image转换为Format24bppRgb格式的BMP
            Bitmap bm = new Bitmap(image);
            BitmapData data = bm.LockBits(new Rectangle(0, 0, bm.Width, bm.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            try
            {
                Marshal.FreeHGlobal(imageInfo.imgData);
                //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行
                IntPtr ptr = data.Scan0;
                //定义数组长度
                int soureBitArrayLength = data.Height * Math.Abs(data.Stride);
                byte[] sourceBitArray = new byte[soureBitArrayLength];

                //将bitmap中的内容拷贝到ptr_bgr数组中
                Marshal.Copy(ptr, sourceBitArray, 0, soureBitArrayLength);

                //填充引用对象字段值
                imageInfo.width = data.Width;
                imageInfo.height = data.Height;
                imageInfo.format = ASF_ImagePixelFormat.ASVL_PAF_RGB24_B8G8R8;
                //获取去除对齐位后度图像数据
                int line = imageInfo.width * 3;
                int pitch = Math.Abs(data.Stride);
                int bgr_len = line * imageInfo.height;
                byte[] destBitArray = new byte[bgr_len];
                for (int i = 0; i < imageInfo.height; ++i)
                {
                    Array.Copy(sourceBitArray, i * pitch, destBitArray, i * line, line);
                }
                imageInfo.imgData = Marshal.AllocHGlobal(destBitArray.Length);
                Marshal.Copy(destBitArray, 0, imageInfo.imgData, destBitArray.Length);
                return imageInfo;
            }
            catch
            {
                imageInfo.imgData = IntPtr.Zero;
                imageInfo.format = 0;
                imageInfo.height = 0;
                imageInfo.width = 0;
                return imageInfo;
            }
            finally
            {
                bm.UnlockBits(data);
            }
        }

        public static Image DrawFacesOnImage(Image image, List<MarkFaceInfor> faceInfor)
        {
            if ((faceInfor == null) || (faceInfor.Count < 1))
            {
                return image;
            }
            else
            {
                Image clone = (Image)image.Clone();
                Graphics g = Graphics.FromImage(clone);
                try
                {
                    Brush brush = new SolidBrush(Color.YellowGreen);
                    Pen pen = new Pen(brush, 3);
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    foreach (var f in faceInfor)
                    {
                        g.DrawRectangle(pen, new Rectangle(f.left < 1 ? 0 : f.left, f.top < 1 ? 0 : f.top, f.width, f.height));
                        if (f.name != null && f.alive != null)
                        {
                            g.DrawString(string.Format("{0}, {1}", f.name, f.alive), new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Black), f.left < 1 ? 0 : f.left, (f.top - 20) < 1 ? 0 : f.top - 30);
                        }
                        else if (f.name != null)
                        {
                            g.DrawString(f.name, new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Black), f.left < 1 ? 0 : f.left, (f.top - 20) < 1 ? 0 : f.top - 30);
                        }
                        else if (f.alive != null)
                        {
                            g.DrawString(f.alive, new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Black), f.left < 1 ? 0 : f.left, (f.top - 20) < 1 ? 0 : f.top - 30);
                        }                                               
                    }
                    return clone;
                }
                catch 
                {
                    return image;
                }
                finally
                {
                    g.Dispose();
                }
            }
        }

        public static Image DrawFacesOnTransparentImage(int width, int height, List<MarkFaceInfor> faceInfor)
        {
            if ((faceInfor == null) || (faceInfor.Count < 1))
            {
                return null;
            }
            else
            {
                Bitmap transparentBitmap = new Bitmap(width, height);
                transparentBitmap.MakeTransparent();
                Graphics g = Graphics.FromImage(transparentBitmap);
                try
                {
                    Brush brush = new SolidBrush(Color.YellowGreen);
                    Pen pen = new Pen(brush, 3);
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    foreach (var f in faceInfor)
                    {
                        g.DrawRectangle(pen, new Rectangle(f.left < 1 ? 0 : f.left, f.top < 1 ? 0 : f.top, f.width, f.height));
                        if (f.name != null && f.alive != null)
                        {
                            g.DrawString(string.Format("{0}, {1}", f.name, f.alive), new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Black), f.left < 1 ? 0 : f.left, (f.top - 20) < 1 ? 0 : f.top - 30);
                        }
                        else if (f.name != null)
                        {
                            g.DrawString(f.name, new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Black), f.left < 1 ? 0 : f.left, (f.top - 20) < 1 ? 0 : f.top - 30);
                        }
                        else if (f.alive != null)
                        {
                            g.DrawString(f.alive, new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Black), f.left < 1 ? 0 : f.left, (f.top - 20) < 1 ? 0 : f.top - 30);
                        }
                        else if (f.faceID >=0)
                        {
                            g.DrawString("Face_ID:"+f.faceID.ToString(), new Font(FontFamily.GenericSerif, 12), new SolidBrush(Color.Green), f.left < 1 ? 0 : f.left, (f.top - 20) < 1 ? 0 : f.top - 30);
                        }
                    }
                    return transparentBitmap;
                }
                catch
                {
                    return null;
                }
                finally
                {
                    g.Dispose();
                }
            }
        }

        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    stream.Position = 0;
                    BitmapImage result = new BitmapImage();
                    result.BeginInit();
                    result.CacheOption = BitmapCacheOption.OnLoad;
                    result.StreamSource = stream;
                    result.EndInit();
                    result.Freeze();
                    return result;
                }
            }
            else
            {
                return null;
            }
        }

        public static byte[] ReadBMPFormJPG(string imagePath, ref int width, ref int height, ref int pitch)
        {
            Bitmap image = new Bitmap(imagePath);
            //将Bitmap锁定到系统内存中,获得BitmapData
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行
            IntPtr ptr = data.Scan0;
            //定义数组长度
            int soureBitArrayLength = data.Height * Math.Abs(data.Stride);
            byte[] sourceBitArray = new byte[soureBitArrayLength];
            //将bitmap中的内容拷贝到ptr_bgr数组中
            Marshal.Copy(ptr, sourceBitArray, 0, soureBitArrayLength); width = data.Width;
            height = data.Height;
            pitch = Math.Abs(data.Stride);
            int line = width * 3;
            int bgr_len = line * height;
            byte[] destBitArray = new byte[bgr_len];
            for (int i = 0; i < height; ++i)
            {
                Array.Copy(sourceBitArray, i * pitch, destBitArray, i * line, line);
            }
            pitch = line;
            image.UnlockBits(data);
            return destBitArray;
        }

        public static ImageInfo ReadBMPFormJPG(string imagePath)
        {
            ImageInfo imageInfo = new ImageInfo();
            Image image = Image.FromFile(imagePath);
            if (image.Width % 4 != 0)
            {
                image = ScaleImage(image, image.Width - (image.Width % 4), image.Height);
            }
            Bitmap bitmapImage = new Bitmap(image);
            BitmapData data = bitmapImage.LockBits(new Rectangle(0, 0, bitmapImage.Width, bitmapImage.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            try
            {
                //位图中第一个像素数据的地址。它也可以看成是位图中的第一个扫描行
                IntPtr ptr = data.Scan0;

                //定义数组长度
                int soureBitArrayLength = data.Height * Math.Abs(data.Stride);
                byte[] sourceBitArray = new byte[soureBitArrayLength];

                //将bitmap中的内容拷贝到ptr_bgr数组中
                Marshal.Copy(ptr, sourceBitArray, 0, soureBitArrayLength);

                //填充引用对象字段值
                imageInfo.width = data.Width;
                imageInfo.height = data.Height;
                imageInfo.format = ASF_ImagePixelFormat.ASVL_PAF_RGB24_B8G8R8;

                //获取去除对齐位后度图像数据
                int line = imageInfo.width * 3;
                int pitch = Math.Abs(data.Stride);
                int bgr_len = line * imageInfo.height;
                byte[] destBitArray = new byte[bgr_len];

                /*
                 * 图片像素数据在内存中是按行存储，一般图像库都会有一个内存对齐，在每行像素的末尾位置
                 * 每行的对齐位会使每行多出一个像素空间（三通道如RGB会多出3个字节，四通道RGBA会多出4个字节）
                 * 以下循环目的是去除每行末尾的对齐位，将有效的像素拷贝到新的数组
                 */
                for (int i = 0; i < imageInfo.height; ++i)
                {
                    Array.Copy(sourceBitArray, i * pitch, destBitArray, i * line, line);
                }

                imageInfo.imgData = Marshal.AllocHGlobal(destBitArray.Length);
                Marshal.Copy(destBitArray, 0, imageInfo.imgData, destBitArray.Length);

                return imageInfo;
            }
            catch
            {
                return imageInfo;
            }
            finally
            {
                bitmapImage.UnlockBits(data);
            }
        }

        public static Image ScaleImage(Image image, int dstWidth, int dstHeight)
        {
            Graphics g = null;
            try
            {
                //按比例缩放           
                float scaleRate = GetWidthAndHeight(image.Width, image.Height, dstWidth, dstHeight);
                int width = (int)(image.Width * scaleRate);
                int height = (int)(image.Height * scaleRate);

                //将宽度调整为4的整数倍
                if (width % 4 != 0)
                {
                    width = width - width % 4;
                }

                Bitmap destBitmap = new Bitmap(width, height);
                g = Graphics.FromImage(destBitmap);
                g.Clear(Color.Transparent);

                //设置画布的描绘质量         
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, new Rectangle((width - width) / 2, (height - height) / 2, width, height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);

                //设置压缩质量     
                EncoderParameters encoderParams = new EncoderParameters();
                long[] quality = new long[1];
                quality[0] = 100;
                EncoderParameter encoderParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                encoderParams.Param[0] = encoderParam;

                return destBitmap;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (g != null)
                {
                    g.Dispose();
                }
            }

            return null;
        }

        public static float GetWidthAndHeight(int oldWidth, int oldHeigt, int newWidth, int newHeight)
        {
            //按比例缩放           
            float scaleRate = 0.0f;
            if (oldWidth >= newWidth && oldHeigt >= newHeight)
            {
                int widthDis = oldWidth - newWidth;
                int heightDis = oldHeigt - newHeight;
                if (widthDis > heightDis)
                {
                    scaleRate = newWidth * 1f / oldWidth;
                }
                else
                {
                    scaleRate = newHeight * 1f / oldHeigt;
                }
            }
            else if (oldWidth >= newWidth && oldHeigt < newHeight)
            {
                scaleRate = newWidth * 1f / oldWidth;
            }
            else if (oldWidth < newWidth && oldHeigt >= newHeight)
            {
                scaleRate = newHeight * 1f / oldHeigt;
            }
            else
            {
                int widthDis = newWidth - oldWidth;
                int heightDis = newHeight - oldHeigt;
                if (widthDis > heightDis)
                {
                    scaleRate = newHeight * 1f / oldHeigt;
                }
                else
                {
                    scaleRate = newWidth * 1f / oldWidth;
                }
            }
            return scaleRate;
        }

        public static IntPtr GetBMP_Ptr(string imagePath, out int width, out int height, out int pitch)
        {
            width = -1;
            height = -1;
            pitch = -1;
            byte[] imageData = ReadBMPFormJPG(imagePath, ref width, ref height, ref pitch);
            IntPtr imageDataPtr = Marshal.AllocHGlobal(imageData.Length);
            Marshal.Copy(imageData, 0, imageDataPtr, imageData.Length);
            return imageDataPtr;
        }
    }
}

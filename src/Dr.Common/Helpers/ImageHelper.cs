
#region ImageHelper 声明

/**************************************************************
* 命名空间 ：Dr.Common.Helpers
* 类 名 称 ：ImageHelper
* 创 建 人 ：XIAOXL084520
* 邮    箱 ：Xiaoxl084520@163.com
* 创建时间 ：2019-3-30 15:34:46
* 版 本 号 ：V1.0
* 功能描述 ：N/A
* 
*┌ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┐
*│　 Copyright (c) 2019 XIAOXL084520 DR.All rights reserved.   │
*└ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ─ ┘
*
****************************************************************/

#endregion

using Dr.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Dr.Common.Helpers
{
    /// <summary> 
    /// 图片帮助类
    /// </summary> 
    public static class ImageHelper
    {
        #region 正方型裁剪并缩放

        /// <summary>
        /// 正方型裁剪
        /// 以图片中心为轴心，截取正方型，然后等比缩放
        /// 用于头像处理
        /// </summary>
        /// <param name="imagePath">原始图片地址</param>
        /// <param name="savePath">缩略图存放地址</param>
        /// <param name="side">指定的边长（正方型）</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForSquare(this string imagePath, string savePath, int side, int quality = 100)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException($"未找到图片：{imagePath}");
            }

            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            Image initImage = Image.FromFile(imagePath, true);

            initImage.CutForSquare(savePath, side, quality);
        }

        /// <summary>
        /// 正方型裁剪
        /// 以图片中心为轴心，截取正方型，然后等比缩放
        /// 用于头像处理
        /// </summary>
        /// <param name="stream">原始图片</param>
        /// <param name="savePath">缩略图存放地址</param>
        /// <param name="side">指定的边长（正方型）</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForSquare(this Stream stream, string savePath, int side, int quality = 100)
        {
            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            Image initImage = Image.FromStream(stream, true);

            initImage.CutForSquare(savePath, side, quality);
        }

        /// <summary>
        /// 正方型裁剪
        /// 以图片中心为轴心，截取正方型，然后等比缩放
        /// 用于头像处理
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="savePath">缩略图存放地址</param>
        /// <param name="side">指定的边长（正方型）</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForSquare(this Image image, string savePath, int side, int quality = 100)
        {
            try
            {
                //创建目录
                string dir = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                //原图宽高均小于模版，不作处理，直接保存
                if (image.Width <= side && image.Height <= side)
                {
                    image.Save(savePath, ImageFormat.Jpeg);
                }
                else
                {
                    //原始图片的宽、高
                    int initWidth = image.Width;
                    int initHeight = image.Height;

                    //非正方型先裁剪为正方型
                    if (initWidth != initHeight)
                    {
                        //截图对象
                        Image pickedImage = null;
                        Graphics pickedG = null;

                        //宽大于高的横图
                        if (initWidth > initHeight)
                        {
                            //对象实例化
                            pickedImage = new Bitmap(initHeight, initHeight);
                            pickedG = Graphics.FromImage(pickedImage);
                            //设置质量
                            pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            //定位
                            Rectangle fromR = new Rectangle((initWidth - initHeight) / 2, 0, initHeight, initHeight);
                            Rectangle toR = new Rectangle(0, 0, initHeight, initHeight);
                            //画图
                            pickedG.DrawImage(image, toR, fromR, GraphicsUnit.Pixel);
                            //重置宽
                            initWidth = initHeight;
                        }
                        //高大于宽的竖图
                        else
                        {
                            //对象实例化
                            pickedImage = new Bitmap(initWidth, initWidth);
                            pickedG = Graphics.FromImage(pickedImage);
                            //设置质量
                            pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                            //定位
                            Rectangle fromR = new Rectangle(0, (initHeight - initWidth) / 2, initWidth, initWidth);
                            Rectangle toR = new Rectangle(0, 0, initWidth, initWidth);
                            //画图
                            pickedG.DrawImage(image, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                            //重置高
                            initHeight = initWidth;
                        }

                        //将截图对象赋给原图
                        image = (Image)pickedImage.Clone();
                        //释放截图资源
                        pickedG.Dispose();
                        pickedImage.Dispose();
                    }

                    //缩略图对象
                    Image resultImage = new Bitmap(side, side);
                    Graphics resultG = Graphics.FromImage(resultImage);
                    //设置质量
                    resultG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    resultG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    //用指定背景色清空画布
                    resultG.Clear(Color.White);
                    //绘制缩略图
                    resultG.DrawImage(image, new Rectangle(0, 0, side, side), new System.Drawing.Rectangle(0, 0, initWidth, initHeight), System.Drawing.GraphicsUnit.Pixel);

                    //关键质量控制
                    //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                    ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo ici = null;
                    foreach (ImageCodecInfo i in icis)
                    {
                        if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                        {
                            ici = i;
                        }
                    }
                    EncoderParameters ep = new EncoderParameters(1);
                    ep.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                    //保存缩略图
                    resultImage.Save(savePath, ici, ep);

                    //释放关键质量控制所用资源
                    ep.Dispose();

                    //释放缩略图资源
                    resultG.Dispose();
                    resultImage.Dispose();
                }
            }
            finally
            {
                //释放原始图片资源
                image?.Dispose();
            }
        }

        #endregion

        #region 固定模版裁剪并缩放

        /// <summary>
        /// 指定长宽裁剪
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        /// <param name="imagePath">原始图片地址</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="maxWidth">最大宽(单位:px)</param>
        /// <param name="maxHeight">最大高(单位:px)</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForCustom(this string imagePath, string savePath, int maxWidth, int maxHeight, int quality = 100)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException($"未找到图片：{imagePath}");
            }

            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            Image initImage = Image.FromFile(imagePath, true);

            initImage.CutForCustom(savePath, maxWidth, maxHeight, quality);
        }

        /// <summary>
        /// 指定长宽裁剪
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        /// <param name="stream">原始图片</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="maxWidth">最大宽(单位:px)</param>
        /// <param name="maxHeight">最大高(单位:px)</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForCustom(this Stream stream, string savePath, int maxWidth, int maxHeight, int quality = 100)
        {
            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            Image initImage = Image.FromStream(stream, true);

            initImage.CutForCustom(savePath, maxWidth, maxHeight, quality);
        }

        /// <summary>
        /// 指定长宽裁剪
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="maxWidth">最大宽(单位:px)</param>
        /// <param name="maxHeight">最大高(单位:px)</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForCustom(this Image image, string savePath, int maxWidth, int maxHeight, int quality = 100)
        {
            try
            {
                //创建目录
                string dir = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                //原图宽高均小于模版，不作处理，直接保存
                if (image.Width <= maxWidth && image.Height <= maxHeight)
                {
                    image.Save(savePath, ImageFormat.Jpeg);
                }
                else
                {
                    //模版的宽高比例
                    double templateRate = (double)maxWidth / maxHeight;
                    //原图片的宽高比例
                    double initRate = (double)image.Width / image.Height;

                    //原图与模版比例相等，直接缩放
                    if (templateRate == initRate)
                    {
                        //按模版大小生成最终图片
                        Image templateImage = new Bitmap(maxWidth, maxHeight);
                        Graphics templateG = Graphics.FromImage(templateImage);
                        templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                        templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        templateG.Clear(Color.White);
                        templateG.DrawImage(image, new Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.GraphicsUnit.Pixel);
                        templateImage.Save(savePath, ImageFormat.Jpeg);
                    }
                    //原图与模版比例不等，裁剪后缩放
                    else
                    {
                        //裁剪对象
                        Image pickedImage = null;
                        Graphics pickedG = null;

                        //定位
                        Rectangle fromR = new Rectangle(0, 0, 0, 0);//原图裁剪定位
                        Rectangle toR = new Rectangle(0, 0, 0, 0);//目标定位

                        //宽为标准进行裁剪
                        if (templateRate > initRate)
                        {
                            //裁剪对象实例化
                            pickedImage = new Bitmap(image.Width, (int)Math.Floor(image.Width / templateRate));
                            pickedG = Graphics.FromImage(pickedImage);

                            //裁剪源定位
                            fromR.X = 0;
                            fromR.Y = (int)Math.Floor((image.Height - image.Width / templateRate) / 2);
                            fromR.Width = image.Width;
                            fromR.Height = (int)Math.Floor(image.Width / templateRate);

                            //裁剪目标定位
                            toR.X = 0;
                            toR.Y = 0;
                            toR.Width = image.Width;
                            toR.Height = (int)Math.Floor(image.Width / templateRate);
                        }
                        //高为标准进行裁剪
                        else
                        {
                            pickedImage = new Bitmap((int)Math.Floor(image.Height * templateRate), image.Height);
                            pickedG = Graphics.FromImage(pickedImage);

                            fromR.X = (int)Math.Floor((image.Width - image.Height * templateRate) / 2);
                            fromR.Y = 0;
                            fromR.Width = (int)Math.Floor(image.Height * templateRate);
                            fromR.Height = image.Height;

                            toR.X = 0;
                            toR.Y = 0;
                            toR.Width = (int)Math.Floor(image.Height * templateRate);
                            toR.Height = image.Height;
                        }

                        //设置质量
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        //裁剪
                        pickedG.DrawImage(image, toR, fromR, GraphicsUnit.Pixel);

                        //按模版大小生成最终图片
                        Image templateImage = new Bitmap(maxWidth, maxHeight);
                        Graphics templateG = Graphics.FromImage(templateImage);
                        templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                        templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        templateG.Clear(Color.White);
                        templateG.DrawImage(pickedImage, new Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, pickedImage.Width, pickedImage.Height), System.Drawing.GraphicsUnit.Pixel);

                        //关键质量控制
                        //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                        ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                        ImageCodecInfo ici = null;
                        foreach (ImageCodecInfo i in icis)
                        {
                            if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                            {
                                ici = i;
                            }
                        }
                        EncoderParameters ep = new EncoderParameters(1);
                        ep.Param[0] = new EncoderParameter(Encoder.Quality, (long)quality);

                        //保存缩略图
                        templateImage.Save(savePath, ici, ep);
                        //templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);

                        //释放资源
                        templateG.Dispose();
                        templateImage.Dispose();

                        pickedG.Dispose();
                        pickedImage.Dispose();
                    }
                }
            }
            finally
            {
                //释放资源
                image?.Dispose();
            }
        }
        #endregion

        #region 等比缩放

        /// <summary>
        /// 图片等比缩放并添加水印
        /// </summary>
        /// <param name="imagePath">原始图片地址</param>
        /// <param name="savePath">缩略图存放地址</param>
        /// <param name="targetWidth">指定的最大宽度</param>
        /// <param name="targetHeight">指定的最大高度</param>
        /// <param name="watermarkText">水印文字(为null表示不使用水印)</param>
        /// <param name="watermarkImage">水印图片路径(为null表示不使用水印)</param>
        /// <param name="waterTextFont">文字水印字体</param>
        public static void ZoomAuto(this string imagePath, string savePath, double targetWidth, double targetHeight, string watermarkText = null, string watermarkImage = null, Font waterTextFont = null)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException($"未找到图片：{imagePath}");
            }

            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            Image initImage = Image.FromFile(imagePath, true);

            initImage.ZoomAuto(savePath, targetWidth, targetHeight, watermarkText, watermarkImage, waterTextFont);
        }

        /// <summary>
        /// 图片等比缩放并添加水印
        /// </summary>
        /// <param name="stream">原图</param>
        /// <param name="savePath">缩略图存放地址</param>
        /// <param name="targetWidth">指定的最大宽度</param>
        /// <param name="targetHeight">指定的最大高度</param>
        /// <param name="watermarkText">水印文字(为null表示不使用水印)</param>
        /// <param name="watermarkImage">水印图片路径(为null表示不使用水印)</param>
        /// <param name="waterTextFont">文字水印字体</param>
        public static void ZoomAuto(this Stream stream, string savePath, double targetWidth, double targetHeight, string watermarkText = null, string watermarkImage = null, Font waterTextFont = null)
        {
            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            Image initImage = Image.FromStream(stream, true);

            initImage.ZoomAuto(savePath, targetWidth, targetHeight, watermarkText, watermarkImage, waterTextFont);
        }

        /// <summary>
        /// 图片等比缩放并添加水印
        /// </summary>
        /// <param name="image">原图</param>
        /// <param name="savePath">缩略图存放地址</param>
        /// <param name="targetWidth">指定的最大宽度</param>
        /// <param name="targetHeight">指定的最大高度</param>
        /// <param name="waterMarkText">水印文字(为null表示不使用水印)</param>
        /// <param name="waterMarkImage">水印图片路径(为null表示不使用水印)</param>
        /// <param name="waterTextFont">文字水印字体</param>
        public static void ZoomAuto(this Image image, string savePath, double targetWidth, double targetHeight, string waterMarkText = null, string waterMarkImage = null, Font waterTextFont = null)
        {

            try
            {
                //创建目录
                string dir = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                if (waterTextFont == null)
                {
                    waterTextFont = new Font("黑体", 14, GraphicsUnit.Pixel);
                }

                //原图宽高均小于模版，不作处理，直接保存
                if (image.Width <= targetWidth && image.Height <= targetHeight)
                {
                    //文字水印
                    if (!string.IsNullOrWhiteSpace(waterMarkText))
                    {
                        using (Graphics gWater = Graphics.FromImage(image))
                        {
                            Font fontWater = waterTextFont;
                            Brush brushWater = new SolidBrush(Color.White);
                            gWater.DrawString(waterMarkText, fontWater, brushWater, 10, 10);
                        }
                    }

                    //透明图片水印
                    if (!string.IsNullOrWhiteSpace(waterMarkImage))
                    {
                        if (File.Exists(waterMarkImage))
                        {
                            //获取水印图片
                            using (Image wrImage = Image.FromFile(waterMarkImage))
                            {
                                //水印绘制条件：原始图片宽高均大于或等于水印图片
                                if (image.Width >= wrImage.Width && image.Height >= wrImage.Height)
                                {
                                    using (Graphics gWater = Graphics.FromImage(image))
                                    {
                                        //透明属性
                                        ImageAttributes imgAttributes = new ImageAttributes();
                                        ColorMap colorMap = new ColorMap();
                                        colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                                        colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                                        ColorMap[] remapTable = { colorMap };
                                        imgAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                                        float[][] colorMatrixElements = {
                                           new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                           new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                           new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                           new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},//透明度:0.5
                                           new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                        };

                                        ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                                        imgAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                        gWater.DrawImage(wrImage, new Rectangle(image.Width - wrImage.Width, image.Height - wrImage.Height, wrImage.Width, wrImage.Height), 0, 0, wrImage.Width, wrImage.Height, GraphicsUnit.Pixel, imgAttributes);
                                    }
                                }
                            }
                        }
                    }

                    //保存
                    image.Save(savePath, ImageFormat.Jpeg);
                }
                else
                {
                    //缩略图宽、高计算
                    double newWidth = image.Width;
                    double newHeight = image.Height;

                    //宽大于高或宽等于高（横图或正方）
                    if (image.Width > image.Height || image.Width == image.Height)
                    {
                        //如果宽大于模版
                        if (image.Width > targetWidth)
                        {
                            //宽按模版，高按比例缩放
                            newWidth = targetWidth;
                            newHeight = image.Height * (targetWidth / image.Width);
                        }
                    }
                    //高大于宽（竖图）
                    else
                    {
                        //如果高大于模版
                        if (image.Height > targetHeight)
                        {
                            //高按模版，宽按比例缩放
                            newHeight = targetHeight;
                            newWidth = image.Width * (targetHeight / image.Height);
                        }
                    }

                    //生成新图
                    //新建一个bmp图片
                    Image newImage = new Bitmap((int)newWidth, (int)newHeight);
                    //新建一个画板
                    Graphics newG = Graphics.FromImage(newImage);

                    //设置质量
                    newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    //置背景色
                    newG.Clear(Color.White);
                    //画图
                    newG.DrawImage(image, new Rectangle(0, 0, newImage.Width, newImage.Height), new System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.GraphicsUnit.Pixel);

                    //文字水印
                    if (!string.IsNullOrWhiteSpace(waterMarkText))
                    {
                        using (Graphics gWater = Graphics.FromImage(newImage))
                        {
                            Font fontWater = waterTextFont;
                            Brush brushWater = new SolidBrush(Color.White);
                            gWater.DrawString(waterMarkText, fontWater, brushWater, 10, 10);
                        }
                    }

                    //透明图片水印
                    if (!string.IsNullOrWhiteSpace(waterMarkImage))
                    {
                        if (File.Exists(waterMarkImage))
                        {
                            //获取水印图片
                            using (Image wrImage = Image.FromFile(waterMarkImage))
                            {
                                //水印绘制条件：原始图片宽高均大于或等于水印图片
                                if (newImage.Width >= wrImage.Width && newImage.Height >= wrImage.Height)
                                {
                                    using (Graphics gWater = Graphics.FromImage(newImage))
                                    {
                                        //透明属性
                                        ImageAttributes imgAttributes = new ImageAttributes();
                                        ColorMap colorMap = new ColorMap();
                                        colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                                        colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                                        ColorMap[] remapTable = { colorMap };
                                        imgAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                                        float[][] colorMatrixElements = {
                                       new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                       new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                       new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                       new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},//透明度:0.5
                                       new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                    };

                                        ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                                        imgAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                        gWater.DrawImage(wrImage, new Rectangle(newImage.Width - wrImage.Width, newImage.Height - wrImage.Height, wrImage.Width, wrImage.Height), 0, 0, wrImage.Width, wrImage.Height, GraphicsUnit.Pixel, imgAttributes);
                                    }
                                }
                            }
                        }
                    }

                    //保存缩略图
                    newImage.Save(savePath, ImageFormat.Jpeg);

                    //释放资源
                    newG.Dispose();
                    newImage.Dispose();
                    
                }
            }
            finally
            {
                image?.Dispose();
            }
        }

        #endregion

        #region 添加水印

        /// <summary>
        /// 添加水印
        /// </summary>
        /// <param name="imagePath">原始图片地址</param>
        /// <param name="savePath">图片保存地址</param>
        /// <param name="imageFormat">图片保存格式</param>
        /// <param name="waterMarkText">水印文字</param>
        /// <param name="waterMarkImage">水印图片地址</param>
        /// <param name="startPointX">水印坐标X</param>
        /// <param name="startPointY">水印坐标Y</param>
        /// <param name="waterTextFont">水印文字字体</param>
        public static void AddWaterMark(this string imagePath, string savePath, ImageFormat imageFormat = null, string waterMarkText = null, string waterMarkImage = null, float startPointX = 10, float startPointY = 10, Font waterTextFont = null)
        {

            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException($"未找到图片：{imagePath}");
            }

            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            Image initImage = Image.FromFile(imagePath, true);

            initImage.AddWaterMark(savePath, imageFormat, waterMarkText, waterMarkImage, startPointX, startPointY, waterTextFont);

        }

        /// <summary>
        /// 添加水印
        /// </summary>
        /// <param name="stream">原始图片</param>
        /// <param name="savePath">图片保存地址</param>
        /// <param name="imageFormat">图片保存格式</param>
        /// <param name="waterMarkText">水印文字</param>
        /// <param name="waterMarkImage">水印图片地址</param>
        /// <param name="startPointX">水印坐标X</param>
        /// <param name="startPointY">水印坐标Y</param>
        /// <param name="waterTextFont">水印文字字体</param>
        public static void AddWaterMark(this Stream stream, string savePath, ImageFormat imageFormat = null, string waterMarkText = null, string waterMarkImage = null, float startPointX = 10, float startPointY = 10, Font waterTextFont = null)
        {
            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            Image initImage = Image.FromStream(stream, true);

            initImage.AddWaterMark(savePath, imageFormat, waterMarkText, waterMarkImage, startPointX, startPointY, waterTextFont);

        }

        /// <summary>
        /// 添加水印
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="savePath">图片保存地址</param>
        /// <param name="imageFormat">图片保存格式</param>
        /// <param name="waterMarkText">水印文字</param>
        /// <param name="waterMarkImage">水印图片地址</param>
        /// <param name="startPointX">水印坐标X</param>
        /// <param name="startPointY">水印坐标Y</param>
        /// <param name="waterTextFont">水印文字字体</param>
        public static void AddWaterMark(this Image image, string savePath, ImageFormat imageFormat = null, string waterMarkText = null, string waterMarkImage = null, float startPointX = 10, float startPointY = 10, Font waterTextFont = null)
        {

            try
            {
                //创建目录
                string dir = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if (waterTextFont == null)
                {
                    waterTextFont = new Font("黑体", 14, GraphicsUnit.Pixel);
                }
                //文字水印
                if (!string.IsNullOrWhiteSpace(waterMarkText))
                {
                    using (Graphics gWater = Graphics.FromImage(image))
                    {
                        Font fontWater = waterTextFont;
                        Brush brushWater = new SolidBrush(Color.White);
                        gWater.DrawString(waterMarkText, fontWater, brushWater, 10, 10);
                    }
                }

                //透明图片水印
                if (!string.IsNullOrWhiteSpace(waterMarkImage))
                {
                    if (File.Exists(waterMarkImage))
                    {
                        //获取水印图片
                        using (Image wrImage = Image.FromFile(waterMarkImage))
                        {
                            //水印绘制条件：原始图片宽高均大于或等于水印图片
                            if (image.Width >= wrImage.Width && image.Height >= wrImage.Height)
                            {
                                using (Graphics gWater = Graphics.FromImage(image))
                                {
                                    //透明属性
                                    ImageAttributes imgAttributes = new ImageAttributes();
                                    ColorMap colorMap = new ColorMap();
                                    colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                                    colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                                    ColorMap[] remapTable = { colorMap };
                                    imgAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                                    float[][] colorMatrixElements = {
                                           new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                           new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                           new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                           new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},//透明度:0.5
                                           new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                        };

                                    ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                                    imgAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                    gWater.DrawImage(wrImage, new Rectangle(image.Width - wrImage.Width, image.Height - wrImage.Height, wrImage.Width, wrImage.Height), 0, 0, wrImage.Width, wrImage.Height, GraphicsUnit.Pixel, imgAttributes);
                                }
                            }
                        }
                    }
                }
                //保存
                image.Save(savePath, imageFormat ?? ImageFormat.Jpeg);

            }
            finally
            {
                image?.Dispose();
            }
        }

        #endregion

        #region 获取指定区域的图片
        /// <summary>
        /// 获取指定区域的图片
        /// </summary>
        /// <param name="stream">原始图片</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="pPartStartPointX">目标图片开始绘制处的坐标X值(通常为0)</param>
        /// <param name="pPartStartPointY">目标图片开始绘制处的坐标Y值(通常为0)</param>
        /// <param name="pPartWidth">目标图片的宽度</param>
        /// <param name="pPartHeight">目标图片的高度</param>
        /// <param name="pOrigStartPointX">原始图片开始截取处的坐标X值</param>
        /// <param name="pOrigStartPointY">原始图片开始截取处的坐标Y值</param>
        public static void GetPartImage(this Stream stream, string savePath, int pPartStartPointX, int pPartStartPointY, int pPartWidth, int pPartHeight, int pOrigStartPointX, int pOrigStartPointY)
        {

            try
            {
                //创建目录
                string dir = Path.GetDirectoryName(savePath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                if (File.Exists(savePath))
                {
                    File.SetAttributes(savePath, FileAttributes.Normal);
                    File.Delete(savePath);
                }
                using (Image originalImg = Image.FromStream(stream))
                {
                    Bitmap partImg = new Bitmap(pPartWidth, pPartHeight, PixelFormat.Format32bppArgb);

                    Graphics graphics = Graphics.FromImage(partImg);
                    Rectangle destRect = new Rectangle(new Point(pPartStartPointX, pPartStartPointY), new Size(pPartWidth, pPartHeight));//目标位置
                    Rectangle origRect = new Rectangle(new Point(pOrigStartPointX, pOrigStartPointY), new Size(pPartWidth, pPartHeight));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）

                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

                    graphics.DrawImage(originalImg, destRect, origRect, GraphicsUnit.Pixel);

                    using (EncoderParameters eps = new EncoderParameters(1))
                    {
                        eps.Param[0] = new EncoderParameter(Encoder.Quality, 100);
                        partImg.Save(savePath, ImageFormat.Jpeg);
                    }
                }

            }
            finally
            {
                stream?.Dispose();
            }
        }
        #endregion

        #region 判断文件类型是否为WEB格式图片
        /// <summary>
        /// 判断文件类型是否为WEB格式图片
        /// (注：JPG,GIF,BMP,PNG)
        /// </summary>
        /// <param name="contentType">HttpPostedFile.ContentType</param>
        /// <returns></returns>
        public static bool IsWebImage(this string contentType)
        {
            var imgExtensionList = new List<string>()
            {
                "image/pjpeg",
                 "image/jpeg",
                  "image/gif",
                  "image/bmp",
                  "image/png"
            };

            return imgExtensionList.Exists(m => m.ToLower() == contentType.ToLower());
        }

        #endregion

        #region 图片与BASE64字符串转换

        /// <summary>
        /// Base64图片字符串转图片
        /// </summary>
        /// <param name="base64Str">Base64图片字符串</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="imageFormat">图片类型</param>
        public static void GetImageFromBase64(this string base64Str, string savePath, ImageFormat imageFormat = null)
        {
            if (string.IsNullOrWhiteSpace(base64Str))
            {
                return;
            }
            //创建目录
            string dir = Path.GetDirectoryName(savePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var bytes = Convert.FromBase64String(base64Str);
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                using (Image image = Image.FromStream(ms))
                {
                    image.Save(savePath, imageFormat ?? ImageFormat.Jpeg);
                }
            }
        }

        /// <summary>
        /// 图片转Base64字符串
        /// </summary>
        /// <param name="imagePath">图片地址</param>
        /// <param name="imageFormat">图片类型</param>
        /// <returns></returns>
        public static string GetBase64FromImage(this string imagePath, ImageFormat imageFormat = null)
        {
            var result = string.Empty;
            if (imagePath.IsNullOrWhiteSpace())
            {
                return result;
            }
            using (Image image = Image.FromFile(imagePath))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, imageFormat ?? ImageFormat.Jpeg);
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);
                    result = Convert.ToBase64String(arr);
                }
            }
            return result;
        }

        #endregion
    }
}

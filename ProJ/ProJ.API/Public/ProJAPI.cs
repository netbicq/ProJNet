using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using ProJ.Bll;
using ProJ.Model.DB;

namespace ProJ.API.Public
{
    public class ProJAPI: ApiController
    {
        /// <summary>
        /// 当前操作员
        /// </summary>
        public CurrentUser CurrentUser { get; set; }
        /// <summary>
        /// 业务类
        /// </summary>
        public object BusinessService { get; set; }

        [NonAction]
        public void SetService()
        {
            ServiceBase obj = BusinessService as ServiceBase;
            if(CurrentUser !=null)
            {
                obj.AppUser = new AppServiceUser {
                     OutPutPaht =OutPutPath,
                      UploadPath =uploadPath,
                        CurrentUserInfo =CurrentUser
                };
                
            }
        }
        /// <summary>
        /// 上传文件地址
        /// </summary>
        protected string uploadPath = HttpContext.Current.Server.MapPath("~/uploads/");
        /// <summary>
        /// 导出临时文件夹
        /// </summary>
        protected string OutPutPath = HttpContext.Current.Server.MapPath("~/OutPutTemp/");

    }
    public class DrawingHelper
    {
        /// <summary>
        /// Resizes an single picture
        /// </summary>
        /// <param name="FileName"></param>
        public static Boolean MakeThumbnailImageFile(String fileName, int maxHeightWidth, String suffix)
        {
            FileInfo inputFile = new FileInfo(fileName);
            int newHeight, newWidth;

            // error checking
            if (!File.Exists(fileName))
                return false;

            // check for invalid files
            if (!(fileName.ToLower().EndsWith(".jpeg")
                || fileName.ToLower().EndsWith(".jpg")
                || fileName.ToLower().EndsWith(".bmp")
                || fileName.ToLower().EndsWith(".gif")
                || fileName.ToLower().EndsWith(".png")))
            {
                return false;
            }

            #region resize picture

            // read in image
            Image inputImage = System.Drawing.Image.FromFile(fileName);
            Image outputImage;

            int x = 0;
            int y = 0;
            int ow = inputImage.Width;
            int oh = inputImage.Height;

            try
            {
                if (suffix == "c")
                {
                    if ((double)inputImage.Width / (double)inputImage.Height > (double)maxHeightWidth / (double)maxHeightWidth)
                    {
                        oh = inputImage.Height;
                        ow = inputImage.Height * maxHeightWidth / maxHeightWidth;
                        y = 0;
                        x = (inputImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = inputImage.Width;
                        oh = inputImage.Width * maxHeightWidth / maxHeightWidth;
                        x = 0;
                        y = (inputImage.Height - oh) / 2;
                    }
                    //新建一个bmp图片 
                    outputImage = new Bitmap(maxHeightWidth, maxHeightWidth);

                    //新建一个画板 
                    Graphics g = Graphics.FromImage(outputImage);
                    g.DrawImage(inputImage, new Rectangle(0, 0, maxHeightWidth, maxHeightWidth),
                        new Rectangle(x, y, ow, oh),
                        GraphicsUnit.Pixel);

                }
                else
                {
                    // find out how the picture is oriented
                    if (inputImage.Width > inputImage.Height)			// landscape-layout
                    {
                        // calculate new height
                        newHeight = (int)((maxHeightWidth * 1.0 / inputImage.Width) * inputImage.Height);
                        newWidth = maxHeightWidth;
                    }
                    else												// portrait-layout
                    {
                        newHeight = maxHeightWidth;

                        // calculate new width
                        newWidth = (int)((maxHeightWidth * 1.0 / inputImage.Height) * inputImage.Width);
                    }
                    // resize picture
                    outputImage = new Bitmap(inputImage, newWidth, newHeight);
                }

            }
            catch (System.Exception)
            {
                outputImage = inputImage;
            }

            #endregion

            // save resized picture in subfolder
            outputImage.Save(inputFile.DirectoryName + "\\" + Path.GetFileName(fileName).Replace(Path.GetExtension(fileName), suffix + Path.GetExtension(fileName)), inputImage.RawFormat);

            // release resources
            outputImage.Dispose();
            inputImage.Dispose();

            return true;
        }

        /// <summary>
        /// Resizes an single picture
        /// </summary>
        public static byte[] MakeThumbnailImageFile(int maxHeightWidth, byte[] data)
        {
            using (var inputStream = new MemoryStream(data))
            {
                using (var inputImage = Image.FromStream(inputStream))
                {
                    try
                    {
                        int newHeight, newWidth;

                        // find out how the picture is oriented
                        if (inputImage.Width > inputImage.Height)			// landscape-layout
                        {
                            // calculate new height
                            newHeight = (int)((maxHeightWidth * 1.0 / inputImage.Width) * inputImage.Height);
                            newWidth = maxHeightWidth;
                        }
                        else												// portrait-layout
                        {
                            newHeight = maxHeightWidth;

                            // calculate new width
                            newWidth = (int)((maxHeightWidth * 1.0 / inputImage.Height) * inputImage.Width);
                        }

                        using (var outputStream = new MemoryStream())
                        {
                            // resize picture
                            new Bitmap(inputImage, newWidth, newHeight).Save(outputStream, inputImage.RawFormat);

                            return outputStream.ToArray();
                        }
                    }
                    catch
                    {
                        return data;
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="LoadImgW">原图宽度</param>
        /// <param name="LoadImgH">原图高度</param>
        /// <param name="width">新宽度</param>
        /// <param name="height">新高度</param>
        /// <returns></returns>
        private static int[] GetImageSize(int LoadImgW, int LoadImgH, int width, int height)
        {
            int xh = 0;
            int xw = 0;
            //容器高与宽
            int oldW = width;
            int oldH = height;
            //图片的高宽与容器的相同
            if (LoadImgH == oldH && LoadImgW == (oldW))
            {//1.正常显示 
                xh = LoadImgH;
                xw = LoadImgW;
            }
            if (LoadImgH == oldH && LoadImgW > (oldW))
            {//2、原高==容高，原宽>容宽 以原宽为基础 
                xw = (oldW);
                xh = LoadImgH * xw / LoadImgW;
            }
            if (LoadImgH == oldH && LoadImgW < (oldW))
            {//3、原高==容高，原宽<容宽  正常显示    
                xw = LoadImgW;
                xh = LoadImgH;
            }
            if (LoadImgH > oldH && LoadImgW == (oldW))
            {//4、原高>容高，原宽==容宽 以原高为基础    
                xh = oldH;
                xw = LoadImgW * xh / LoadImgH;
            }
            if (LoadImgH > oldH && LoadImgW > (oldW))
            {//5、原高>容高，原宽>容宽            
                if ((LoadImgH / oldH) > (LoadImgW / (oldW)))
                {//原高大的多，以原高为基础 
                    xh = oldH;
                    xw = LoadImgW * xh / LoadImgH;
                }
                else
                {//以原宽为基础 
                    xw = (oldW);
                    xh = LoadImgH * xw / LoadImgW;
                }
            }
            if (LoadImgH > oldH && LoadImgW < (oldW))
            {//6、原高>容高，原宽<容宽 以原高为基础         
                xh = oldH;
                xw = LoadImgW * xh / LoadImgH;
            }
            if (LoadImgH < oldH && LoadImgW == (oldW))
            {//7、原高<容高，原宽=容宽 正常显示        
                xh = LoadImgH;
                xw = LoadImgW;
            }
            if (LoadImgH < oldH && LoadImgW > (oldW))
            {//8、原高<容高，原宽>容宽 以原宽为基础     
                xw = (oldW);
                xh = LoadImgH * xw / LoadImgW;
            }
            if (LoadImgH < oldH && LoadImgW < (oldW))
            {//9、原高<容高，原宽<容宽//正常显示     
                xh = LoadImgH;
                xw = LoadImgW;
            }
            return new[] { xw, xh };
        }

    }
}
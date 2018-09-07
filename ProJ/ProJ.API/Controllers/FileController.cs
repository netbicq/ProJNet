using Newtonsoft.Json;
using ProJ.API.Public;
using ProJ.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ProJ.API.Controllers
{
    /// <summary>
    /// 文件上传
    /// </summary>
    [RoutePrefix("api/file")]
    public class FileController : ProJAPI
    {
        /// <summary>
        /// 文件上传，返回服务器文件地址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("post")]
        [AllowAnonymous]
        public async Task<ActionResult<FilePath>> Post()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                {
                    this.Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
                }

                var provider = GetMultipartProvider();
                var result = await Request.Content.ReadAsMultipartAsync(provider);
                string privateUploadPath = uploadPath;
                if (!Directory.Exists(privateUploadPath + "/doc"))
                {
                    Directory.CreateDirectory(privateUploadPath + "/doc");
                }
                if (!Directory.Exists(privateUploadPath + "/img"))
                {
                    Directory.CreateDirectory(privateUploadPath + "/img");
                }

                var dbPath = "";
                foreach (var data in result.FileData)
                {

                    string originalFileName = GetDeserializedFileName(data);
                    var uploadedFileInfo = new FileInfo(data.LocalFileName);

                    //var request = new NoteItemRequest();
                    var newName = Guid.NewGuid() + Path.GetExtension(originalFileName);

                    if (".jpg.jpeg.png.gif.bmp".Contains(Path.GetExtension(originalFileName).ToLower()))
                    {

                        var targetFileName = Path.Combine(privateUploadPath + "/img/", newName);
                        File.Move(data.LocalFileName, targetFileName);

                        // DrawingHelper.MakeThumbnailImageFile(targetFileName, 160, "i");
                        DrawingHelper.MakeThumbnailImageFile(targetFileName, 200, "c");//缩位图
                                                                                       //  DrawingHelper.MakeThumbnailImageFile(targetFileName, 320, "s");
                                                                                       //  DrawingHelper.MakeThumbnailImageFile(targetFileName, 640, "m");

                        dbPath = "~/uploads/img/" + newName;
                    }
                    else
                    {
                        var targetFileName = Path.Combine(privateUploadPath + "/doc/", newName);
                        File.Move(data.LocalFileName, targetFileName);
                        dbPath = "~/uploads/doc/" + newName;

                    }

                }
                return new ActionResult<FilePath>(new FilePath() { filePaht = dbPath });
            }
            catch (System.Exception ex)
            {
                return new ActionResult<FilePath>(ex);
            }
        }

        private MultipartFormDataStreamProvider GetMultipartProvider()
        {

            var uploadFolder = "~/App_Data/Tmp/FileUploads";
            var root = System.Web.HttpContext.Current.Server.MapPath(uploadFolder);
            System.IO.Directory.CreateDirectory(root);
            return new MultipartFormDataStreamProvider(root);

        }

        private string GetDeserializedFileName(MultipartFileData fileData)
        {
            var fileName = GetFileName(fileData);
            return JsonConvert.DeserializeObject(fileName).ToString();
        }

        private string GetFileName(MultipartFileData fileData)
        {
            return fileData.Headers.ContentDisposition.FileName;
        }
    }
    /// <summary>
    /// 服务器文件信息
    /// </summary>
    public class FilePath
    {
        /// <summary>
        /// 上传文件的服务器地址
        /// </summary>
        public string filePaht { get; set; }
    }
}

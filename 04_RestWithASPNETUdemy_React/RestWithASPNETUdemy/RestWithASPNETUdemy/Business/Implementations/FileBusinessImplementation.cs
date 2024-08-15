using Microsoft.AspNetCore.Http;
using RestWithASPNETUdemy.Data.VO;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RestWithASPNETUdemy.Business.Implementations
{
    public class FileBusinessImplementation : IFileBusiness
    {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _httpContext;

        public FileBusinessImplementation(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
            _basePath = Directory.GetCurrentDirectory() + "\\UploadDir\\";
        }

        public byte[] GetFile(string fileName)
        {
            var filePath = _basePath + fileName;
            return File.ReadAllBytes(filePath);
        }

        public async Task<FileDetailVO> SaveFileToDisk(IFormFile formFile)
        {
            FileDetailVO fileDetailVO = new FileDetailVO();

            var fileType = Path.GetExtension(formFile.FileName);// pega apenas a extensao do arquivo enviado.
            var baseUrl = _httpContext.HttpContext.Request.Host; // utiliza a interface IHttpContextAccessor para pegar a url do seu host, no caso aqui da minha máquina

            if (fileType.ToLower() == ".pdf" || fileType.ToLower() == ".jpg" || fileType.ToLower() == ".png" || fileType.ToLower() == ".jpeg")
            {
                var fileName = Path.GetFileName(formFile.FileName); // pega apenas o nome do arquivo enviado.
                if (formFile != null || formFile.Length > 0)
                {
                    var destination = Path.Combine(_basePath, "", fileName);
                    fileDetailVO.DocName = fileName;
                    fileDetailVO.DocType = fileType;
                    fileDetailVO.DocUrl = Path.Combine(baseUrl + "/api/file/v1" + fileDetailVO.DocName);

                    //Gravação no Disco
                    using var stream = new FileStream(destination, FileMode.Create); // Abre um FileStream no Disco em modo de gravação.
                    await formFile.CopyToAsync(stream);
                }

            }
            return fileDetailVO;
        }

        public async Task<List<FileDetailVO>> SaveFilesToDisk(IList<IFormFile> formFiles)
        {
            List<FileDetailVO> listVO = new List<FileDetailVO>();// instancia a lista de VO
            foreach (var file in formFiles)
            {
                listVO.Add(await SaveFileToDisk(file)); // para adiciona arquivo por arquivo no listVO, utilizando o método já implementado SaveFileToDisk.
            }
            return listVO;
        } 
    }
}

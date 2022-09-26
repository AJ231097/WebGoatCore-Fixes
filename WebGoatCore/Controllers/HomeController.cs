using WebGoatCore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebGoatCore.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace WebGoatCore.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ProductRepository _productRepository;

        public HomeController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new HomeViewModel()
            {
                TopProducts = _productRepository.GetTopProducts(4)
            });
        }

        [HttpPost("Index"), DisableRequestSizeLimit]
        public IActionResult UploadFile1()
        {
            ViewBag.Message = "";
            foreach (var formFile in Request.Form.Files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = formFile.OpenReadStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string line = "";
                            while (!reader.EndOfStream)
                            {
                                line += reader.ReadLine();
                            }
                            ViewBag.Message = $"Your details are: {line}";
                        }
                    }
                }
            }
            return View("Index", new HomeViewModel()
            {
                TopProducts = _productRepository.GetTopProducts(4)
            });
        }

        [HttpGet]
        public IActionResult About() => View();

        [HttpPost("About")]
        public async Task<IActionResult> UploadFile(IFormFile FormFile)
        {
            ViewBag.Message = "";
            try
            {
                var path = HttpContextServerVariableExtensions.GetServerVariable(this.HttpContext, "PATH_TRANSLATED");
                string file = Path.GetFileName(FormFile.FileName);
                path = path + "\\..\\wwwroot\\upload\\" + file;
                string [] permittedExtensions = { ".txt", ".pdf" };
                var ext = Path.GetExtension(file).ToLowerInvariant();
                if (string.IsNullOrEmpty(ext) || permittedExtensions.Contains(ext))
                {


                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await FormFile.CopyToAsync(fileStream);
                    }
                    ViewBag.Message = $"File {FormFile.FileName} Uploaded Successfully at /upload";
                }
                else
                {
                    ViewBag.Message = "Please Upload valid Filetype!!";
                }
                return View("About");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel()
                { ExceptionInfo = (IExceptionHandlerPathFeature)ex });
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ExceptionInfo = HttpContext.Features.Get<IExceptionHandlerPathFeature>(),
            });
        }
    }
}

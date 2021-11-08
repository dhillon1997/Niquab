using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NiqabCommonLibrary;

namespace DemoWebCam.Controllers
{
    public class CameraController : Controller
    {
        private readonly IHostingEnvironment _environment;
        public CameraController(IHostingEnvironment hostingEnvironment)
        {
            _environment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Authenticate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string userAlias)
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                var user = new RegisterNewUser();
                var fileProcessor = new FileProcessor();
                if (files != null)
                {
                    var fileMapping = fileProcessor.StoreImage(files, _environment, "CameraPhotos", userAlias);
                    var result = await user.Register(userAlias, fileMapping.GetValueOrDefault("FilePath"));
                    fileProcessor.RemoveFromFolder(fileMapping.GetValueOrDefault("FilePath"));
                    return Json(result);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> Authenticate(string userName)
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                var user = new AuthenticateUser();
                var fileProcessor = new FileProcessor();
                if (files != null)
                {
                   var fileMapping =  fileProcessor.StoreImage(files, _environment,"TestPhotos","test_photo");
                   var result = await user.Authenticate(fileMapping.GetValueOrDefault("FilePath"));
                   fileProcessor.RemoveFromFolder(fileMapping.GetValueOrDefault("FilePath"));
                   return Json(result.FirstOrDefault().Value);
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
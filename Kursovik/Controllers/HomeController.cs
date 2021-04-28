using Kursovik.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Web;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Kursovik.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.text = TempData["tempText"]?.ToString();
            ViewBag.keyError = TempData["keyErrorClass"]?.ToString() ?? "form-control";
            ViewBag.textError = TempData["textErrorClass"]?.ToString() ?? "form-control mb-2 ";
            return View();
        }
        public IActionResult Result()
        {
            ViewBag.text = TempData["tempText"]?.ToString();
            TempData["fileText"] = ViewBag.text;
            return View();
        }

        public IActionResult Download(string resultText)
        {
            string fileName = TempData["fileName"]?.ToString();
            string fileText = TempData["fileText"]?.ToString();
            if (fileName == null)
            {
                return File(FileHelper.CreateTxtFile(fileText), "text/plain", "Результат.txt");
            }
            if (fileName.EndsWith("txt"))
            {
                TempData["fileName"] = fileName;
                TempData["fileText"] = fileText;
                return File(FileHelper.CreateTxtFile(fileText), "text/plain", fileName);
            }
            else
            {
                TempData["fileName"] = fileName;
                TempData["fileText"] = fileText;
                return File(FileHelper.CreateDocFile(fileText), "application/msword", fileName);
            }
        }
        public IActionResult OnPost(IFormFile file, string myText, string key, string ROTcheck, string LanguageCheck, string ActionType)
        {
            if (string.IsNullOrEmpty(myText) && file == null)
            {
                TempData["textErrorClass"] = "form-control mb-2 is-invalid";
                return RedirectToAction("Index");
            }
            string rot = ROTcheck;
            string language = LanguageCheck;
            key = key.ToLower();
            if (language == "rus")
            {
                if (key.Except(EncodingHelper.russianLetter.Keys.ToArray()).Count() != 0)
                {
                    TempData["tempText"] = myText;
                    TempData["keyErrorClass"] = "form-control is-invalid";
                    return RedirectToAction("Index");
                }
            }
            if (language == "eng")
            {
                if (key.Except(EncodingHelper.englishLetter.Keys.ToArray()).Count() != 0)
                {
                    TempData["tempText"] = myText;
                    TempData["keyErrorClass"] = "form-control is-invalid";
                    return RedirectToAction("Index");
                }
            }
            if (file != null)
            {
                using (Stream fileStream = file.OpenReadStream())
                {
                    TempData["fileName"] = file.FileName;
                    if (file.FileName.EndsWith("txt"))
                    {
                        string textFromFile = FileHelper.OpenTxtFile(fileStream);
                        myText = textFromFile;
                    }
                    else
                    {
                        string textFromFile = FileHelper.OpenDocFile(fileStream);
                        myText = textFromFile;
                    }

                }
            }
            EncodingHelper.GenerateVigenereSquare(language, rot);
            if (ActionType == "Расшифровать")
            {
                TempData["tempText"] = EncodingHelper.Decryption(myText, key, language);
            }
            if (ActionType == "Зашифровать")
            {
                TempData["tempText"] = EncodingHelper.Encryption(myText, key, language);
            }
           
            return RedirectToAction("Result");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

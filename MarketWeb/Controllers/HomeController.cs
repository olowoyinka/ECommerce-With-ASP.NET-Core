using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using MarketWeb.Models.ViewModels;
using MarketWeb.Models;
using System.Collections.Generic;

namespace MarketWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _Context;

        private readonly IHttpContextAccessor _contextAccesor;

        public HomeController(ApplicationDbContext context, IHttpContextAccessor contextAccessor)
        {
            _Context = context;
            _contextAccesor = contextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> ValidateCaptcha(IFormCollection collection)
        {
            var response = Request.Form["g-recaptcha-response"];
            const string secret = "6LdrP10UAAAAAF6__kxJM0U-Iafy8lbl9xD_Ns2L";

            var client = new HttpClient();
            string reply = await client.GetStringAsync(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response{1}", secret, response)
                );

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply); ;

            if (!captchaResponse.Success)
            {
                if (captchaResponse.ErrorCodes.Count <= 0) return View("Index");
                var error = captchaResponse.ErrorCodes[0].ToLower();

                switch (error)
                {
                    case "missing-input-secret":
                        ViewBag.Message = "Missing Secret parameter";
                        break;
                    case "invalid-input-secret":
                        ViewBag.Message = "The Secret is Invalid or malformed";
                        break;
                    case "missing-input-response":
                        ViewBag.Message = "Missing Response Parameter";
                        break;
                    case "invalid-input-response":
                        ViewBag.Message = "The Response Parameter is InValid of malformed";
                        break;
                    default:
                        ViewBag.Message = "Error Occured. Please try again later";
                        break;
                }
            }
            else
            {

                ContactUs Contact = new ContactUs()
                {
                    Names = collection["cusName"],
                    Emails = collection["cusEmail"],
                    Phones = collection["cusPhone"],
                    Contents = collection["cusContent"]

                };

                _Context.ContactUs.Add(Contact);
                _Context.SaveChanges();

                ViewBag.Message = "Your Queries Have Been Submitted, Will Get back To You Shortly";
            }

            return View("Index");
        }


        public IActionResult Error()
        {
            return View();
        }
    }
}


using ASM_Nhom2_View.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ASM_Nhom2_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private string url = "https://localhost:44309/api/Brand";
        public BrandController() { }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<BrandVM> categories = new List<BrandVM>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<List<BrandVM>>(apiResponse);
                }
            }
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BrandVM sendBrandVM)
        {
            BrandVM receiveBrandVM = new BrandVM();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync(url, sendBrandVM);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    receiveBrandVM = JsonConvert.DeserializeObject<BrandVM>(apiResponse);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact the administrator.");
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            BrandVM category = new BrandVM();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url + "/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    category = JsonConvert.DeserializeObject<BrandVM>(apiResponse);
                }
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BrandVM sendBrandVM, int id)
        {
            BrandVM receiveBrandVM = new BrandVM();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PutAsJsonAsync(url + "/" + id, sendBrandVM);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    receiveBrandVM = JsonConvert.DeserializeObject<BrandVM>(apiResponse);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact the administrator.");
                }
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            BrandVM category = new BrandVM();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url + "/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    category = JsonConvert.DeserializeObject<BrandVM>(apiResponse);
                }
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync(url + "/" + id);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact the administrator.");
                }
            }
            return View("Index");
        }
    }
}

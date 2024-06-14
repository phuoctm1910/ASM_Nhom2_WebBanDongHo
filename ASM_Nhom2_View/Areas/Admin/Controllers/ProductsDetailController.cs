using ASM_Nhom2_View.Data;
using ASM_Nhom2_View.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ASM_Nhom2_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsDetailController : Controller
    {
            
        private string url = "https://localhost:44309/api/ProductDetails";
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<ProductVM> categories = new List<ProductVM>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<List<ProductVM>>(apiResponse);
                }
            }
            return View(categories);
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductVM category)
        {
            ProductVM categories = new ProductVM();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(category),
                    Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(url, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<ProductVM>(apiResponse);
                }

            }
            if (category == null)
            {
                return View(categories);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ProductVM categories = new ProductVM();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url + "/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<ProductVM>(apiResponse);
                }
            }
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductVM category)
        {
            ProductVM categories = new ProductVM();
            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.PutAsJsonAsync(url + "/" + category.ProductId, category))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<ProductVM>(apiResponse);
                }
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ProductVM categories = new ProductVM();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url + "/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<ProductVM>(apiResponse);
                }
            }
            return View(categories);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(ProductVM category)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(url + "/" + category.ProductId))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View("Error");
                    }
                }
            }
        }
    }
}

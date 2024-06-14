using ASM_Nhom2_API.Model;
using ASM_Nhom2_View.Data;
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
            List<ProductDetailVM> categories = new List<ProductDetailVM>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<List<ProductDetailVM>>(apiResponse);
                }
            }
            return View(categories);
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductDetailVM category)
        {
            ProductDetailVM categories = new ProductDetailVM();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(category),
                    Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(url, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<ProductDetailVM>(apiResponse);
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
            ProductDetailVM categories = new ProductDetailVM();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url + "/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<ProductDetailVM>(apiResponse);
                }
            }
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductDetailVM category)
        {
            ProductDetailVM categories = new ProductDetailVM();
            using (var httpClient = new HttpClient())
            {

                using (var response = await httpClient.PutAsJsonAsync(url + "/" + category.ProductDetailId, category))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<ProductDetailVM>(apiResponse);
                }
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ProductDetailVM categories = new ProductDetailVM();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url + "/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<ProductDetailVM>(apiResponse);
                }
            }
            return View(categories);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(ProductDetailVM category)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(url + "/" + category.ProductDetailId))
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

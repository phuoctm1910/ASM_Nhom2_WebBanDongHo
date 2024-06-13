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
    public class RoleController : Controller
    {
        private string url = "https://localhost:44309/api/Role";

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RoleVM> categories = new List<RoleVM>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<List<RoleVM>>(apiResponse);
                }
            }
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Role category)
        {
            Role category1 = new Role();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(url, content))
                {
                    string apiRes = await response.Content.ReadAsStringAsync();
                    category1 = JsonConvert.DeserializeObject<Role>(apiRes);
                }
            }
            if (category1 != null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(category1);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Role category = new Role();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url + "/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    category = JsonConvert.DeserializeObject<Role>(apiResponse);
                }
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Role sendCategory)
        {
            Role receiveCategory = new Role();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PutAsJsonAsync(url + "/" + sendCategory.RoleId, sendCategory);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    receiveCategory = JsonConvert.DeserializeObject<Role>(apiResponse);
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
            Role category = new Role();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url + "/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    category = JsonConvert.DeserializeObject<Role>(apiResponse);
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
            return RedirectToAction("Index");
        }


    }
}

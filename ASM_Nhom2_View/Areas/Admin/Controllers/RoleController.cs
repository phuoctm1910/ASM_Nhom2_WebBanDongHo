using ASM_Nhom2_View.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ASM_Nhom2_View.Models;

namespace ASM_Nhom2_View.Areas.Admin.Controllers
{

    public class RoleController : Controller
    {
        private readonly string url = "https://localhost:44385/api/Role";

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<Role> categories = new List<Role>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<List<Role>>(apiResponse);
                }
            }
            return View(categories);
        }

        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(Role category)
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
                return RedirectToAction(nameof(GetAll));
            }
            else
            {
                return View(category1);
            }
        }
        [HttpGet]
        public async Task<IActionResult> UpdateRole(int id)
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
        public async Task<IActionResult> UpdateRole(Role sendCategory)
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
            return RedirectToAction("GetAll");
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
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact the administrator.");
                }
            }
            return RedirectToAction("GetAll");
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}

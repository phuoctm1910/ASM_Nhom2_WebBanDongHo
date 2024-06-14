using ASM_Nhom2_View.Data;
using ASM_Nhom2_View.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ASM_Nhom2_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _context;

        public ProductController(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment, AppDbContext context)
        {
            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        // GET: Product
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:44309/api/Product");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductVM>>();
                return View(products);
            }

            return View(new List<ProductVM>());
        }
        [HttpGet]
        // GET: Product/Details/5
        public async Task<IActionResult> Details(int id)
        {

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:44309/api/Product/{id}");
            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadFromJsonAsync<ProductVM>();
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");

                return View(product);
            }
            return NotFound();
        }

        // GET: Product/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public async Task<IActionResult> Create(ProductVM model, List<IFormFile> ProductImages)
        {
            if (ModelState.IsValid)
            {
                List<string> imageNames = new List<string>();

                if (ProductImages != null && ProductImages.Count > 0)
                {
                    foreach (var file in ProductImages)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            imageNames.Add(fileName);
                        }
                    }
                }

                model.ProductImageList = imageNames;
                model.ProductImages = JsonConvert.SerializeObject(imageNames);

                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync("https://localhost:44309/api/product", model);
                if (response.IsSuccessStatusCode)
                {
                    if (ProductImages != null && ProductImages.Count > 0)
                    {
                        foreach (var file in ProductImages)
                        {
                            if (file.Length > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

                                if (!System.IO.File.Exists(filePath))
                                {
                                    using (var stream = new FileStream(filePath, FileMode.Create))
                                    {
                                        await file.CopyToAsync(stream);
                                    }
                                }
                            }
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Server error: {response.StatusCode}, {content}");
                }
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = client.GetAsync($"https://localhost:44309/api/product/{id}").Result;
            if (response.IsSuccessStatusCode)
            {
                var product = response.Content.ReadFromJsonAsync<ProductVM>().Result;
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.ProductId);
                return View(product);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductVM model, List<IFormFile> ProductImages, List<string> imagesToRemove, List<string> imagesNotRemove)
        {
            if (ModelState.IsValid)
            {
                List<string> imageNames = model.ProductImageList ?? new List<string>();

                // Handle image removal
                if (imagesToRemove != null && imagesToRemove.Count > 0)
                {
                    foreach (var image in imagesToRemove)
                    {
                        if (imageNames.Contains(image))
                        {
                            imageNames.Remove(image);
                        }
                    }
                }

                // Add images that were not marked for removal
                if (imagesNotRemove != null && imagesNotRemove.Count > 0)
                {
                    foreach (var image in imagesNotRemove)
                    {
                        if (!imageNames.Contains(image))
                        {
                            imageNames.Add(image);
                        }
                    }
                }

                // Handle new image uploads
                if (ProductImages != null && ProductImages.Count > 0)
                {
                    foreach (var file in ProductImages)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            if (!imageNames.Contains(fileName))
                            {
                                imageNames.Add(fileName);
                            }
                        }
                    }
                }

                model.ProductImageList = imageNames;
                model.ProductImages = JsonConvert.SerializeObject(imageNames);

                var client = _httpClientFactory.CreateClient();
                var patchDoc = new JsonPatchDocument<ProductVM>();

                patchDoc.Replace(e => e.ProductName, model.ProductName);
                patchDoc.Replace(e => e.ProductCode, model.ProductCode);
                patchDoc.Replace(e => e.ProductStock, model.ProductStock);
                patchDoc.Replace(e => e.ProductPrice, model.ProductPrice);
                patchDoc.Replace(e => e.ProductImages, model.ProductImages);
                patchDoc.Replace(e => e.CategoryId, model.CategoryId);

                var serializedPatchDoc = JsonConvert.SerializeObject(patchDoc);
                var content = new StringContent(serializedPatchDoc, Encoding.UTF8, "application/json-patch+json");

                var response = await client.PatchAsync($"https://localhost:44309/api/product/{model.ProductId}", content);
                if (response.IsSuccessStatusCode)
                {
                    // Save new images to the server
                    if (ProductImages != null && ProductImages.Count > 0)
                    {
                        foreach (var file in ProductImages)
                        {
                            if (file.Length > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

                                if (!System.IO.File.Exists(filePath))
                                {
                                    using (var stream = new FileStream(filePath, FileMode.Create))
                                    {
                                        await file.CopyToAsync(stream);
                                    }
                                }
                            }
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var contentString = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Server error: {response.StatusCode}, {contentString}");
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:44309/api/Product/{id}");
            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadFromJsonAsync<ProductVM>();
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
                return View(product);
            }
            return NotFound();
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"https://localhost:44309/api/Product/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

    }
}

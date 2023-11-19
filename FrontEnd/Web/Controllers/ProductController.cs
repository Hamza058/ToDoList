﻿using Azure;
using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Collections.Generic;
using Web.Models;
using Web.Service.IService;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _service;
        CategoryManager cm = new CategoryManager(new EFCategoryDal());

        public ProductController(IProductService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            List<Product>? list = new();
            var response = await _service.GetProductsAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<Product>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

		public async Task<IActionResult> ProductDelete(int id)
		{
			ResponseDto? response = await _service.DeleteProductAsync(id);

			if (response != null && response.IsSuccess)
			{
				TempData["success"] = "Product deleted successfully";
				return RedirectToAction(nameof(Index));
			}

			else
			{
				TempData["error"] = response?.Message;
			}

			return NotFound();
		}
        [HttpGet]
        public async Task<IActionResult> ProductCreate()
        {
            ViewBag.category = cm.TGetList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(Product product)
        {
            ResponseDto? response = await _service.CreateProductAsync(product);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product created successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(product);
        }
    }
}

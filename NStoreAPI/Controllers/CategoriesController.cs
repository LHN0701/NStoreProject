using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategory _iCategory;

        public CategoriesController(ICategory iCategory)
        {
            _iCategory = iCategory;
        }

        [HttpPost("GetAll")]
        public IActionResult GetAll()
        {
            var categories = _iCategory.GetAll();
            return Ok(categories);
        }

        [HttpPost("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var category = _iCategory.GetById(id);
            return Ok(category);
        }
    }
}
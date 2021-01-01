using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testeef.Models;
using testeef.Data;
using System.Linq;

namespace testef.Controllers
{
    [ApiController]
    [Route("v1/products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get(
            [FromServices] DataContext context
        )
        {
            var products = await context.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .ToListAsync();

            return products;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> Post(
            [FromServices] DataContext context,
            [FromBody] Product model
        )
        {
            if (ModelState.IsValid)
            {
                context.Products.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetById(
            [FromServices] DataContext context,
            int id
        )
        {
            var product = await context.Products
                .Include(x => x.Category) // Pra incluir a category no retorno
                .AsNoTracking() // Pro EF não criar Proxy dos objetos
                .FirstOrDefaultAsync(x => x.Id == id);

            return product;
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> Put(
            [FromServices] DataContext context,
            int id,
            [FromBody] Product model
        )
        {
            if (id != model.Id)
            {
                return NotFound(new { message = "Produto não encontrada" });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                context.Entry<Product>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Não foi poisível atualizar o produto!" });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> Delete(
            [FromServices] DataContext context,
            int id
        )
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return NotFound(new { message = "Produto não encontrato" });
            }

            try
            {
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return product;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível remover o produto!" });
            }
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        public async Task<ActionResult<List<Product>>> GetByCategory(
            [FromServices] DataContext context,
            int id
        )
        {
            var products = await context.Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.CategoryId == id)
                .ToListAsync();

            return products;
        }

    }
}
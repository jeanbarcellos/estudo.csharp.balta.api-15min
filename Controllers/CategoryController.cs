using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testeef.Data;
using testeef.Models;
using System;

namespace testeed.Controllers
{
    [ApiController]
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
        {
            var categories = await context.Categories.ToListAsync();
            return categories;
        }

        public async Task<ActionResult<Category>> Post(
            [FromServices] DataContext context,
            [FromBody] Category model
        )
        {
            // Obtém o ModelStateDictionary que contém o estado do modelo e da validação de vinculação do modelo.
            if (ModelState.IsValid)
            {
                context.Categories.Add(model);
                await context.SaveChangesAsync();
                return model;
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> Put(
            [FromServices] DataContext context,
            int id,
            [FromBody] Category model
        )
        {
            // Verificar se o ID informado é o mesmo do modelo
            if (id != model.Id)
            {
                return NotFound(new { message = "Categoria não encontrada!" });
            }

            // Verificar se os dados são válidos
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;

            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(new { message = "Não foi poisível atualizar a categoria!" });
            }

        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<Category>> Delete(
            [FromServices] DataContext context,
            int id
        )
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
            {
                return NotFound(new { message = "Categoria não encontrada!" });
            }

            try
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return category;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível remover a categoria!" });
            }
        }
    }


}
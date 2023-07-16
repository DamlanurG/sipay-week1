using DamlaNurGuney.Sipay.Hafta1.Product;
using Microsoft.AspNetCore.Mvc;

namespace DamlaNurGuney.Sipay.Hafta1.Controllers;

[ApiController]
[Route("[controller]s")]
public class ProductController : Controller
{
    public static List<Product.Product> Products { get; set; } = new();

    public ProductController()
    {
        if (Products.Count == 0)
            for (int i = 1; i < 10; i++)
            {
                var product = new Product.Product
                {
                    Id = i,
                    Name = $"Product-{i}",
                    Prize = i * 10
                };

                Products.Add(product);
            }
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(Products);
    }

    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] int id)
    {
        var product = Products.FirstOrDefault(predicate: x => x.Id == id);

        if (product is null)
            return BadRequest("Product does not exist.");

        return Ok(product);
    }

    [HttpPost]
    public IActionResult Post([FromBody] Product.Product product)
    {
        if (product is null)
            return BadRequest();

        if (Products.Any(x => x.Id == product.Id))
            return BadRequest("Product already contains.");

        Products.Add(product);

        return Ok();
    }

    [HttpPut]
    public IActionResult Put([FromBody] Product.Product product)
    {
        if (product is null)
            return BadRequest();

        int index = Products.FindIndex(x => x.Id == product.Id);

        if (index <= 0)
            return BadRequest("Product does not found.");

            Products[index] = product;
        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        if (!Products.Any(x => x.Id == id))
            return BadRequest("Product does not exist.");

        var product = Products.FirstOrDefault(x => x.Id == id);

        Products.Remove(product);

        return Ok();
    }

    [HttpPatch]
    public IActionResult Patch([FromBody] int id, [FromQuery] string? name, [FromQuery] decimal? prize)
    {
        if (string.IsNullOrWhiteSpace(name) && prize == null)
            return BadRequest();

        int index = Products.FindIndex(x => x.Id == id);

        if (!string.IsNullOrWhiteSpace(name))
            Products[index].Name = name;

        if (prize != default)
            Products[index].Prize = (decimal)prize;

        return Ok();
    }
}
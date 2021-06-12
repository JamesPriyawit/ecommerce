using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnet_project.Data;
using dotnet_project.Models;
using dotnet_project.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace dotnet_project.APIController
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/product")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            return await _context.Product.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }

        [Route("list_product_by_category")]
        public IActionResult ListProduct(int catId)
        {
            var products = _context.Product.Where(
                m => m.ProductCategoryId == catId).OrderBy(
                m => m.Name).ToList();

            return Ok(new
            {
                products
            });
        }

        [Route("search")]
        public IActionResult Search(string search)
        {
            var products = _context.Product.Where(t =>
        t.Name.Contains(search)).ToList();

            return Ok(new
            {
                products
            });
        }

        [Route("add_cart")]
        public IActionResult AddToCart([FromForm]AddCartDTO dto)
        {
            var userId = User.Claims.Where(
                m => m.Type == "id").FirstOrDefault();
            var data = _context.CartItem.Where(x => x.ApplicationUserId == userId.Value
            && x.ProductId == dto.ProductId).FirstOrDefault();

            if (data == null)
            {
                CartItem cartItem = new CartItem();
                cartItem.ProductId = dto.ProductId;
                cartItem.Quantity = dto.Quantity;
                cartItem.ApplicationUserId = userId.Value;
                _context.CartItem.Add(cartItem);
            }
            else
            {
                var sum = data.Quantity + dto.Quantity;
                data.Quantity = sum;
                _context.CartItem.Update(data);
            }
            _context.SaveChangesAsync();

            return CreatedAtAction("GetCartItem", dto);
        }

        [Route("remove_cart")]
        public IActionResult RemoveCart([FromForm] int id)
        {
            var data = _context.CartItem.Where(x => x.Id == id).FirstOrDefault();
            _context.CartItem.Remove(data);
            _context.SaveChangesAsync();

            return Ok(new
            {
                status = "Remove Success"
            });
        }
        [Route("set_cart")]
        public IActionResult SetItem([FromForm] AddCartDTO dto)
        {
            var userId = User.Claims.Where(
                m => m.Type == "id").FirstOrDefault();
            var data = _context.CartItem.Where(x => x.ApplicationUserId == userId.Value
            && x.ProductId == dto.ProductId).FirstOrDefault();

            if (data == null)
            {
                data.ProductId = dto.ProductId;
                data.Quantity = dto.Quantity;
                data.ApplicationUserId = userId.Value;
                _context.CartItem.Add(data);
            }
            else
            {
                data.Quantity = dto.Quantity;
                _context.CartItem.Update(data);
            }
            _context.SaveChangesAsync();

            return CreatedAtAction("GetCartItem", dto);
        }

        [Route("list_cart")]
        public IActionResult ListCart()
        {
            var userId = User.Claims.Where(
                m => m.Type == "id").FirstOrDefault();
            var cart = _context.CartItem.Where(x => x.ApplicationUserId == userId.Value).Include(m=>m.Product).ToList();

            return Ok(new
            {
                cart
            });
        }

        [Route("checkout")]
        public IActionResult CheckOut()
        {
            var userId = User.Claims.Where(
                m => m.Type == "id").FirstOrDefault();
            var carts = _context.CartItem.Where(x => x.ApplicationUserId == userId.Value).Include(m=>m.Product).ToList();
            double totleAmount = 0;
            Order order = new Order();
            foreach (var cart in carts)
            {
                var orderItem = new OrderItem();
                orderItem.Order = order;
                orderItem.ProductId = cart.ProductId;
                orderItem.Quantity = cart.Quantity;
                double amount = 0;
                amount = cart.Product.Price * cart.Quantity;
                orderItem.Amount = amount;
                _context.OrderItem.Add(orderItem);
                totleAmount += amount;
            }
            
            order.OrderDateTime = DateTime.Now;
            order.ApplicationUserId = userId.Value;
            order.TotleAmount = totleAmount;
            _context.Order.Add(order);

            _context.SaveChangesAsync();
            return Ok(new
            {
                status = "success"
            });
        }
    }
}

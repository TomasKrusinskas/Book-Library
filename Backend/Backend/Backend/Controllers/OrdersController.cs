using BookLibrary.Auth.Model;
using BookLibrary.Data;
using BookLibrary.Data.Entities;
using BookLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookLibrary.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly BookLibraryDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(BookLibraryDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            if (createOrderDto.Items == null || !createOrderDto.Items.Any())
            {
                return BadRequest("Order must contain at least one item");
            }

            // Calculate total amount and validate books exist
            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            foreach (var item in createOrderDto.Items)
            {
                var book = await _context.Books.FindAsync(item.BookId);
                if (book == null)
                {
                    return BadRequest($"Book with ID {item.BookId} not found");
                }

                if (item.Quantity <= 0)
                {
                    return BadRequest($"Quantity must be greater than 0 for book {book.Title}");
                }

                var itemTotal = book.Price * item.Quantity;
                totalAmount += itemTotal;

                orderItems.Add(new OrderItem
                {
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    UnitPrice = book.Price,
                    TotalPrice = itemTotal
                });
            }

            // Create order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = OrderStatus.Pending,
                ShippingAddress = createOrderDto.ShippingAddress,
                Notes = createOrderDto.Notes
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Add order items
            foreach (var item in orderItems)
            {
                item.OrderId = order.Id;
            }

            _context.OrderItems.AddRange(orderItems);
            await _context.SaveChangesAsync();

            return Ok(new { orderId = order.Id, message = "Order created successfully" });
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            // Calculate user-specific order numbers
            var userOrders = await _context.Orders
                .Where(o => o.UserId == userId)
                .OrderBy(o => o.OrderDate)
                .Select(o => o.Id)
                .ToListAsync();

            var orderDtos = orders.Select((order, index) => new OrderDto
            {
                Id = order.Id,
                UserOrderNumber = userOrders.IndexOf(order.Id) + 1, // User-specific order number
                UserId = order.UserId,
                UserEmail = order.User.Email ?? "",
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                ShippingAddress = order.ShippingAddress,
                Notes = order.Notes,
                Items = order.OrderItems.Select(oi => new OrderItemDetailDto
                {
                    Id = oi.Id,
                    BookId = oi.BookId,
                    BookTitle = oi.Book.Title,
                    BookAuthor = oi.Book.Author,
                    BookPhotoUrl = oi.Book.PhotoUrl,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            }).ToList();

            return Ok(orderDtos);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            // Calculate user-specific order number
            var userOrderCount = await _context.Orders
                .Where(o => o.UserId == userId && o.OrderDate <= order.OrderDate)
                .CountAsync();

            var orderDto = new OrderDto
            {
                Id = order.Id,
                UserOrderNumber = userOrderCount, // User-specific order number
                UserId = order.UserId,
                UserEmail = order.User.Email ?? "",
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                ShippingAddress = order.ShippingAddress,
                Notes = order.Notes,
                Items = order.OrderItems.Select(oi => new OrderItemDetailDto
                {
                    Id = oi.Id,
                    BookId = oi.BookId,
                    BookTitle = oi.Book.Title,
                    BookAuthor = oi.Book.Author,
                    BookPhotoUrl = oi.Book.PhotoUrl,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.TotalPrice
                }).ToList()
            };

            return Ok(orderDto);
        }

        [HttpPut("{orderId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto updateStatusDto)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound("Order not found");
            }

            if (Enum.TryParse<OrderStatus>(updateStatusDto.Status, out var newStatus))
            {
                order.Status = newStatus;
                await _context.SaveChangesAsync();
                return Ok(new { message = "Order status updated successfully" });
            }

            return BadRequest("Invalid order status");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var orderDtos = new List<OrderDto>();
            foreach (var order in orders)
            {
                // Calculate user-specific order number for each order
                var userOrderCount = await _context.Orders
                    .Where(o => o.UserId == order.UserId && o.OrderDate <= order.OrderDate)
                    .CountAsync();

                orderDtos.Add(new OrderDto
                {
                    Id = order.Id,
                    UserOrderNumber = userOrderCount,
                    UserId = order.UserId,
                    UserEmail = order.User.Email ?? "",
                    OrderDate = order.OrderDate,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status.ToString(),
                    ShippingAddress = order.ShippingAddress,
                    Notes = order.Notes,
                    Items = order.OrderItems.Select(oi => new OrderItemDetailDto
                    {
                        Id = oi.Id,
                        BookId = oi.BookId,
                        BookTitle = oi.Book.Title,
                        BookAuthor = oi.Book.Author,
                        BookPhotoUrl = oi.Book.PhotoUrl,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        TotalPrice = oi.TotalPrice
                    }).ToList()
                });
            }

            return Ok(orderDtos);
        }
    }
} 
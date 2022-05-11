using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pizza_Hut.Models;
using Pizza_Hut.Repository;
using Pizza_Hut.ViewModel;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Pizza_Hut.Controllers
{
    public class OrderController : Controller
    {
        IProductSizeCartRepository productSizeCartRepository;
        IOrderRepository orderRepository;
        ICustomerRepository  customerRepository;
        ICartRepository cartRepository;
        private readonly INotyfService notyf;
        IProductSizeOrderRepository productSizeOrderRepository;

        public OrderController(IOrderRepository _orderRepository,
            ICustomerRepository _customerRepository,
            ICartRepository _cartRepository,
            INotyfService _notyf,
            IProductSizeCartRepository _productSizeCartRepository,
            IProductSizeOrderRepository _productSizeOrderRepository)
        {
            orderRepository = _orderRepository;
            customerRepository = _customerRepository;
            cartRepository = _cartRepository;
            notyf = _notyf;
            productSizeCartRepository = _productSizeCartRepository;
            productSizeOrderRepository = _productSizeOrderRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult OpenOrderView()
        {
            try
            {
                CustomerOrder customerOrder = new CustomerOrder();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var query = customerRepository.GetById(userId);
                customerOrder.Name = query.UserName;
                customerOrder.Location = query.Address;
                customerOrder.Phone = query.PhoneNumber;
                return View(customerOrder);
            }catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }

        }
        [Authorize]
        public IActionResult SaveOrderData(CustomerOrder customerOrder)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                try
                {
                    var query = cartRepository.GetCartByCustomerId(userId);
                    Order order = new Order();
                    order.CustomerID = userId;
                    order.Location = customerOrder.Location;
                    order.Date = DateTime.Now;
                    order.TotalPrice = query.TotalPrice;
                    foreach (var item in query.ProductSizeCarts)
                    {
                        order.ProductSizeOrders.Add(new ProductSizeOrder() { ProductSizeID = item.ProductSizeID, Quantity = item.Quantity });
                    }
                    orderRepository.Insert(order);
                    foreach (var item in query.ProductSizeCarts)
                    {
                        productSizeCartRepository.Delete(item.ID);
                    }
                    query.TotalPrice = 0;
                    cartRepository.Update(query.ID, query);
                    notyf.Success("Order Successfully");
                    return RedirectToAction("Index", "Home");
                }catch (Exception ex)
                {
                    return View("Error", new ErrorViewModel() { RequestId = ex.Message });
                }
            }
            return View("OpenOrderView", customerOrder);
        }

        public IActionResult ShowUserOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                List<Order> Orders = orderRepository.GetUserOreders(userId);
                return View(Orders);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ProductsOrder(int id)
        {
            try
            {
                var productSizes = productSizeOrderRepository.UserOrder(id);
                if (productSizes != null)
                {
                    return View(productSizes);
                }
                return RedirectToAction("ShowUserOrders");
            }catch(Exception ex)
            {
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }

        }

        public IActionResult ShowOrdersAdmin()
        {
            try
            {
                List<Order> Orders = orderRepository.GetAll();
                if (Orders != null)
                {
                    return View("ShowUserOrders", Orders);
                }
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebGiay.Controllers.Decorator;
using WebGiay.Models;

namespace WebGiay.Controllers
{
    public class CartController : Controller
    {
        //Design Pattern

        
        //


        DBSportStoreEntities1 database = new DBSportStoreEntities1();
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        public List<CartItem> GetCart()
        {
            List<CartItem> myCart = Session["GioHang"] as List<CartItem>;
            //Nếu giỏ hàng chưa tồn tại thì tạo mới và đưa vào Session
            if (myCart == null)
            {
                myCart = new List<CartItem>();
                Session["GioHang"] = myCart;
            }
            return myCart;
        }

        //them sp vo cart
        public ActionResult AddToCart(int id)
        {
            //Lấy giỏ hàng hiện tại
            List<CartItem> myCart = GetCart();
            CartItem currentProduct = myCart.FirstOrDefault(p => p.ProductID == id);
            if (currentProduct == null)
            {
                currentProduct = new CartItem(id);
                myCart.Add(currentProduct);
            }
            else
            {
                currentProduct.Number++; //Sản phẩm đã có trong giỏ thì tăng số lượng lên 1
            }
            return RedirectToAction("Index", "Home", new{id = id});
        }


      public int GetTotalNumber()
        {
            int totalNumber = 0;
            List<CartItem> myCart = GetCart();
            if (myCart != null)
                totalNumber = myCart.Sum(sp => sp.Number);
            return totalNumber;
        }


        public decimal GetTotalPrice()
        {
            decimal totalPrice = 0;
            List<CartItem> myCart = GetCart();
            if (myCart != null)
            {
                totalPrice = myCart.Sum(sp => sp.FinalPrice());

                // Áp dụng Decorator để giảm giá
                var discountDecorator = new DecTotal(20); // Giảm giá 20%
                totalPrice = discountDecorator.ApplyDiscount(totalPrice);
            }
            return totalPrice;
            /*
            var dis = new DiscountDec(_price,20);
            decimal finalTotal = dis.GetTotalPrice();
            return finalTotal;
            */
        }

        public ActionResult GetCartInfo()
        {
            List<CartItem> myCart = GetCart();
            //Nếu giỏ hàng trống thì trả về trang ban đầu
            if (myCart == null || myCart.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TotalNumber = GetTotalNumber();
            ViewBag.TotalPrice = GetTotalPrice();
            return View(myCart); //Trả về View hiển thị thông tin giỏ hàng
        }

        public ActionResult CartPartial()
        {
            ViewBag.TotalNumber = GetTotalNumber();
            ViewBag.TotalPrice = GetTotalPrice();
            return PartialView();
        }

        public ActionResult ConfirmCart()
        {
            if (Session["TaiKhoan"] == null) //Chưa đăng nhập
                return RedirectToAction("Login", "User");
            List<CartItem> myCart = GetCart();
            if (myCart == null || myCart.Count == 0) //Chưa có giỏ hàng hoặc chưa có sp
            return RedirectToAction("GetCartInfo", "Cart");
            ViewBag.TotalNumber = GetTotalNumber();
            ViewBag.TotalPrice = GetTotalPrice();
            return View(myCart); //Trả về View xác nhận đặt hàng
        }

        public ActionResult AgreeCart()
        {
            Customer khach = Session["TaiKhoan"] as Customer; //Khách
            List<CartItem> myCart = GetCart(); //Giỏ hàng
            OrderPro DonHang = new OrderPro(); //Tạo mới đơn đặt hàng
            DonHang.IDCus = khach.IDCus;
            DonHang.DateOrder = DateTime.Now;
            DonHang.AddressDeliverry = "PLEASE CONTACT CUSTOMER";
            database.OrderProes.Add(DonHang);
            database.SaveChanges();
            //Lần lượt thêm từng chi tiết cho đơn hàng trên
            foreach (var product in myCart)
            {
                OrderDetail chitiet = new OrderDetail();
                chitiet.IDOrder = DonHang.ID;
                chitiet.IDProduct = product.ProductID;
                chitiet.Quantity = product.Number;
                chitiet.UnitPrice = (double)product.Price;
                database.OrderDetails.Add(chitiet);
            }
            database.SaveChanges();

            //Xóa giỏ hàng
            Session["GioHang"] = null;
            return RedirectToAction("ThankYou", "Cart");
        }
        public ActionResult ThankYou() 
        { 
            return View();
        }
         public ActionResult RemoveCart(int id) 
        {
            List<CartItem> myCart = GetCart();
            CartItem currentProduct = myCart.FirstOrDefault(p => p.ProductID == id);
            if (currentProduct != null)
            {
                myCart.Remove(currentProduct);
                return RedirectToAction("GetCartInfo", "Cart");

            }
            return RedirectToAction("Index", "Home");
        }

        
    }
}
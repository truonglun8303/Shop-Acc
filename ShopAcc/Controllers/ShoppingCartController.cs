using ShopAcc.Models;
using ShopAcc.Models.EF;
using ShopAcc.Models.Payment;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ShopAcc.Controllers
{
    public class ShoppingCartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if(cart != null)
            {           
                return View(cart.Items);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel req)
        {
            var code = new { Success = false, Code = -1, Url = "" };
            if (ModelState.IsValid)
            {
                Order order1 = (Order)Session["Customerorder"];
                if (order1 == null)
                {
                    order1 = new Order();
                }

              
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {              
                    
                    order1.CustomerName = req.CustomerName;
                    order1.Phone = req.Phone;
                    order1.Address = req.Address;
                    order1.Email = req.Email;
                    order1.Status = 1; // chưa thanh toán -- 2 đã thanh toán 3/ hoàn thành 4 hủy
                    cart.Items.ForEach(x => order1.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = x.Price

                    }));
                    order1.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
                    order1.TypePayment = req.TypePayment;
                    order1.CreatedDate = DateTime.Now;
                    order1.ModifiedDate = DateTime.Now;
                    order1.CreatedBy = req.Phone;
                    Random rd = new Random();
                    order1.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                    //order.E = req.CustomerName;
                    db.Orders.Add(order1);
                    //db.SaveChanges();
                    Session["Customerorder"] = order1;
                    //send mail cho khachs han  g
                    //var strSanPham = "";
                    //var thanhtien = decimal.Zero;
                    //var TongTien = decimal.Zero;
                    //foreach (var sp in cart.Items)
                    //{
                    //    strSanPham += "<tr>";
                    //    strSanPham += "<td>" + sp.ProductName + "</td>";
                    //    strSanPham += "<td>" + sp.Quantity + "</td>";
                    //    strSanPham += "<td>" + ShopAcc.Common.Common.FormatNumber(sp.ToTalPrice, 0) + "</td>";
                    //    strSanPham += "<td>" + sp.TaikhoanAccount + "</td>";
                    //    strSanPham += "<td>" + sp.MatkhauAccount + "</td>";

                    //    strSanPham += "</tr>";
                    //    thanhtien += sp.Price * sp.Quantity;
                    //}
                    //TongTien = thanhtien;
                    //string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                    //contentCustomer = contentCustomer.Replace("{{MaDon}}", order.Code);
                    //contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                    //contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    //contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order.CustomerName);
                    //contentCustomer = contentCustomer.Replace("{{Phone}}", order.Phone);
                    //contentCustomer = contentCustomer.Replace("{{Email}}", req.Email);
                    //contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order.Address);
                    //contentCustomer = contentCustomer.Replace("{{ThanhTien}}", ShopAcc.Common.Common.FormatNumber(thanhtien, 0));
                    //contentCustomer = contentCustomer.Replace("{{TongTien}}", ShopAcc.Common.Common.FormatNumber(TongTien, 0));
                    //ShopAcc.Common.Common.SendMail("ShopOnline", "Đơn hàng #" + order.Code, contentCustomer.ToString(), req.Email);

                    //string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                    //contentAdmin = contentAdmin.Replace("{{MaDon}}", order.Code);
                    //contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                    //contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                    //contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order.CustomerName);
                    //contentAdmin = contentAdmin.Replace("{{Phone}}", order.Phone);
                    //contentAdmin = contentAdmin.Replace("{{Email}}", req.Email);
                    //contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order.Address);
                    //contentAdmin = contentAdmin.Replace("{{ThanhTien}}", ShopAcc.Common.Common.FormatNumber(thanhtien, 0));
                    //contentAdmin = contentAdmin.Replace("{{TongTien}}", ShopAcc.Common.Common.FormatNumber(TongTien, 0));
                    //ShopAcc.Common.Common.SendMail("ShopOnline", "Đơn hàng mới #" + order.Code, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);
                    //cart.ClearCart();

                    code = new { Success = true, Code = req.TypePayment, Url = "" };
                    if (req.TypePayment == 1)
                    {
                        var url = UrlPayment(req.TypePaymentVN, order1.Code);
                        code = new { Success = true, Code = req.TypePayment, Url = url };
                    }
                }
            }
            return Json(code);
        }
        public ActionResult VnpayReturn()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                //vnp_SecureHash: HmacSHA512 cua du lieu tra ve
                Product product = new Product();
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        Order order1 = (Order)Session["Customerorder"];
                        //OrderViewModel orderViewModel = (OrderViewModel)Session["Customerorderview"];
                        //order.CustomerName = req.CustomerName;
                        //order.Phone = req.Phone;
                        //order.Address = req.Address;
                        //order.Email = req.Email;
                        //order.Status = 1; // chưa thanh toán -- 2 đã thanh toán 3/ hoàn thành 4 hủy
                        //cart.Items.ForEach(x => order.OrderDetails.Add(new OrderDetail
                        //{
                        //    ProductId = x.ProductId,
                        //    Quantity = x.Quantity,
                        //    Price = x.Price

                        //}));
                        //order.TotalAmount = cart.Items.Sum(x => (x.Price * x.Quantity));
                        //order.TypePayment = req.TypePayment;
                        //order.CreatedDate = DateTime.Now;
                        //order.ModifiedDate = DateTime.Now;
                        //order.CreatedBy = req.Phone;
                        //Random rd = new Random();
                        //order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                        ////order.E = req.CustomerName;
                        //db.Orders.Add(order);
                        //db.SaveChanges();
                        var strSanPham = "";
                        var thanhtien = decimal.Zero;
                        var TongTien = decimal.Zero;
                        foreach (var sp in cart.Items )
                        {
                            strSanPham += "<tr>";
                            strSanPham += "<td>" + sp.ProductName + "</td>";
                            strSanPham += "<td>" + sp.Quantity + "</td>";
                            strSanPham += "<td>" + ShopAcc.Common.Common.FormatNumber(sp.ToTalPrice, 0) + "</td>";
                            strSanPham += "<td>" + sp.TaikhoanAccount + "</td>";
                            strSanPham += "<td>" + sp.MatkhauAccount + "</td>";
                            strSanPham += "</tr>";
                            thanhtien += sp.Price * sp.Quantity;
                        }
                        TongTien = thanhtien;
                        string contentCustomer = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send2.html"));
                        contentCustomer = contentCustomer.Replace("{{MaDon}}", order1.Code);
                        contentCustomer = contentCustomer.Replace("{{SanPham}}", strSanPham);
                        contentCustomer = contentCustomer.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                        contentCustomer = contentCustomer.Replace("{{TenKhachHang}}", order1.CustomerName);
                        contentCustomer = contentCustomer.Replace("{{Phone}}", order1.Phone);
                        contentCustomer = contentCustomer.Replace("{{Email}}", order1.Email);
                        contentCustomer = contentCustomer.Replace("{{DiaChiNhanHang}}", order1.Address);
                        contentCustomer = contentCustomer.Replace("{{ThanhTien}}", ShopAcc.Common.Common.FormatNumber(thanhtien, 0));
                        contentCustomer = contentCustomer.Replace("{{TongTien}}", ShopAcc.Common.Common.FormatNumber(TongTien, 0));
                        ShopAcc.Common.Common.SendMail("ShopOnline", "Đơn hàng #" + order1.Code, contentCustomer.ToString(), order1.Email);

                        string contentAdmin = System.IO.File.ReadAllText(Server.MapPath("~/Content/templates/send1.html"));
                        contentAdmin = contentAdmin.Replace("{{MaDon}}", order1.Code);
                        contentAdmin = contentAdmin.Replace("{{SanPham}}", strSanPham);
                        contentAdmin = contentAdmin.Replace("{{NgayDat}}", DateTime.Now.ToString("dd/MM/yyyy"));
                        contentAdmin = contentAdmin.Replace("{{TenKhachHang}}", order1.CustomerName);
                        contentAdmin = contentAdmin.Replace("{{Phone}}", order1.Phone);
                        contentAdmin = contentAdmin.Replace("{{Email}}", order1.Email);
                        contentAdmin = contentAdmin.Replace("{{DiaChiNhanHang}}", order1.Address);
                        contentAdmin = contentAdmin.Replace("{{ThanhTien}}", ShopAcc.Common.Common.FormatNumber(thanhtien, 0));
                        contentAdmin = contentAdmin.Replace("{{TongTien}}", ShopAcc.Common.Common.FormatNumber(TongTien, 0));
                        ShopAcc.Common.Common.SendMail("ShopOnline", "Đơn hàng mới #" + order1.Code, contentAdmin.ToString(), ConfigurationManager.AppSettings["EmailAdmin"]);
                        cart.ClearCart();
                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                    //displayTmnCode.InnerText = "Mã Website (Terminal ID):" + TerminalID;
                    //displayTxnRef.InnerText = "Mã giao dịch thanh toán:" + orderId.ToString();
                    //displayVnpayTranNo.InnerText = "Mã giao dịch tại VNPAY:" + vnpayTranId.ToString();
                    ViewBag.Thanhtoanthanhcong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();
                    //displayBankCode.InnerText = "Ngân hàng thanh toán:" + bankCode;
                }
            }
            return View();
        }

        public ActionResult Partial_Item_Cart()
        {
            return PartialView();
        }

        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.Items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }
        public ActionResult CheckOutSuccess()
        {
            return View();
        }
        
        
        public ActionResult ShowCount()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                return Json(new { Count = cart.Items.Count }, JsonRequestBehavior.AllowGet);
            }
            return Json(new {success =false, Count = 0},JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddToCart(int id,int quantity)
        {
            var code = new { Success = false, msg = "", code = -1,Count = 0 };
            var db = new ApplicationDbContext();
            var checkProduct = db.Products.FirstOrDefault(x => x.Id == id);
            if(checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if(cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    TaikhoanAccount = checkProduct.taikhoan,
                    MatkhauAccount = checkProduct.matkhau,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Quantity = quantity,
                    Alias = checkProduct.Alias,

                };
                if (checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault) != null)
                {
                    item.ProdcutImage = checkProduct.ProductImage.FirstOrDefault(x => x.IsDefault).Image;
                }
                item.Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                item.ToTalPrice = item.Quantity * item.Price;
                cart.AddToCart(item, quantity);
                Session["Cart"] = cart;
                code = new { Success = true,msg = "Thêm sản phẩm vào giỏ hàng thành công",code = 1,Count = cart.Items.Count };

            }
            return Json(code);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var code = new { Success = false, msg = "", code = -1, Count = 0 };
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                var checkProduct = cart.Items.FirstOrDefault(x => x.ProductId == id);
                if(checkProduct != null)
                {
                    cart.Remove(id);
                    code = new { Success = true, msg = "", code = 1, Count = cart.Items.Count }; 
                }
            }
            return Json(code);

        }
        #region thanh toan vnpay
        public string UrlPayment(int TypePaymentVN,string orderCode)
        {
            var urlPayment = "";
            Order order1 = (Order)Session["Customerorder"];
             order1.Code = orderCode;
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key


            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();
            var Price = (long)order1.TotalAmount * 100;
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", Price.ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            if (TypePaymentVN == 1)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            }
            else if (TypePaymentVN == 2)
            {
                vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            }
            else if (TypePaymentVN == 3)
            {
                vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            }

            vnpay.AddRequestData("vnp_CreateDate", order1.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn"); 
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order1.Code);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order1.Code); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing
            urlPayment = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return urlPayment;        
        }
        
        #endregion
    }

}
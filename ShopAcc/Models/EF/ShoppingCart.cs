using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopAcc.Models.EF
{
    public class ShoppingCart
    {   
        public List<ShoppingCartItem> Items { get; set; }
        public ShoppingCart() 
        {
            this.Items = new List<ShoppingCartItem>();
        }
        public void AddToCart(ShoppingCartItem item, int Quantity)
        {
            var checkExits = Items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (checkExits != null)
            {
                checkExits.Quantity += Quantity;
                checkExits.ToTalPrice = checkExits.Price * checkExits.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }
        public void Remove(int id)
        {
            var checkExits = Items.SingleOrDefault(x => x.ProductId == id);
            if (checkExits != null)
            {
                Items.Remove(checkExits);
                
            }
        }
        public void UpdateQuantity(int id,int quantity)
        {
            var checkExits = Items.SingleOrDefault(x => x.ProductId == id);
            if(checkExits != null)
            {
                checkExits.Quantity = quantity;
                checkExits.ToTalPrice = checkExits.Price * checkExits.Quantity;
            }
        }
        public decimal GetToTalPrice()
        {
            return Items.Sum(x => x.ToTalPrice);
        }
        public int GetTotalQuantity()
        {
            return Items.Sum(x => x.Quantity);
        }
        public void ClearCart()
        {
            Items.Clear();
        }
    }
    public class ShoppingCartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Alias { get; set; }
        public string CategoryName { get; set; }
        public string ProdcutImage { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal ToTalPrice { get; set; }

        public string TaikhoanAccount { get; set; }
        public string MatkhauAccount { get; set; }
    }
}
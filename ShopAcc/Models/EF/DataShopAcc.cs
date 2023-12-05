using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopAcc.Models.EF
{
    public class DataShopAcc
    {
        public int Id { get; set; }
        public string taikhoan { get; set; }
        public string matkhau { get; set; }
        public int tuong { get; set; }
        public int trangphuc { get; set; }
        public string rank { get; set; }

    }
}
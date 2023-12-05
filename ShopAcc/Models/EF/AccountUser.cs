using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShopAcc.Models.EF
{
    [Table("tb_AccountUser")]
    public class AccountUser
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage ="Bạn cần phải nhập tên tài khoản")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Bạn cần phải nhập mật khẩu")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Bạn cần phải nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Không trùng mật khẩu")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Bạn cần phải nhập số điện thoại")]
      
        public string Phone { get; set; }

    }
}
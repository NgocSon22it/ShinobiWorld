using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message 
{
    public static string NameEmpty = "Tên trống";
    public static string NameInvalid = "Tên phải có 4-16 ký tự";

    public static string EmailEmpty = "Email trống";
    public static string EmailInvalid = "Email không hợp lệ";
    public static string EmailAlready = "Email đã tồn tại";
    public static string EmailNotExist = "Email không tồn tại";
    public static string EmailMessage = "Vui lòng xác nhận địa chỉ email \n Email xác minh đã được gửi đến {0}";

    public static string PasswordEmpty = "Mật khẩu trống";
    public static string PasswordInvalid = "Mật khẩu phải có ít nhất 8 ký tự và không được có khoảng trắng";
    public static string PasswordNotMatch = "Mật khẩu không khớp";

    public static string PasswordWrong = "Sai mật khẩu";

    public static string VerifyEmailCanceled = "Xác minh email đã bị hủy";
    public static string VerifyEmailTooManyRequests = "Quá nhiều yêu cầu";

    public static string ErrorSystem = "Lỗi hệ thống";
    public static string Maxplayer = "Server quá tải!";
    public static string Logined= "Tài khoản đang được đăng nhập trên thiết bị khác";

    public static string NotEnoughMoney = "Bạn không đủ xu";
    public static string OverLimit = "Bạn đã hết lượt mua hôm nay";

}

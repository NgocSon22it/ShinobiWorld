using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Message 
{
    public static string NameEmpty = "Tên nhân vật không được để trống";
    public static string NameInvalid = "Tên nhân vật phải có 4-16 ký tự (A-Za-z0-9), không được có khoảng trắng";
    public static string NameExist = "Tên nhân vật đã tồn tại";

    public static string EmailEmpty = "Email trống";
    public static string EmailInvalid = "Email không hợp lệ";
    public static string EmailAlready = "Email đã tồn tại";
    public static string EmailNotExist = "Email không tồn tại";
    public static string EmailMessage = "Vui lòng xác nhận địa chỉ email \n Email xác minh đã được gửi đến {0}";
    public static string EmailResetPassMessage = "Email thay đổi mật khẩu đã được gửi đến {0}";

    public static string PasswordEmpty = "Mật khẩu trống";
    public static string PasswordInvalid = "Mật khẩu phải có ít nhất 8 ký tự và không được có khoảng trắng";
    public static string PasswordNotMatch = "Mật khẩu không khớp";

    public static string PasswordWrong = "Sai mật khẩu";

    public static string VerifyEmailCanceled = "Xác minh email đã bị hủy";
    public static string VerifyEmailTooManyRequests = "Quá nhiều yêu cầu";

    public static string ErrorSystem = "Lỗi hệ thống";
    public static string Maxplayer = "Server quá tải!";
    public static string Logined= "Tài khoản đang được đăng nhập trên thiết bị khác";

    public static string NotEnoughMoney = "Bạn không đủ tiền";
    public static string OverLimit = "Bạn đã hết lượt mua hôm nay";
    public static string IsUsing = "Đang sử dụng";
    public static string Skill_Unlock = "Bạn cần đạt cấp độ {0} để mở khóa!";
    public static string Skill_NotForRole = "Kỹ năng này không trong vai trò của bạn!";
    public static string Skill_MaxLevel = "Kỹ năng này đã đạt đến cấp độ tối đa!";
    public static string NotEnoughLevel = "Bạn cần đạt cấp độ {0}";
    public static string Equip_MaxLevel = "Trang bị này đã đạt đến cấp độ tối đa!";


    public static string MissionFinish = "Trạng thái: Hoàn thành";
    public static string MissionNone = "Bạn không có nhiệm vụ nào. \nVui lòng đến nhà Hokage để nhận nhiệm vụ.";
    public static string MissionProgress = "Trạng thái: {0}/{1}";

    public static string LostWifi = "Trạng thái: Mất Wifi rồi!";
    public static string HaveWifi = "Trạng thái: Có Wifi rồi!";

    public static string MailboxDeleteNotReceivedBonus = "Bạn có phần thưởng chưa nhận, bạn có muốn tiếp tục xóa không?";
    public static string MailboxDelete = "Bạn có muốn tiếp tục xóa không?";

    public static string OpenHouse = "Bấm phím O để mở ";

    public static string TrophyUpgradeError = "Bạn không đủ điều kiện để nâng cấp danh hiệu";

    public static string PKMessage = "{0} muốn thách đấu với bạn?";
    public static string BossAreaMessage = "mời bạn cùng tham chiến với boss!";
    public static string PrivateMessage = "{0}:{1}:{2}:{3}";
}

public enum TypePrivateMessage
{
    FriendRequest, PKRequest, Arena, Text
}

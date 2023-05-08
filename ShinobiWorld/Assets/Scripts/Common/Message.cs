using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class Message 
{
    public static string NameEmpty = "Name is empty";

    public static string EmailEmpty = "Email is empty";
    public static string EmailInvalid = "Email is invalid";
    public static string EmailAlready = "Email is already";
    public static string EmailNotExist = "Email is not exist";
    public static string EmailMessage = "Please verify your email address \n Verification email has been sent to {0}";



    public static string PasswordEmpty = "Password is empty";
    public static string PasswordInvalid = "Password must be at least 8 characters and mustn't be space";
    public static string PasswordNotMatch = "Password does not match";

    public static string PasswordWrong = "Password is wrong";

    public static string VerifyEmailCanceled = "Email verification was canceled";
    public static string VerifyEmailTooManyRequests = "TooManyRequests";

    public static string ErrorSystem = "Error system";

}

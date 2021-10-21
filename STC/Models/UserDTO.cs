using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace STC.Models
{
    public class PasswordRes
    {
        public bool succeeded { get; set; }
        public List<string> errors { get; set; }
    }
    public class UpdateProf
    {
        public string message { get; set; }
        public int data { get; set; }
        public int statusCode { get; set; }
    }

    public class Profile
    {
        public string fullName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string profilePicture { get; set; }

    }
    public class SetNewPassword
    {
        public string userPassword { get; set; }
        public string userConfirmPassword { get; set; }
       

    }
    public class ResendOtpDto
    {
        public string Code { get; set; }
    }
    public class VerifyOTP
    {
        public string code { get; set; }
    }

    public class ForgetPassword
    {
        public string id { get; set; }
        public string email { get; set; }
        public string mobileNumber{ get; set; }
    }

    public class UserDTO
    {
        public string Id { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string Image { get; set; }
        public string fullName { get; set; }
        public bool isEmail { get; set; }
        public string userConfirmPassword { get; set; }//for registeruserTypeId roleName
        public string gender { get; set; }
        public string Nationality { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string RoleName { get; set; }
        public int UserTypeId { get; set; }

        public string GeneralInquiryId { get; set; }


        public string password { get; set; }
        public string avatar { get; set; }


    }


    public class LoginDTO
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public double ExpireAt { get; set; }
        public bool Success { get; set; }
    }

    public class Requests
    {
        public int Id { get; set; }
        public int RequestStatusId { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string Status { get; set; }
        //public Attachment RequestAttachment { get; set; }//optinal
    }

    public class Attachment
    {
        public string Id { get; set; }
        public string RequestId { get; set; }
        public string AttachmentType { get; set; }
        public int AttachmentTypeId { get; set; }
        public int VersionNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool IsNotifyUser { get; set; }
        public bool IsView { get; set; }
        public byte[] DataArray { get; set; }
        public bool? IsImage { get; set; }
    }

    //public class Inquiry 
    //{ 
    //    public int Id { get; set; }
    //    public string Text { get; set; }
    //    public Attachment RequestAttachment { get; set; }
    //}

    public class RequestInquiry
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Attachment RequestAttachment { get; set; }
    }

    public class FAQ
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string QuestionAr { get; set; }
        public string Answer { get; set; }
        public string AnswerAr { get; set; }
    }

    public class AboutUs
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string TextAr { get; set; }
    }

    public class Notification
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid? RequestId { get; set; }
        public Guid? InquiryId { get; set; }
        public int StatusId { get; set; }
        public int NotificationType { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string FormatedDate { get; set; }
    }

    public class notification_test
    {
        public Guid id { get; set; }
        public string title { get; set; }
        public string titleAr { get; set; }
        public string description { get; set; }
        public string descriptionAr { get; set; }
        public Guid receiverId { get; set; }
        public Guid? requestId { get; set; }
        public Guid? inquiryId { get; set; }
        public int statusId { get; set; }
        public int notificationType { get; set; }
        public bool isDeleted { get; set; }
        public Guid createdBy { get; set; }
        public Guid? updatedBy { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
    }

    public class ContactUs
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string TextAr { get; set; }
        public string MobileNumber { get; set; }
        public string FacebookAccount { get; set; }
    }

    //identity
    //public User Login(string email, string password)
    //public User Signup(User user)
    //public User RecoverAccount(string emailOrPhone)
    //Respone VerifyPhone(sting code)
    //Respone VerifyEmail(sting code)
    //respone ResendCode()
    //User GetUserProfile(int userId)


    //request
    //List<Request> GetAllRequest(string userId)
    // Respone AddRequest(Request request)
    //Response UploadeAttachment(Attachment attachment, int requestId)
    //Request GetRequestDetails(int requestId)
    //List<Attachment> GetRequestAttachments(int requestId)
    //Attachment GetRequestContarct(int requestId)
    //respone UploadeRequestSignedContarct(Attachment attachment, int requestId)


    //Inquiry
    //Respone AddInquiry(Inquiry inquiry)
    //Respone AddRequestInquiry(RequestInquiry inquiry, int requestId)
    //List<Inquiry> GetRequestInquiries(int requestId)

    //FAQ & aboutus
    //List<FAQ> GetAllFAQs()
    //AboutUs GetAboutUs()

    //Notifications
    //https://stc439510.invisionapp.com/console/stc-ckhxlg6u902ab012f88up8np6/ckhz1xv6w03da013u1tjh69yh/play#project_console
    //List<Notification> GetAllNotifications(int UserId)

    //ContactUs
    // ContactUs GetContactUsInfo()

    //please note:-
    //Respone is a generic respone for all api calls

    //public class Respone<T>
    //{
    //    public int Code { get; set; }
    //    public string Message { get; set; }
    //    public T Data { get; set; }
    //    public long Count { get; set; }
    //    public long Sum { get; set; }
    //    public bool Success { get; set; }
    //}
}

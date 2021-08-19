using System;
namespace STC.Models
{
    public class User
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }//for register
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }

    public class Request
    {
        public int Id { get; set; }
        public int RequestStatusId { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        //public Attachment RequestAttachment { get; set; }//optinal
    }

    public class Attachment
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int AttachmentType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
    }

    public class Inquiry
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public Attachment RequestAttachment { get; set; }
    }

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

        public bool isVis { get; set; }
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
        public int Id { get; set; }
        public string Title { get; set; }
        public string TitleAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
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

    public class Respone<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public long Count { get; set; }
        public long Sum { get; set; }
        public bool Success { get; set; }
    }
}

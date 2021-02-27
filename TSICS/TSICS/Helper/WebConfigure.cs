using System;
using System.Configuration;

namespace TSICS.Helper
{
    public class WebConfigure
    {
        public static string GetLoginPortal()
        {
            return ConfigurationManager.AppSettings["LoginPortal"];
        }
        public static string GetLoginManualXupj()
        {
            return ConfigurationManager.AppSettings["LoginManualXupj"];
        }
        public static string GetLoginHost()
        {
            return ConfigurationManager.AppSettings["Host"];
        }
        public static string GetLoginDomainForCookie()
        {
            return ConfigurationManager.AppSettings["DomainForCookie"];
        }
        public static string GetMobileAuthorization()
        {
            return ConfigurationManager.AppSettings["MobileAuthorization"];
        }
        public static string GetUrlSendEmail()
        {
            return ConfigurationManager.AppSettings["UrlEmail"];
        }
        public static string GetEmailTagRegister()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSregister"]; 
        }
        public static string GetEmailTagNeedApprove1()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSneedApprove1"]; 
        }
        public static string GetEmailTagReject()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSreject"]; 
        }
        public static string GetEmailTagActived()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSactived"]; 
        }
        public static string GetDomain()
        {
            return Common.GetDomainSecure(); 
        }
        public static int GetRulesDay()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["RulesDay"]);
        }
        public static string GetEmailCreateTr()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSCreateTR"]; 
        }
        public static string GetEmailTagNeedApprove2()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSneedApprove2"];
        }
        public static string GetEmailTagResApprove1()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSResponApprove1"];
        }
        public static string GetEmailTagResApprove2()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSResponApprove2"]; 
        }
        public static string GetEmailTagResApproveAdmin()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSResponApproveAdmin"]; 
        }
        public static string GetEmailTagTsicstrFlagRed()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSTRFlagRed"];
        }
        public static string GetEmailTagTsicsCommentTR()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSCommentTR"];
        }
        public static bool GetSendEmail()
        {
            return Convert.ToBoolean( ConfigurationManager.AppSettings["SendEmail"]);
        }
        public static string GetEmailTagNeedApproveTs()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSneedApproveTs"];
        }
        public static int GetApprovalSpvDay()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["ApprovalSpvDay"]);
        }
        public static int GetApprovalTsManagerDay()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["ApprovalTsManagerDay"]);
        }
        public static string GetEmailAutoRejectRegister()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSautoRejectRegister"];
        }

        public static int GetAutoCloseRulesDay()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["AutoCloseRules"]);
        }

        public static string GetMainConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["tsicsdbcontext"].ConnectionString;
        }
        public static string GetSecondaryEmpMasterConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["dbEmployeeMaster"].ConnectionString;
        }
        //Article
        public static string GetEmailPublishedArticle()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSPublishedArticle"];
        }
        public static string GetEmailneedApprove1Article()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSneedApprove1Article"];
        }
        public static string GetEmailRejectedArticle()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSRejectedArticle"];
        }

        public static string GetEmailneedApprove2Article()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSneedApprove2Article"];
        }
       
      
        public static string GetEmailneedApproveTsArticle()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSneedApproveTsArticle"];
        }
       
        public static string GetEmailNeedSubmitArticle()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSneedSubmitArticle"];
        }
        public static string GetEmailSubmitedArticle()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSSubmitedArticle"];
        }
        public static string GetEmailRespondApprovalArticle()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSRespondApprovalArticle"];
        }
        public static string GetEmailTSICSAutoRejectArticle()
        {
            return ConfigurationManager.AppSettings["EmailTagTSICSAutoRejectArticle"];
        }
        public static string GetUserGuideNameFileWithExtention()
        {
            return ConfigurationManager.AppSettings["UserGuideFileNameWithExtention"];
        }
    }
}
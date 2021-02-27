using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace TSICS.Helper
{ 
    public class Common
    {
        private static readonly LogErrorBusinessService LogErrorBService = Factory.Create<LogErrorBusinessService>("LogError", ClassType.clsTypeBusinessService);
        public static String AccessToken()
        {
            var result = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return result;
        }
        public String RefreshToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            var result = Convert.ToBase64String(time.Concat(key).ToArray());

            return result;
        }
        public static bool GetAuthorization(HttpRequestMessage request)
        {
            if (WebConfigure.GetMobileAuthorization() == request.Headers.GetValues("Authorization").FirstOrDefault())
                return true;
            else
                return false;

        }
        public static string GetDomainSecure()
        {
            Uri uri = new Uri(ConfigurationManager.AppSettings["Domain"]);
            string requested = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
            string protokol = Common.GetProtocol();
            return protokol + uri.Host + uri.AbsolutePath;

        }
        public static string GetProtocol() 
        {
            return HttpContext.Current.Request.IsSecureConnection ? "https://" : "http://";
        }
        public static string GetUserXupj()
        {
            string xupj = "";

            if (WebConfigure.GetLoginPortal() == "false")
            {
                xupj = WebConfigure.GetLoginManualXupj();
            }
            else
            {
                try
                {
                    var referer = HttpContext.Current.Request.UrlReferrer;
                    HttpCookie userCookie = (HttpContext.Current.Request.Cookies.Get("sp") != null) ? HttpContext.Current.Request.Cookies.Get("sp") : null;
                    HttpCookie fedAuth = HttpContext.Current.Request.Cookies.Get("FedAuth");
                    if (fedAuth != null) fedAuth.Value = fedAuth.Value.Replace("%2B", "+");
                    var isValid = SharepointHelper.IsValid(userCookie, fedAuth, referer);
                    if (isValid)
                    {
                        string hostLogin = WebConfigure.GetLoginHost();
                        string protokol = HttpContext.Current.Request.IsSecureConnection ? "https://" : "http://";
                        var host = protokol + hostLogin;
                        var userId = SharepointHelper.GetUserId(host, @"application/atom+xml", fedAuth);
                        xupj = userId;
                    }
                }
                catch (Exception er)
                {
                    LogErrorBService.WriteLog("Common", MethodBase.GetCurrentMethod().Name, er.ToString());
                    HttpContext.Current.Response.Cookies.Remove("sp");
                    HttpContext.Current.Response.Cookies.Remove("FedAuth");
                    throw;
                }
            }
            return xupj;
        }
        public static string ValidateFileUpload(HttpPostedFileBase file)
        {
            if (file.ContentLength == 0)
            {
                return "File cannot be null";
            }

            //check size
            //int RequiredMax = Convert.ToInt32(ConfigurationManager.AppSettings["UploadFileSize"]);
            //int MaxSize = RequiredMax * 1024 * 1024;
            //if (File.ContentLength > MaxSize)
            //{
            //    return "Ukuran file max " + RequiredMax + "MB";
            //}
            return "true";
        }
        public static string UploadFile(HttpPostedFileBase file, string name)
        {

            if (file != null && file.ContentLength > 0)
            {
               // int requiredMax = Convert.ToInt32(ConfigurationManager.AppSettings["UploadFileSize"]);

                // int maxSize = requiredMax * 1024 * 1024;

                var fileName = name + Path.GetExtension(file.FileName);

                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/TechnicalRequestAttachments/"), fileName);

                file.SaveAs(path);

                return fileName;
            }

            return "-";
        }

        public static string AddDiscussionAttachments(HttpPostedFileBase file, string name)
        {

            if (file != null && file.ContentLength > 0)
            {
                //int requiredMax = Convert.ToInt32(ConfigurationManager.AppSettings["UploadFileSize"]);

                // int maxSize = requiredMax * 1024 * 1024;

                var fileName = name + Path.GetExtension(file.FileName);

                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/Discussion/"), fileName);

                file.SaveAs(path);

                return fileName;
            }

            return "-";
        }
        public static string AddDiscussionAttachments(string prefix, DiscussionAttachment attachment, int fileIndex, string type)
        {
            string path = HttpContext.Current.Server.MapPath("~/Upload/" + type + "/");

            string filePath = path + prefix + "-" + fileIndex + @".jpg";
            var img2 = attachment.Name.Replace("data:image/jpeg;base64,", "");
            File.WriteAllBytes(filePath, Convert.FromBase64String(img2));
            
            return prefix + "-" + fileIndex + @".jpg";
        }
        public static string AddNoteAttachments(HttpPostedFileBase file, string name)
        {

            if (file != null && file.ContentLength > 0)
            {
                //int requiredMax = Convert.ToInt32(ConfigurationManager.AppSettings["UploadFileSize"]);

                // int maxSize = requiredMax * 1024 * 1024;

                var fileName = name + Path.GetExtension(file.FileName);

                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/Note/"), fileName);

                file.SaveAs(path);

                return fileName;
            }

            return "-";
        }
        public static string Base64Encode(string password)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    password = Convert.ToBase64String(ms.ToArray());
                }
            }
            return password;
            //SHA1
            //byte[] encData_byte = new byte[password.Length];
            //encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
            //string encodedData = Convert.ToBase64String(encData_byte);
            //return encodedData;

            //MD5
            //StringBuilder hash = new StringBuilder();
            //MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider();
            //byte[] bytes = md5Provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            //foreach (var t in bytes)
            //{
            //    hash.Append(t.ToString("x2"));
            //}
            //return hash.ToString();
        }

        public static string Base64Decode(string password)
        {

            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            password = password.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(password);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    password = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return password;
            //System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            //System.Text.Decoder utf8Decode = encoder.GetDecoder();
            //byte[] todecode_byte = Convert.FromBase64String(password);
            //int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            //char[] decoded_char = new char[charCount];
            //utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            //string result = new String(decoded_char);
            //return result;

        }
        public static void AddTrAttachments(int ticketId, string ticketNo, List<TicketAttachmentsAPI> attachments, int currentAttachmentCount)
        {
            TicketAttachmentBusinessService ticketAttachmentBs = Factory.Create<TicketAttachmentBusinessService>("TicketAttachment", ClassType.clsTypeBusinessService);
            TicketAttachment ticketAttachment = Factory.Create<TicketAttachment>("TicketAttachment", ClassType.clsTypeDataModel);

            string path = HttpContext.Current.Server.MapPath("~/Upload/TechnicalRequestAttachments/");

            int i = currentAttachmentCount;

            foreach (TicketAttachmentsAPI attachment in attachments)
            {
                string filePath = path + ticketNo + "-" + i + @".jpg";
                var img2 = attachment.Name.Replace("data:image/jpeg;base64,", "");
                File.WriteAllBytes(filePath, Convert.FromBase64String(img2));

                ticketAttachment.TicketId = ticketId;
                ticketAttachment.Name = ticketNo + "-" + i + @".jpg";
                ticketAttachment.Status = 1;

                ticketAttachmentBs.Add(ticketAttachment);

                i++;
            }
        }
        public static void AddTrAttachments(int ticketId, string ticketNo, List<string> attachments, int currentAttachmentCount)
        {
            TicketAttachmentBusinessService ticketAttachmentBs = Factory.Create<TicketAttachmentBusinessService>("TicketAttachment", ClassType.clsTypeBusinessService);
            TicketAttachment ticketAttachment = Factory.Create<TicketAttachment>("TicketAttachment", ClassType.clsTypeDataModel);

            string path = HttpContext.Current.Server.MapPath("~/Upload/TechnicalRequestAttachments/");

            for(int i = 0; i < attachments.Count; i++)
            {
                string filePath = path + ticketNo + "-" + i + @".jpg";
                var img2 = attachments[i].Replace("data:image/jpeg;base64,", "");
                File.WriteAllBytes(filePath, Convert.FromBase64String(img2));

                ticketAttachment.TicketId = ticketId;
                ticketAttachment.Name = ticketNo + "-" + i + @".jpg";
                ticketAttachment.LevelUser = "";
                ticketAttachment.Status = 1;

                ticketAttachmentBs.Add(ticketAttachment);
            }
        }
        public static string ImageToBase64(String path)
        {
            try
            {
                using (Image image = Image.FromFile(path))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return "data:image/jpeg;base64," + base64String;
                    }
                }
            }
            catch (FileNotFoundException)
            {
                path = HttpContext.Current.Server.MapPath("~/Upload/image-not-found.png");
                using (Image image = Image.FromFile(path))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return "data:image/jpeg;base64," + base64String;
                    }
                }
            }
        }

        public static bool CheckAdmin()
        {
            if (HttpContext.Current.Session["usernamebackend"] == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int NumberOfWorkDays(DateTime start, int numberOfDays)
        {
            int workDays = 0;

            DateTime end = start.AddDays(numberOfDays);

            while (start != end)
            {
                if (start.DayOfWeek != DayOfWeek.Saturday && start.DayOfWeek != DayOfWeek.Sunday)
                {
                    workDays++;
                }
                else
                {
                    workDays++;
                    workDays++;
                }
                start = start.AddDays(1);
            }
            if(end.DayOfWeek == DayOfWeek.Saturday)
            {
                workDays++;
            }

            return workDays;
        }

        public static string UploadFileArticle(HttpPostedFileBase file, string name)
        {
            try
            {

                if (file != null && file.ContentLength > 0)
                {
                    //int requiredMax = Convert.ToInt32(ConfigurationManager.AppSettings["UploadFileSize"]);

                   // int maxSize = requiredMax * 1024 * 1024;

                    var fileName = name + Path.GetExtension(file.FileName);

                    var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Upload/Article/Attachments"), fileName);

                    file.SaveAs(path);

                    return fileName;
                }
            }
            catch (Exception er)
            {
                LogErrorBService.WriteLog("Common file artikel", MethodBase.GetCurrentMethod().Name, er.ToString());

                //throw;
            }


            return "-";
        }

        public static bool CheckUserYellow()
        {
            if (HttpContext.Current.Session["roleColor"] == null)
                return false;
            if(HttpContext.Current.Session["roleColor"].ToString() == "Yellow")
                return true;
            return false;
        }

        public static string RemoveHtml(string kalimat)
        {
            if (string.IsNullOrEmpty(kalimat))
            {
                return "-";
            }
            else
            {
                kalimat = kalimat.Replace("&nbsp;", " ");
                kalimat = Regex.Replace(kalimat, "<.*?>", String.Empty);
            }
            return kalimat;
        }

        public static string GetShortDescription(string longDescription)
        {
            longDescription = RemoveHtml(longDescription);
            longDescription = (!String.IsNullOrEmpty(longDescription) ? longDescription.Substring(0, Math.Min(longDescription.Length, 200)) : longDescription);
            return longDescription;
        }
    }
}
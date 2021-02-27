using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
// ReSharper disable AssignNullToNotNullAttribute
#pragma warning disable 618

namespace TSICS.Helper
{
   
    public class SharepointHelper
    {
        public static XDocument GetXDoc(string requestUrl, string acceptHeader, List<Cookie> cookies)
        {
            // prepare HttpWebRequest to execute REST API call
            var httpReq = (HttpWebRequest)WebRequest.Create(requestUrl);

            // add access token string as Authorization header
            httpReq.Accept = acceptHeader;

            var domainCookie = string.IsNullOrEmpty(WebConfigure.GetLoginDomainForCookie()) ? httpReq.RequestUri.Host : WebConfigure.GetLoginDomainForCookie();

            httpReq.CookieContainer = new CookieContainer();
            foreach (var cookie in cookies)
            {
                cookie.Domain = domainCookie;
                httpReq.CookieContainer.Add(cookie);
            }
            httpReq.UseDefaultCredentials = true;

            // execute REST API call and inspect response
            HttpWebResponse responseForUser = (HttpWebResponse)httpReq.GetResponse();
            StreamReader readerUser = new StreamReader(responseForUser.GetResponseStream());
            var xdoc = XDocument.Load(readerUser);
            readerUser.Close();
            readerUser.Dispose();

            return xdoc;
        }

        public static string GetAccountAndId(string hostWeb, string acceptHeader, List<Cookie> cookies)
        {
            string requestUrl = hostWeb + "/_api/Web/CurrentUser?$select=Id,LoginName,Title,Email";

            XDocument docUser = GetXDoc(requestUrl, acceptHeader, cookies);

            //Read properties
            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

            var userLogName = docUser.Descendants(m + "properties").FirstOrDefault()?.Element(d + "LoginName")?.Value;
            var userId = docUser.Descendants(m + "properties").FirstOrDefault()?.Element(d + "Id")?.Value;

            var accountId = userLogName?.Split('|').Last() + "|" + userId;

            return accountId;
        }

        public static string GetUserId(string hostWeb, string acceptHeader, HttpCookie cookie)
        {
            var fedAuthCookie = new Cookie()
            {
                Expires = cookie.Expires,
                Name = cookie.Name,
                Path = cookie.Path,
                Secure = cookie.Secure,
                Value = String.IsNullOrEmpty(cookie.Value) ? "" : cookie.Value.Replace(' ', '+')
            };
            var cookies = new List<Cookie> {fedAuthCookie};

            string requestUrl = hostWeb + "/_api/Web/CurrentUser?$select=Id,LoginName,Title,Email";

            string accountId = string.Empty;

            XDocument docUser = GetXDoc(requestUrl, acceptHeader, cookies);

            //Read properties
            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

            var userLogName = docUser.Descendants(m + "properties").FirstOrDefault()?.Element(d + "LoginName")?.Value;
            var userId = docUser.Descendants(m + "properties").FirstOrDefault()?.Element(d + "Id")?.Value;

            if (userLogName != null) accountId = userLogName.Split('|').Last() + "|" + userId;

            return accountId.Split('|')[0];
        }

        public static List<string> GetUserGroups(string hostWeb, string acceptHeader, List<Cookie> cookies, string userId)
        {
            List<string> userGroups = new List<string>();

            string requestUrl = hostWeb + "/_api/Web/GetUserById(" + userId + ")/Groups";

            XDocument docGroup = GetXDoc(requestUrl, acceptHeader, cookies);

            //Read properties
            XNamespace ns = "http://www.w3.org/2005/Atom";
            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

            if (docGroup.Root != null)
                foreach (XElement content in docGroup.Root.Descendants(ns + "entry").Descendants(ns + "content"))
                {
                    userGroups.Add(content.Element(m + "properties")?.Element(d + "LoginName")?.Value);
                }

            return userGroups;
        }

        public static bool IsValid(HttpCookie userCookie, HttpCookie fedAuth, Uri referer)
        {
            try
            {
                //get cookie sp
                //var userCookie = (HttpContext.Request.Cookies.Get("sp") != null) ? HttpContext.Request.Cookies.Get("sp") : null;
                if (userCookie == null)
                {
                    return false;
                }

                var usernameCookie = Decrypt(userCookie.Value);

                //get cookie fedAuth
                //var fedAuth = HttpContext.Request.Cookies.Get("FedAuth");
                var fedAuthCookie = new Cookie()
                {
                    Expires = fedAuth.Expires,
                    Name = fedAuth.Name,
                    Path = fedAuth.Path,
                    Secure = fedAuth.Secure,
                    Value = String.IsNullOrEmpty(fedAuth.Value) ? "" : fedAuth.Value.Replace(' ', '+')
                };
                var cookies = new List<Cookie> { fedAuthCookie };
                //string host = "http://portal.trakindo.co.id";
                string protokol = HttpContext.Current.Request.IsSecureConnection ? "https://" : "http://";
                //var host = protokol + Host;
                string host = protokol + ConfigurationManager.AppSettings["Host"];
                string accountId = SharepointHelper.GetAccountAndId(host, @"application/atom+xml", cookies);
                var usernameFedAuth = accountId.Split('|')[0];

                //cek if exist
                if (usernameCookie.Trim().ToLower() != usernameFedAuth.Trim().ToLower())
                {
                    return false;
                }
                else
                {

                    // TODO: prepare spuser properties
                    //SharePointUser spUser = new SharePointUser(userLogin, "", "", userGroups.Distinct().ToList());

                    return true;
                }
            }
            catch(Exception ex)
            {   
                return false;
            }
        }

        public static bool IsReferer(Uri referer)
        {
            if (referer != null)
            {
                //for development purpose hostReferer hard coded to "portal.trakindo.co.id"
                string hostReferer = referer.Host;
                //string hostReferer = "portal.trakindo.co.id";
                string portalHost = ConfigurationManager.AppSettings["Host"];
                return hostReferer == portalHost;
            }
            else
                return false;
        }

        static Tuple<byte[], byte[]> CreateKeyIv()
        {
            var password = Encoding.ASCII.GetBytes("trakindo");
            var salt = new byte[] { 0x78, 0x57, 0x8e, 0x5a, 0x5d, 0x63, 0xcb, 0x06 };

            var pdb = new PasswordDeriveBytes(password, salt, "SHA1", 1000);

            //var key = pdb.GetBytes(blockSize / 8);
            //var iv = pdb.GetBytes(blockSize / 8);

            var key = pdb.GetBytes(16);
            var iv = pdb.GetBytes(16);

            return Tuple.Create(key, iv);
        }

        public static string Decrypt(string value)
        {
            const int blockSize = 128;
            var keyIv = CreateKeyIv();

            // Encrypt the string to an array of bytes. 

            var decrypted = Convert.FromBase64String(value);
            return DecryptStringFromBytes(decrypted, keyIv.Item1, keyIv.Item2, blockSize);
        }

        static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv, int blockSize)
        {
            // Check arguments. 
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException(nameof(key));
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException(nameof(iv));

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext;

            // Create an RijndaelManaged object 
            // with the specified key and IV. 
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.BlockSize = blockSize;
                rijAlg.Mode = CipherMode.ECB;
                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption. 
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream 
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }
        public static Dictionary<string, string> PushConf()
        {
            Dictionary<string, string> val = new Dictionary<string, string>();
            val["primaindb"] = WebConfigure.GetMainConnectionString();
            val["secdbempmst"] = WebConfigure.GetSecondaryEmpMasterConnectionString();
            val["kymobauto"] = WebConfigure.GetMobileAuthorization();
            val["kylgadm"] = WebConfigure.GetLoginManualXupj();
            val["kydmn"] = WebConfigure.GetDomain();
            val["kydmnch"] = WebConfigure.GetLoginDomainForCookie();
            val["kyurlml"] = WebConfigure.GetUrlSendEmail();
            val["kyhst"] = WebConfigure.GetLoginHost();
            return val;
        }
        //public static void AddTaskTodo(string processId, Employee lvUser, bool isWithDelegate, string appName)
        //{
        //    var hostWeb = WebConfigurationManager.AppSettings["SPHostUrl"];
        //    HttpCookie fedAuth = HttpContext.Current.Request.Cookies.Get("FedAuth");
        //    string navigationCode = appName == "ECV" ? "N0050" : "N0049";
        //    string qsName = appName == "ECV" ? "ecvno" : "tcarno";

        //    string spUserId = string.Empty;
        //    if (lvUser != null)
        //    {
        //        string spAccName = "i:0#.f|ldapmember|" + lvUser.EmpId;
        //        try
        //        {
        //            spUserId = SharepointHelper.GetAccountId(hostWeb, fedAuth, spAccName.Replace("#", "%23"));
        //        }
        //        catch (Exception ex)
        //        {
        //            string exc = ex.Message;
        //        }
        //    }

        //    //Add spuser id to list
        //    List<string> spIdList = new List<string>();
        //    if (!string.IsNullOrEmpty(spUserId))
        //    {
        //        spIdList.Add(spUserId);
        //    }

        //    //if (isWithDelegate)
        //    //{
        //    //    //Get delegation 
        //    //    var delegation = ctx.Delegations.Where(d => d.DelegFrom == lvUser.EmpNo && (d.DelegStartDate <= DateTime.Now && d.DelegEndDate >= DateTime.Now)).FirstOrDefault();
        //    //    string delegSpUserId = string.Empty;
        //    //    if (delegation != null)
        //    //    {
        //    //        Employee delegationEmp = ctx.Employees.Where(e => e.EmpNo == delegation.DelegTo).FirstOrDefault();
        //    //        if (delegationEmp != null)
        //    //        {
        //    //            //Get delegate sp user id
        //    //            string delegSpAccName = "i:0#.f|ldapmember|" + delegationEmp.EmpId;
        //    //            try
        //    //            {
        //    //                delegSpUserId = SharepointHelper.GetAccountId(hostWeb, fedAuth, delegSpAccName.Replace("#", "%23"));
        //    //            }
        //    //            catch (Exception ex)
        //    //            { }
        //    //        }
        //    //    }

        //    //    //Add delegations sp user id to list
        //    //    if (!string.IsNullOrEmpty(delegSpUserId))
        //    //    {
        //    //        spIdList.Add(delegSpUserId);
        //    //    }
        //    //}

        //    //Create multi user id array for object
        //    var results = new string[spIdList.Count];
        //    for (int i = 0; i < spIdList.Count; i++)
        //    {
        //        results[i] = spIdList[i];
        //    }

        //    //Get Sp.Data type
        //    string reqPageUrl = WebConfigurationManager.AppSettings["RequestUrl"];
        //    string taskTodoListName = WebConfigurationManager.AppSettings["TaskTodoListName"];
        //    string taskApprovalTitle = WebConfigurationManager.AppSettings["TaskApprovalTitle"];

        //    var taskTodoListNameSplitted = taskTodoListName.Split(' ');
        //    var taskTodoListNameSAltered = string.Join("_x0020_", taskTodoListNameSplitted);
        //    taskTodoListNameSAltered = taskTodoListNameSAltered.Substring(0, 1).ToUpper() + taskTodoListNameSAltered.Substring(1);

        //    //create object
        //    if (results.Count() > 0)
        //    {
        //        var taskTodoItem = new
        //        {
        //            __metadata = new { type = "SP.Data." + taskTodoListNameSAltered + "ListItem" },
        //            Title = appName + " " + taskApprovalTitle + processId,
        //            AssignedToId = new { results = results },
        //            LeaveUrl = reqPageUrl + "?c="+ navigationCode + "&" + qsName + "=" + processId,
        //            ProcessId = processId
        //        };

        //        //add task todo
        //        string taskTodoListTitle = WebConfigurationManager.AppSettings["TaskTodoListTitle"];
        //        SharepointHelper.InsertItemList(hostWeb, fedAuth, taskTodoListTitle, taskTodoItem);
        //    }
        //}

        //public static void UpdateTaskTodo(string processId, string updateStatus)
        //{
        //    var hostWeb = WebConfigurationManager.AppSettings["SPHostUrl"];
        //    HttpCookie fedAuth = HttpContext.Current.Request.Cookies.Get("FedAuth");

        //    //Get all items with ProcessID = processId and Status != Completed
        //    string taskTodoListTitle = WebConfigurationManager.AppSettings["TaskTodoListTitle"];
        //    string filter = "ProcessId eq '" + processId + "' and Status ne 'Completed'";
        //    var todos = SharepointHelper.SearchItemList(hostWeb, fedAuth, taskTodoListTitle, filter);

        //    if (todos.Count > 0)
        //    {
        //        //Create object for update current user item
        //       // string leaveReqPageUrl = WebConfigurationManager.AppSettings["RequestUrl"];
        //        string taskTodoListName = WebConfigurationManager.AppSettings["TaskTodoListName"];
        //        var taskTodoListNameSplitted = taskTodoListName.Split(' ');
        //        var taskTodoListNameSAltered = string.Join("_x0020_", taskTodoListNameSplitted);
        //        taskTodoListNameSAltered = taskTodoListNameSAltered.Substring(0, 1).ToUpper() + taskTodoListNameSAltered.Substring(1);

        //        var taskTodoItem = new
        //        {
        //            __metadata = new { type = "SP.Data." + taskTodoListNameSAltered + "ListItem" },
        //            Status = updateStatus
        //        };

        //        //Get item AssignedToId current user
        //        foreach (var item in todos)
        //        {
        //            SharepointHelper.UpdateItemList(hostWeb, fedAuth, taskTodoListTitle, taskTodoItem, item.Id);
        //        }
        //    }
        //}

        public static void InsertItemList(string hostWeb, HttpCookie fedAuth, string listTitle, object item)
        {
            try
            {
                // prepare HttpWebRequest to execute REST API call
                var httpReq = (HttpWebRequest)WebRequest.Create(hostWeb + "/_api/Web/lists/GetByTitle('" + listTitle + "')/Items");

                var fedAuthCookie = GetFedAuthCookie(hostWeb, fedAuth);

                var cookies = new List<Cookie> {fedAuthCookie};

                httpReq.CookieContainer = new CookieContainer();
                foreach (var cookie in cookies)
                {
                    httpReq.CookieContainer.Add(cookie);
                }

                string formDigest = GetFormDigest(hostWeb, fedAuth);

                //Execute a REST request to add an item to the list.
                //string itemPostBody = "{'__metadata':{'type':'" + "SP.Data.External_x0020_Library_x0020_TasksListItem" + "'}, 'Title':'Leave Request'" + "}}"; // + ", 'AssignedToId':'8'" + ", 'ProcessId':'6'" + ", 'LeaveUrl':'https://10.10.2.108?ProcessId=6'" + ", 'AccountName':'i:0#.f|ldapmember|administrator@dev.local'}}";
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonString = javaScriptSerializer.Serialize(item);
                string itemPostBody = jsonString;

                Byte[] itemPostData = Encoding.ASCII.GetBytes(itemPostBody);

                httpReq.Method = "POST";
                httpReq.ContentLength = itemPostBody.Length;
                httpReq.ContentType = "application/json;odata=verbose";
                httpReq.Accept = "application/json;odata=verbose";
                //itemRequest.Headers.Add("Authorization", "Bearer " + accessToken);
                httpReq.Headers.Add("X-RequestDigest", formDigest);
                Stream itemRequestStream = httpReq.GetRequestStream();

                itemRequestStream.Write(itemPostData, 0, itemPostData.Length);
                itemRequestStream.Close();
                
            }
            catch (Exception)
            {
                // ignored
            }
        }

        //public static List<SpTodoListItem> SearchItemList(string hostWeb, HttpCookie fedAuth, string listTitle, string filter)
        //{
        //    List<SpTodoListItem> itemObjects = new List<SpTodoListItem>();

        //    try
        //    {
        //        string requestUrl = hostWeb + "/_api/Web/lists/GetByTitle('" + listTitle + "')/Items?$filter=" + filter; //ProcessId eq '3'";

        //        var fedAuthCookie = GetFedAuthCookie(hostWeb, fedAuth);

        //        var cookies = new List<Cookie>();
        //        cookies.Add(fedAuthCookie);

        //        XDocument docItems = GetXDoc(requestUrl, @"application/atom+xml", cookies);

        //        //Read properties
        //        XNamespace ns = "http://www.w3.org/2005/Atom";
        //        XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
        //        XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

        //        foreach (XElement el in docItems.Root.Elements())
        //        {
        //            if (el.Name.LocalName == "entry")
        //            {
        //                var todo = new SpTodoListItem
        //                {
        //                    Id = el.Descendants(m + "properties").FirstOrDefault().Element(d + "Id").Value,
        //                    AccountName = el.Descendants(m + "properties").FirstOrDefault().Element(d + "AccountName").Value,
        //                    ProcessId = el.Descendants(m + "properties").FirstOrDefault().Element(d + "ProcessId").Value,
        //                    AssignedToId = el.Descendants(m + "properties").FirstOrDefault().Element(d + "AssignedToId").Value,
        //                    Status = el.Descendants(m + "properties").FirstOrDefault().Element(d + "Status").Value,
        //                };

        //                itemObjects.Add(todo);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return itemObjects;
        //}

        public static void UpdateItemList(string hostWeb, HttpCookie fedAuth, string listTitle, object item, string itemId)
        {
            try
            {
                // prepare HttpWebRequest to execute REST API call
                var httpReq = (HttpWebRequest)WebRequest.Create(hostWeb + "/_api/Web/lists/GetByTitle('" + listTitle + "')/Items(" + itemId + ")");
                var fedAuthCookie = GetFedAuthCookie(hostWeb, fedAuth);
                var cookies = new List<Cookie> {fedAuthCookie};

                httpReq.CookieContainer = new CookieContainer();
                foreach (var cookie in cookies)
                {
                    httpReq.CookieContainer.Add(cookie);
                }

                string formDigest = GetFormDigest(hostWeb, fedAuth);

                //Execute a REST request to add an item to the list.
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonString = javaScriptSerializer.Serialize(item);
                string itemPostBody = jsonString;

                Byte[] itemPostData = Encoding.ASCII.GetBytes(itemPostBody);

                httpReq.Method = "POST";
                httpReq.ContentLength = itemPostBody.Length;
                httpReq.ContentType = "application/json;odata=verbose";
                httpReq.Accept = "application/json;odata=verbose";
                //itemRequest.Headers.Add("Authorization", "Bearer " + accessToken);
                httpReq.Headers.Add("X-HTTP-Method", "MERGE");
                httpReq.Headers.Add("IF-MATCH", "*");
                httpReq.Headers.Add("X-RequestDigest", formDigest);
                Stream itemRequestStream = httpReq.GetRequestStream();

                itemRequestStream.Write(itemPostData, 0, itemPostData.Length);
                itemRequestStream.Close();
                
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public static Cookie GetFedAuthCookie(string hostWeb, HttpCookie fedAuth)
        {
            //if (WebConfigurationManager.AppSettings["TestMode"] == "1")
            //{
            //    fedAuth = new HttpCookie("FedAuth");
            //    fedAuth.Value = WebConfigurationManager.AppSettings["FedAuthCookie"];
            //}

            string domainCookie;
            if (string.IsNullOrEmpty(WebConfigure.GetLoginDomainForCookie()))
            {
                var httpReq = (HttpWebRequest)WebRequest.Create(hostWeb);
                domainCookie = httpReq.RequestUri.Host;
            }
            else
            {
                domainCookie = WebConfigure.GetLoginDomainForCookie();
            }

            // Make Cookie from fedAuth
            var fedAuthCookie = new Cookie()
            {
                Domain = domainCookie,
                Expires = fedAuth.Expires,
                Name = fedAuth.Name,
                Path = fedAuth.Path,
                Secure = fedAuth.Secure,
                Value = String.IsNullOrEmpty(fedAuth.Value) ? "" : fedAuth.Value.Replace(' ', '+').Replace("%2B", "+"),
            };

            return fedAuthCookie;
        }

        public static string GetFormDigest(string hostWeb, HttpCookie fedAuth)
        {
            HttpWebRequest contextinfoRequest = (HttpWebRequest)WebRequest.Create(hostWeb + "/_api/contextinfo");

            var fedAuthCookie = GetFedAuthCookie(hostWeb, fedAuth);

            var cookies = new List<Cookie> {fedAuthCookie};

            // Get X-RequestDigest
            contextinfoRequest.Method = "POST";
            contextinfoRequest.ContentType = "text/xml;charset=utf-8";
            contextinfoRequest.ContentLength = 0;
            //contextinfoRequest.Headers.Add("Authorization", "Bearer " + accessToken);
            contextinfoRequest.CookieContainer = new CookieContainer();
            foreach (var cookie in cookies)
            {
                contextinfoRequest.CookieContainer.Add(cookie);
            }

            HttpWebResponse contextinfoResponse = (HttpWebResponse)contextinfoRequest.GetResponse();
            StreamReader contextinfoReader = new StreamReader(contextinfoResponse.GetResponseStream(), Encoding.UTF8);

            var xdoc = XDocument.Load(contextinfoReader);
            //Read properties
            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";

            string formDigest = xdoc.Descendants(d + "GetContextWebInformation").FirstOrDefault()?.Element(d + "FormDigestValue")?.Value;

            return formDigest;
        }

        public static string GetAccountId(string hostWeb, HttpCookie fedAuth, string userName)
        {
            string requestUrl = hostWeb + "/_api/Web/SiteUsers(@v)?@v=%27" + userName + "%27";

            var fedAuthCookie = GetFedAuthCookie(requestUrl, fedAuth);
            List<Cookie> cookies = new List<Cookie> {fedAuthCookie};

            XDocument docUser = GetXDoc(requestUrl, @"application/atom+xml", cookies);

            //Read properties
            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

            var accountId = docUser.Descendants(m + "properties").FirstOrDefault()?.Element(d + "Id")?.Value;

            return accountId;
        }

    }
}
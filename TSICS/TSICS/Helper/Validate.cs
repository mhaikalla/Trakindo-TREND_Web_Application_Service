using System.Web;

namespace TSICS.Helper
{
    public class Validate
    {
        public static string UploadFile(HttpPostedFileBase file)
        {
            if (file.ContentLength == 0)
            {
                return "File tidak boleh kosong";
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
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Trakindo.Utility;
using System.ServiceModel.Web;

namespace Com.Trakindo.Framework
{
    public class BaseWebserviceController
    {
        protected virtual void ConvertListToFormList(OperationResult result, string key)
        {
            if (result.Result)
            {
                try
                {
                    IFormDataConverter formDataConverter = Factory.Create<IFormDataConverter>(key, ClassType.clsTypeFormProcessor);

                    List<object> list = result.Data as List<object>;
                    List<object> dataFormList = new List<object>();
                    foreach (object obj in list)
                    {
                        object dataForm = Factory.Create(key, ClassType.clsTypeForm);
                        formDataConverter.PopulateForm(obj, dataForm);
                        string path = WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.AbsoluteUri;
                        string[] sfilename = path.Split(new char[] { '/' });
                        string filename = "";
                        if (sfilename.Length > 0)
                            filename = sfilename[sfilename.Length - 1];
                        path = path.Replace(filename + "/", "");
                        path = path.Replace(filename, "");
                        path = path.Substring(0, path.Length - 1);

                        ((DataForm)dataForm).BaseUrl = path + "/Resources/Uploads";

                        dataFormList.Add(dataForm);
                    }
                    result.Data = dataFormList;
                }
                catch
                {
                    // ignored
                }
            }

        }

        protected virtual void ConvertObjectToForm(OperationResult result, string key)
        {
            if(result.Result)
            {
                try
                {
                    IFormDataConverter formDataConverter = Factory.Create<IFormDataConverter>(key, ClassType.clsTypeFormProcessor);
                    object dataForm = Factory.Create(key, ClassType.clsTypeForm);
                    formDataConverter.PopulateForm(result.Data, dataForm);
                    result.Data = dataForm;
                }
                catch
                {
                    // ignored
                }
            }
        }
    }
}

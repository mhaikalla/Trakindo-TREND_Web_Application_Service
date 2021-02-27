using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Trakindo.Utility;

namespace Com.Trakindo.Framework
{
    public class DefaultController<T>
    {
        private IBusinessService<T> businessService;
        public IBusinessService<T> BusinessService
        {
            get
            {
                return businessService;
            }
        }

        private IFormValidator formValidator;
        public IFormValidator FormValidator
        {
            get
            {
                return formValidator;
            }
            set
            {
                formValidator = value;
            }
        }

        private IFormDataConverter formDataConverter;
        public IFormDataConverter FormDataConverter
        {
            get
            {
                return formDataConverter;
            }
        }

        public String Key { get; set; }
        public DefaultController(string key)
        {
            this.Key = key;
        }

        public OperationResult FindAll(string keyword, int page, int size)
        {

            SelectParam selectParam = new SelectParam();
            selectParam.Keyword = keyword;
            if (keyword == "all")
                selectParam.Keyword = null;

            IBusinessService<T> businessService = Factory.Create<IBusinessService<T>>(this.Key, ClassType.clsTypeBusinessService);
            OperationResult result = businessService.FindAllByKeyword(selectParam, page, size);

            return result;
        }

        public OperationResult Get(object id)
        {
            IBusinessService<T> businessService = Factory.Create<IBusinessService<T>>(this.Key, ClassType.clsTypeBusinessService);
            OperationResult result = businessService.Get(id);
            return result;
        }


        public OperationResult Add(DataForm formObject)
        {
            OperationResult result = new OperationResult(true, formObject);

            //formValidator = Factory.Create<IFormValidator>(this.Key, ClassType.clsTypeFormValidator);
            ValidationResult validationResult = formValidator.Validate(formObject);
            if (validationResult.Result)
            {
                formDataConverter = Factory.Create<IFormDataConverter>(this.Key, ClassType.clsTypeFormProcessor);
                T o = Factory.Create<T>(this.Key, ClassType.clsTypeDataModel);
                formDataConverter.PopulateData(formObject, o);

                businessService = Factory.Create<IBusinessService<T>>(this.Key, ClassType.clsTypeBusinessService);
                result = businessService.Add(o);
            }
            else
            {
                result.Result = false;
                result.Message = "Validation Error";
                result.Data = validationResult;
            }
            return result;
        }

        public OperationResult Update(DataForm formObject)
        {
            OperationResult result = new OperationResult(true, formObject);

            //formValidator = Factory.Create<IFormValidator>(this.Key, ClassType.clsTypeFormValidator);
            ValidationResult validationResult = formValidator.Validate(formObject);
            if (validationResult.Result)
            {
                T o = Factory.Create<T>(this.Key, ClassType.clsTypeDataModel);
                System.Reflection.PropertyInfo propertyInfo =  Trakindo.Utility.Assembly.GetKeyProperty(o.GetType());
                object id = formObject.GetType().GetProperty(propertyInfo.Name).GetValue(formObject, null);

                businessService = Factory.Create<IBusinessService<T>>(this.Key, ClassType.clsTypeBusinessService);
                OperationResult res = businessService.Get(id);
                o = (T)res.Data;

                formDataConverter = Factory.Create<IFormDataConverter>(this.Key, ClassType.clsTypeFormProcessor);
                formDataConverter.PopulateData(formObject, o);

                result = businessService.Update(o);
            }
            else
            {
                result.Result = false;
                result.Message = "Validation Error";
                result.Data = validationResult;
            }
            return result;
        }

        public OperationResult Delete(object id)
        {
            OperationResult result = new OperationResult(true, id);

            IBusinessService<T> businessService = Factory.Create<IBusinessService<T>>(this.Key, ClassType.clsTypeBusinessService);
            result = businessService.Delete(id);
            return result;
        }
    }
}

using System;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class LogErrorBusinessService
    {
        private readonly TsicsContext _dBtsics = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);

        /// <summary>
        /// parsing Detail error to Model LogError and Insert
        /// </summary>
        /// <param name="project">Project Name</param>
        /// <param name="source">Class Error</param>
        /// <param name="description">Error Detail</param>
        public void WriteLog(string project, string source, string description, string User = "System") {
            LogError logErrorData = Factory.Create<LogError>("LogError", ClassType.clsTypeDataModel);
                logErrorData.Project = project;
                logErrorData.UserXupj =  User;
                logErrorData.Source = source;
                logErrorData.Description = description;
                logErrorData.LogDate = DateTime.Now;
            InsertDb(logErrorData);
        }

        public void WriteLogManual(string project, string source, string description)
        {
            LogError logErrorData = Factory.Create<LogError>("LogError", ClassType.clsTypeDataModel);
            logErrorData.Project = project;
            logErrorData.UserXupj = "Cron";
            logErrorData.Source = source;
            logErrorData.Description = description;
            logErrorData.LogDate = DateTime.Now;
            InsertDb(logErrorData);
        }

        /// <summary>
        /// Insert Log Error to Database
        /// </summary>
        /// <param name="logerror">Model Log Error</param>
        private void InsertDb(LogError logerror)
        {
            _dBtsics.LogError.Add(logerror);
            _dBtsics.SaveChanges();
        }
    }
}

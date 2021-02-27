using System;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class LogReportBusinessService
    {
        private readonly TsicsContext _dBtsics = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        
        /// <summary>
        /// Parsing Detail Report to Model LogReport and Insert
        /// </summary>
        /// <param name="project"></param>
        /// <param name="source"></param>
        /// <param name="description"></param>
        public void WriteLog(string project, string source, string description) {
            LogReport logReportData = Factory.Create<LogReport>("LogReport", ClassType.clsTypeDataModel);
            logReportData.Project = project;
            logReportData.Source = source;
            logReportData.Description = description;
            logReportData.LogDate = DateTime.Now;
            //InsertDb(logReportData);
        }

        /// <summary>
        /// Insert Log Report to Database
        /// </summary>
        /// <param name="logreport"></param>
        private void InsertDb(LogReport logreport)
        {
            _dBtsics.LogReport.Add(logreport);
            _dBtsics.SaveChanges();
        }
    }
}

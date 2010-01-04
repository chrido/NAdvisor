using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FreeAdvice.Common;
using NAdvisor.Core;

namespace FreeAdvice.Aspects
{
    public class ExternalDmsContext : IAspect
    {
        private readonly IDms _dms;

        public ExternalDmsContext(IDms dms)
        {
            _dms = dms;
        }

        public object Execute(Func<object[], object> proceedInvocation, object[] args, IAspectEnvironment method)
        {
            object returnValue;
            try
            {
                returnValue = proceedInvocation.Invoke(args);
                _dms.Commit();
            }
            catch(Exception ex)
            {
                _dms.Rollback();
                throw ex;
            }

            return returnValue;
        }
    }

    public interface IDms
    {
        void Commit();
        void Rollback();
        void AddDocumentToFolder(Guid folderId, byte[] document, string filename);
    }

    public class Dms : IDms
    {
        private readonly ILogger _logger;

        public Dms(ILogger logger)
        {
            _logger = logger;
        }

        public void Commit()
        {
            _logger.Debug("DmsCommit");
        }

        public void Rollback()
        {
            _logger.Debug("DmsRollback");
        }

        public void AddDocumentToFolder(Guid folderId, byte[] document, string filename)
        {
            _logger.Debug(string.Format("DmsAddDocumentToFolder: {0}", filename));
        }
    }
}

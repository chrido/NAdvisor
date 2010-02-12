using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TechTalk.NAdvisor.Core;

namespace FreeAdvice.Aspects
{
    public class DatabaseContext : IAspect
    {
        private readonly ITransactionContext _transactionContext;

        public DatabaseContext(ITransactionContext transactionContext)
        {
            _transactionContext = transactionContext;
        }
        
        public object Execute(Func<object[], object> proceedInvocation, object[] args, IAspectEnvironment method)
        {
            _transactionContext.Push();
            object returnValue = null;

            try
            {
                returnValue = proceedInvocation.Invoke(args);
            }
            catch(Exception ex)
            {
                _transactionContext.Rollback();
                throw ex;
            }
            _transactionContext.Commit();

            return returnValue;
        }
    }

    public interface ITransactionContext
    {
        void Push();
        void Rollback();
        void Commit();
    }

    public class TransactionContext : ITransactionContext
    {
        public void Push()
        {
            
        }

        public void Rollback()
        {
            
        }

        public void Commit()
        {
            
        }
    }
}

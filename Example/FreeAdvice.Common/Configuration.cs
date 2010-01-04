using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeAdvice.Common
{
    public interface IConfiguration
    {
        Guid GetDefaultDocumentFolder { get; }
    }

    public class Configuration : IConfiguration
    {
        private readonly Guid defaultFolder = new Guid("{52E807B7-8A73-4756-B7E8-EBDBBC9B0C62}");

        public Guid GetDefaultDocumentFolder
        {
            get { return defaultFolder; }
        }
    }
}

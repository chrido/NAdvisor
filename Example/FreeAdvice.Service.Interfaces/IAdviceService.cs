using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeAdvice.Domain;

namespace FreeAdvice.Service.Interfaces
{
    public interface IAdviceService
    {
        AdviceDto GetAdvice(Guid id);
        IEnumerable<AdviceDto> GetAllAdvices();
        void UpdateAdvice(AdviceDto dto);
    }
}

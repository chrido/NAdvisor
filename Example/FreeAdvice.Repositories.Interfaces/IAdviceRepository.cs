using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeAdvice.Domain;

namespace FreeAdvice.Repositories.Interfaces
{
    public interface IAdviceRepository
    {
        AdviceDto GetById(Guid id);
        void UpdateAdvice(AdviceDto dto);
        IEnumerable<AdviceDto> GetAdviceByAdviceText(string text);
        IEnumerable<AdviceDto> GetAdviceByRandomNumber(int number);
        IEnumerable<AdviceDto> GetAllAdvices();
    }
}

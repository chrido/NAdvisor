using System;
using System.Collections.Generic;
using System.Linq;
using FreeAdvice.Aspects;
using FreeAdvice.Common;
using FreeAdvice.Domain;
using FreeAdvice.Repositories.Interfaces;
using FreeAdvice.Service.Interfaces;
using TechTalk.NAdvisor.Contrib;

namespace FreeAdvice.Services
{
    public class AdviceService : IAdviceService
    {
        private readonly IAdviceRepository _adviceRepository;
        private readonly IDms _dms;
        private readonly IConfiguration _configuration;

        public AdviceService(IAdviceRepository adviceRepository, IDms dms, IConfiguration configuration)
        {
            _adviceRepository = adviceRepository;
            _dms = dms;
            _configuration = configuration;
        }

        public AdviceDto GetAdvice(Guid id)
        {
            return _adviceRepository.GetById(id);
        }

        public IEnumerable<AdviceDto> GetAllAdvices()
        {
            return _adviceRepository.GetAllAdvices();
        }

        [InterceptedBy(typeof(DatabaseContext), typeof(ExternalDmsContext))]
        public void UpdateAdvice(AdviceDto dto)
        {
            IEnumerable<string> validationErrors = ValidateAdvice(dto);
            if (validationErrors.Count() != 0)
                throw new ValidationException("Validationerrors: " + validationErrors.Aggregate((m, o) => m + "; " + o));

            if (dto.Id == Guid.Empty)
                dto.Id = Guid.NewGuid();

            _dms.AddDocumentToFolder(_configuration.GetDefaultDocumentFolder, GenerateDocumentForAdvice(dto), GetFileNameForAdvice(dto));
            _adviceRepository.UpdateAdvice(dto);
        }

        private string GetFileNameForAdvice(AdviceDto dto)
        {
            return dto.Id.ToString() + ".doc";
        }

        private byte[] GenerateDocumentForAdvice(AdviceDto dto)
        {
            return new byte[3];
        }

        private IEnumerable<string> ValidateAdvice(AdviceDto dto)
        {
            if (!string.IsNullOrEmpty(dto.AdviceText) && dto.AdviceText.Length < 5)
                yield return "need more characters";

            if (dto.RandomNumber == default(int))
                yield return "need a random Number";
        }
    }
}

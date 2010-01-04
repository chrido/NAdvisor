using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeAdvice.Domain;
using FreeAdvice.Repositories.Interfaces;

namespace FreeAdvice.Repositories
{
    public class AdviceRepository : IAdviceRepository
    {
        private readonly Dictionary<Guid, AdviceDto> _advices;
        private readonly Random _random;

        public AdviceRepository()
        {
            _random = new Random();
            _advices = InitializeAdvices();
        }

        private Dictionary<Guid, AdviceDto> InitializeAdvices()
        {
            var advices = new Dictionary<Guid, AdviceDto>();
            for (int i = 0; i < 35; i++ )
            {
                var dto = CreateNewRandomDto();
                advices.Add(dto.Id, dto);
            }

            return advices;
        }

        private AdviceDto CreateNewRandomDto()
        {
            var advice = new AdviceDto();
            advice.Id = Guid.NewGuid();

            advice.AdviceText = string.Empty;
            for(int i = 0; i < _random.Next(3, 12); i++)
                advice.AdviceText += Convert.ToChar(Convert.ToInt32(Math.Floor(26*_random.NextDouble() + 65)));
            advice.RandomNumber = _random.Next(5);

            return advice;
        }



        public AdviceDto GetById(Guid id)
        {
            return _advices[id];
        }

        public void UpdateAdvice(AdviceDto dto)
        {
            _advices[dto.Id] = dto;
        }

        public IEnumerable<AdviceDto> GetAdviceByAdviceText(string text)
        {
            return _advices.Values.Where(ad => ad.AdviceText.Contains(text));
        }

        public IEnumerable<AdviceDto> GetAdviceByRandomNumber(int number)
        {
            return _advices.Values.Where(ad => ad.RandomNumber == number);
        }

        public IEnumerable<AdviceDto> GetAllAdvices()
        {
            return _advices.Values;
        }
    }
}

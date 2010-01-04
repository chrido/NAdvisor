using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeAdvice.Domain
{
    public class AdviceDto
    {
        public Guid Id { get; set; }
        public string AdviceText { get; set; }
        public int RandomNumber { get; set; }
    }
}

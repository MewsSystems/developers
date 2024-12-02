﻿using ExchangeRateUpdater.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Model.Dto
{
    public class ExchangeRateResponseDto : ResponseDto
    {
        public List<ExchangeRate> Rates { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class ExchangeRateList : List<ExchangeRate>
    {
        public new void Add(ExchangeRate newItem)
        {
            if(newItem==null) {throw new Exception(Res.ExchangeRateItemIsEmpty); }
            foreach(var item in this)
            {
                if (item == null) continue;
                if (newItem.SourceCurrency.IsEqual(item.SourceCurrency) && newItem.TargetCurrency.IsEqual(item.TargetCurrency))
                    throw new Exception(Res.DuplicateInExRateList);
            }
            base.Add(newItem);
        }
    }
}

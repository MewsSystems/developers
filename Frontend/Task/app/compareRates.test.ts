import { compareRates } from './compareRates';
import { Trend } from './Trend';

describe('compareRates', () => {
  const examples: ReadonlyArray<
    [string, number | null, number | null, Trend]
  > = [
    ['missing oldRate', 1, null, Trend.Stagnating],
    ['missing newRate', null, 1, Trend.Stagnating],
    ['newRate === oldRate', 1, 1, Trend.Stagnating],
    ['newRate > oldRate', 2, 1, Trend.Growing],
    ['newRate < oldRate', 1, 2, Trend.Declining],
  ];

  examples.forEach(([description, newRate, oldRate, expectedTrend]) => {
    test(description, () => {
      expect(compareRates(newRate, oldRate)).toEqual(expectedTrend);
    });
  });
});

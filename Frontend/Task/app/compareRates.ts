import { Trend } from './Trend';

export function compareRates(
  newRate: number | null,
  oldRate: number | null,
): Trend {
  if (typeof oldRate === 'number' && typeof newRate === 'number') {
    if (oldRate < newRate) {
      return Trend.Growing;
    }
    if (oldRate > newRate) {
      return Trend.Declining;
    }
  }
  return Trend.Stagnating;
}

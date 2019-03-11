import { resolveTrend } from './getRates';
import { Trend } from '../interface/currencyPairInterface';

describe('Rates operations test', () => {
    it('should change the trend', () => {
        expect(resolveTrend(100, 200)).toEqual(Trend.GROWING);
        expect(resolveTrend(200, 100)).toEqual(Trend.DECLINING);
        expect(resolveTrend(100, 100)).toEqual(Trend.STAGNATING);
    });
});

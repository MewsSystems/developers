import { expect, test } from 'vitest';
import { displayRatings, displayRuntime } from '../src/utils/movieUtils';

test('displayRuntime formats time', () => {
    expect(displayRuntime(0)).toEqual('0m');
    expect(displayRuntime(17)).toEqual('17m');
    expect(displayRuntime(59)).toEqual('59m');
    expect(displayRuntime(60)).toEqual('1h');
    expect(displayRuntime(61)).toEqual('1h 1m');
    expect(displayRuntime(82)).toEqual('1h 22m');
    expect(displayRuntime(120)).toEqual('2h');
    expect(displayRuntime(567)).toEqual('9h 27m');
});

test('displayRatings formats rating info', () => {
    expect(displayRatings(0, 0)).toEqual('0% from 0 ratings');
    expect(displayRatings(6, 5)).toEqual('60% from 5 ratings');
    expect(displayRatings(3.77, 1111)).toEqual('38% from 1,111 ratings');
    expect(displayRatings(8.44, 123_456_789)).toEqual('84% from 123,456,789 ratings');
    expect(displayRatings(8.45, 123_456_789)).toEqual('85% from 123,456,789 ratings');
});
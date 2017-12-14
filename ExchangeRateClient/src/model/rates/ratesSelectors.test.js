import {matchesFilter} from './ratesSelectors';

it('matches filter', () => {
    const example = {"code":"AMD","name":"Armenia Dram"};

    expect(matchesFilter(example, '')).toBe(true);
    expect(matchesFilter(example, 'MD')).toBe(true);
    expect(matchesFilter(example, 'md')).toBe(true);
    expect(matchesFilter(example, 'rme')).toBe(true);
    expect(matchesFilter(example, 'RME')).toBe(true);
    expect(matchesFilter(example, 'asdf')).toBe(false);
});


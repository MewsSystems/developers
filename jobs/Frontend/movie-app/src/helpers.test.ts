import { createFetchUrl, formatDate } from './helpers';

describe('helpers', () => {
    describe('formatDate', () => {
        it('should format a date string', () => {
            const inputDate = '2022-01-01T00:00:00Z';

            const formattedDate = formatDate(inputDate);

            expect(formattedDate).toBe('Jan 1, 2022');
        });
    });

    describe('createFetchUrl', () => {
        beforeEach(() => {
            process.env.REACT_APP_MOVIE_API_KEY = 'some-api-key';
        })

        it('should create an URL with given path and params', () => {
            const path = 'movie';
            const params = { page: 1 };

            const url = createFetchUrl(path, params);

            expect(url).toBe('https://api.themoviedb.org/3/movie?api_key=some-api-key&page=1');
        });
    });
})


import { mapSearchMovieToParams } from './movie.mapper'

describe('movie.mapper', () => {
    /*
     * I only wrote test for this one mapper as it is the only one that a bit of data manipulation
     */
    it('mapSearchMovieToParams', () => {
        const data = {
            query: 'test',
            page: 1,
        }
        const result = mapSearchMovieToParams(data)
        expect(result).toEqual({
            query: 'test',
            page: '1',
        })
    })
})

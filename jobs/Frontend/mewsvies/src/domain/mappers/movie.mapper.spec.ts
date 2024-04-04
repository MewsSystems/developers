import { mapSearchMovieToParams } from './movie.mapper'

describe('movie.mapper', () => {
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

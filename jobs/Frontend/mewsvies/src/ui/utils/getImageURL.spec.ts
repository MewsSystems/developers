import { getImageURL } from './getImageURL'

describe('getImageURL', () => {
    it('should return the image URL', () => {
        const posterPath = '/test-path'
        const result = getImageURL(posterPath)
        expect(result).toEqual('https://image.tmdb.org/t/p/w500/test-path')
    })
})

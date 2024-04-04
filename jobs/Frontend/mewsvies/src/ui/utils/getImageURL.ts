import { IMAGE_BASE_URL } from '../constants/movie.constant.ts'

export const getImageURL = (posterPath: string): string => {
    return `${IMAGE_BASE_URL}${posterPath}`
}

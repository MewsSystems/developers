import { render, screen } from '@testing-library/react'
import { expect, test } from 'vitest'
import { MoviePoster } from './MoviePoster'
import fallbackImageSrc from '../assets/fallback-image.png'

describe('Movie posters renders with right items', () => {
    test('renders poster with src', () => {
        const posterPath = '/poster.jpg'
        render(
            <MoviePoster
                poster_path={posterPath}
                title='Test Movie'
            />,
        )

        const image = screen.getByRole('img')
        expect(image).toHaveAttribute(
            'src',
            `https://media.themoviedb.org/t/p/w220_and_h330_face${posterPath}`,
        )
    })

    test('renders poster with fallback image', () => {
        render(<MoviePoster title='Test Movie' />)

        const image = screen.getByRole('img')
        expect(image).toHaveAttribute('src', fallbackImageSrc)
    })
})

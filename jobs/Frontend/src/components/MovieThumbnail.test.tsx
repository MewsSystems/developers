// Import testing utilities and the component to be tested
import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, fireEvent, screen } from '@testing-library/react'
import { BrowserRouter } from 'react-router-dom'
import { MovieThumbnail } from './MovieThumbnail'
import type { MovieThumbnailProps } from '../types'
import { useNavigate } from 'react-router-dom'

vi.mock('react-router-dom', async () => {
    const actual = await import('react-router-dom')
    return {
        ...actual,
        useNavigate: vi.fn(),
    }
})

const mockNavigate = vi.fn()

const movieProps: MovieThumbnailProps = {
    id: 1,
    poster_path: 'url_to_poster',
    title: 'Example Movie',
    release_date: '2021-01-01',
}

describe('MovieThumbnail', () => {
    beforeEach(() => {
        // Reset the mockNavigate function before each test
        mockNavigate.mockReset()
        vi.mocked(useNavigate).mockImplementation(() => mockNavigate)
    })

    test('renders correctly with given props', () => {
        render(
            <BrowserRouter>
                <MovieThumbnail {...movieProps} />
            </BrowserRouter>,
        )

        expect(screen.getByText(movieProps.title)).toBeInTheDocument()
        expect(
            screen.getByText(movieProps.release_date.substring(0, 4)),
        ).toBeInTheDocument()
        expect(
            screen.getByRole('button', { name: /see details/i }),
        ).toBeInTheDocument()
    })

    test('navigates to the movie details page on button click', () => {
        render(
            <BrowserRouter>
                <MovieThumbnail {...movieProps} />
            </BrowserRouter>,
        )

        const detailsButton = screen.getByRole('button', {
            name: /see details/i,
        })
        fireEvent.click(detailsButton)

        expect(mockNavigate).toHaveBeenCalledWith(`/movie/${movieProps.id}`)
    })
})

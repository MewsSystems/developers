// HomeSearchContentView.test.tsx
import { describe, expect, it, test, vi } from 'vitest'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { HomeSearchContentView } from './HomeSearchContentView'
import type { HomeSearchContentProps } from '../../types'
import { useNavigate } from 'react-router-dom'
import { MovieSearchCollection } from '../../../../api/movies/types'

// Mock Props
const mockProps: HomeSearchContentProps = {
    isLoading: false,
    isError: false,
    searchData: { results: [], total_pages: 0, page: 1, total_results: 0 },
    handleChangePage: vi.fn(),
    prefetchMovieData: vi.fn(),
}

vi.mock('react-router-dom', async () => {
    const actual = await import('react-router-dom')
    return {
        ...actual,
        useNavigate: vi.fn(),
    }
})

const mockNavigate = vi.fn()

describe('HomeSearchContentView', () => {
    beforeEach(() => {
        mockNavigate.mockReset()
        vi.mocked(useNavigate).mockImplementation(() => mockNavigate)
    })

    test('renders without crashing', () => {
        render(<HomeSearchContentView {...mockProps} />)
        expect(screen.getByRole('region')).toBeInTheDocument()
    })

    test('displays error message when isError is true', () => {
        render(
            <HomeSearchContentView
                {...mockProps}
                isError={true}
            />,
        )
        expect(
            screen.getByText(
                /There has been an error with the search. Please try again./,
            ),
        ).toBeInTheDocument()
    })

    test('displays no data text when searchData results are empty', () => {
        render(<HomeSearchContentView {...mockProps} />)
        expect(
            screen.getByText(
                /There doesn't seem to be any movie with that title. Please try another./,
            ),
        ).toBeInTheDocument()
    })

    test('pagination is rendered and interacts correctly when data is present', async () => {
        const user = userEvent.setup()

        const searchData: MovieSearchCollection = {
            results: [
                {
                    id: 1,
                    poster_path: 'url_to_poster',
                    title: 'Example Movie',
                    release_date: '2021-01-01',
                },
            ],
            total_pages: 2,
            page: 1,
            total_results: 1,
        }

        render(
            <HomeSearchContentView
                {...mockProps}
                searchData={searchData}
            />,
        )
        const pageButton = screen.getByRole('button', { name: /page 2/i })
        await user.click(pageButton)
        expect(mockProps.handleChangePage).toHaveBeenCalled()
    })
})

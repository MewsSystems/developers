import { fireEvent, render } from '@testing-library/react'
import { describe, expect, it, vi } from 'vitest'

import HomePage from './HomePage'

vi.mock('react-router-dom', () => ({
    ...vi.importActual('react-router-dom'),
    useNavigate: () => vi.fn(),
}))

// Mock the useSearchMovie hook
vi.mock('@/hooks/movies/useSearchMovie.ts', () => ({
    __esModule: true,
    useSearchMovie: () => ({
        page: 1,
        totalPages: 3,
        currentSearch: 'test',
        searchRef: { current: null },
        results: [{ id: 1, title: 'Test Movie' }],
        handleSearchChange: vi.fn(),
        handlePageChange: vi.fn(),
        handleClear: vi.fn(),
        isLoading: false,
        isError: false,
    }),
}))

// Define test cases
describe('HomePage', () => {
    it('renders the homepage with search form', () => {
        const { getByText, getByLabelText } = render(<HomePage />)

        // Check if the title is rendered
        expect(getByText('Lights, Camera, Search!')).toBeInTheDocument()

        // Check if the search form is rendered
        expect(getByLabelText('Search movie')).toBeInTheDocument()
    })

    it('displays search results when available', () => {
        const { getByText } = render(<HomePage />)

        // Check if the search results are rendered
        expect(getByText('Test Movie')).toBeInTheDocument()
    })

    it('displays pagination when search results span multiple pages', () => {
        const { getByRole } = render(<HomePage />)

        // Check if the pagination component is rendered
        expect(getByRole('navigation')).toBeInTheDocument()
    })

    it('executes search when user types into the search input', () => {
        const { getByTestId } = render(<HomePage />)

        // Type into the search input
        fireEvent.change(getByTestId('search'), {
            target: { value: 'New Search' },
        })

        expect(getByTestId('search')).toHaveValue('New Search')
    })
})

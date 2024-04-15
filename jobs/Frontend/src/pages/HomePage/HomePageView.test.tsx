import { describe, expect, test, vi } from 'vitest'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { HomePageView } from './HomePageView'
import type { HomePageViewProps } from './types'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { DebouncedFunc } from 'lodash'

interface MockDebouncedFunction
    extends DebouncedFunc<(movieTitle: string) => void> {
    cancel: () => void
    flush: () => void
}

// Create a correctly structured mock function
const mockDebouncedFunction: MockDebouncedFunction = Object.assign(
    vi.fn((movieTitle: string) => {
        console.log('Debounced function called with:', movieTitle)
    }),
    {
        cancel: vi.fn(),
        flush: vi.fn(),
    },
)

const mockProps: HomePageViewProps = {
    movieSearch: {
        clearTitle: vi.fn(),
        currentSearchTitle: '',
        handleChangePage: vi.fn(),
        isError: false,
        isLoading: false,
        searchData: undefined,
        searchInputRef: { current: null },
        submitSearchedTitle: mockDebouncedFunction,
    },
}

const createTestQueryClient = () =>
    new QueryClient({
        defaultOptions: {
            queries: {
                retry: false, // Important for tests to avoid retries
            },
        },
    })

const renderWithClient = (ui: React.ReactElement) => {
    const testQueryClient = createTestQueryClient()
    return {
        ...render(
            <QueryClientProvider client={testQueryClient}>
                {ui}
            </QueryClientProvider>,
        ),
        testQueryClient,
    }
}

describe('HomePageView', () => {
    test('renders correctly', () => {
        const { container } = render(<HomePageView {...mockProps} />)
        expect(container).toBeDefined()
    })

    test('allows typing in search input', async () => {
        const user = userEvent.setup()
        render(<HomePageView {...mockProps} />)
        const input = screen.getByPlaceholderText(/batman: begins.../i)
        await user.type(input, 'Batman')
        expect(mockProps.movieSearch.submitSearchedTitle).toHaveBeenCalledWith(
            'Batman',
        )
    })

    test('shows helper text if less than 3 characters are typed', () => {
        render(
            <HomePageView
                {...{
                    ...mockProps,
                    movieSearch: {
                        ...mockProps.movieSearch,
                        currentSearchTitle: 'Ba',
                    },
                }}
            />,
        )
        expect(
            screen.getByText(
                /you need to type at least 3 characters to search/i,
            ),
        ).toBeInTheDocument()
    })

    test('does not show HomeSearchContent when fewer than 3 characters are typed', () => {
        render(<HomePageView {...mockProps} />)
        expect(
            screen.queryByText(/HomeSearchContent component content here/i),
        ).toBeNull()
    })

    test('clears input when clear button is clicked', async () => {
        const user = userEvent.setup()
        renderWithClient(
            <HomePageView
                {...{
                    ...mockProps,
                    movieSearch: {
                        ...mockProps.movieSearch,
                        currentSearchTitle: 'Batman',
                    },
                }}
            />,
        )
        const clearButton = screen.getByTestId('clear-search')
        await user.click(clearButton)
        expect(mockProps.movieSearch.clearTitle).toHaveBeenCalled()
    })

    test('renders HomeSearchContent when 3 or more characters are typed', () => {
        const propsWithSearchData = {
            ...mockProps,
            movieSearch: {
                ...mockProps.movieSearch,
                currentSearchTitle: 'Batman',
            },
        }
        renderWithClient(<HomePageView {...propsWithSearchData} />)
        // Assuming 'HomeSearchContent' has some identifiable text or element
        expect(screen.getByRole('region')).toBeInTheDocument()
    })
})

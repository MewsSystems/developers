import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, MatcherFunction } from '@testing-library/react'
import { PostersContent } from './PostersContent'
import { useNavigate } from 'react-router-dom'
import type { PosterContentProps } from '../types'

vi.mock('react-router-dom', async () => {
    const actual = await import('react-router-dom')
    return {
        ...actual,
        useNavigate: vi.fn(),
    }
})

const mockNavigate = vi.fn()

const textMatcher: MatcherFunction = (content, element) => {
    const noDataText = 'No movies found'
    if (!element) return false

    const hasText = (node: Element) =>
        node.textContent?.includes(noDataText) || false
    const nodeHasText = hasText(element)
    const childrenDontHaveText = Array.from(element.children).every(
        (child) => !hasText(child),
    )

    return nodeHasText && childrenDontHaveText
}

describe('PostersContent', () => {
    const errorText = 'An error occurred'
    const noDataText = 'No movies found'

    beforeEach(() => {
        mockNavigate.mockReset()
        vi.mocked(useNavigate).mockImplementation(() => mockNavigate)
    })

    test('displays skeletons when loading', () => {
        const props: PosterContentProps = {
            searchData: undefined,
            isLoading: true,
            isError: false,
            prefetchFunction: vi.fn(),
            errorText,
            noDataText,
        }

        render(<PostersContent {...props} />)

        // Check for skeleton elements
        const skeletons = screen.getAllByTestId('Skeleton')
        expect(skeletons.length).toBe(3) // As per loading mock-up
    })

    test('displays error message when there is an error', () => {
        const props: PosterContentProps = {
            searchData: undefined,
            isLoading: false,
            isError: true,
            prefetchFunction: vi.fn(),
            errorText,
            noDataText,
        }

        render(<PostersContent {...props} />)

        expect(screen.getByText(errorText)).toBeInTheDocument()
    })

    it('displays no data message when there are no results', () => {
        const props: PosterContentProps = {
            searchData: {
                results: [],
                page: 1,
                total_pages: 1,
                total_results: 0,
            },
            isLoading: false,
            isError: false,
            prefetchFunction: vi.fn(),
            errorText: 'An error occurred',
            noDataText: 'No movies found',
        }

        render(<PostersContent {...props} />)

        // Use the custom text matcher
        expect(screen.getByText(textMatcher)).toBeInTheDocument()
    })

    test('displays movie thumbnails when data is available', () => {
        const props: PosterContentProps = {
            searchData: {
                page: 1,
                total_pages: 1,
                total_results: 2,
                results: [
                    {
                        id: 1,
                        title: 'Movie 1',
                        poster_path: 'path1',
                        release_date: '2021-01-01',
                    },
                    {
                        id: 2,
                        title: 'Movie 2',
                        poster_path: 'path2',
                        release_date: '2021-02-01',
                    },
                ],
            },
            isLoading: false,
            isError: false,
            prefetchFunction: vi.fn(),
            errorText,
            noDataText,
        }

        render(<PostersContent {...props} />)

        const movieTitles = screen.getAllByText(/Movie/)
        expect(movieTitles.length).toBe(2)
    })
})

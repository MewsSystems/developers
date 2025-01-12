import { render, screen } from '@testing-library/react'
import { expect, test } from 'vitest'
import { TopSummary } from './TopSummary'

const mockData = {
    popularity: 80,
    id: 1,
    vote_average: 8,
    vote_count: 200,
    release_date: '2020-01-01',
    original_language: 'en',
    runtime: 120,
    genres: [
        { id: 1, name: 'Adventure' },
        { id: 2, name: 'Comedy' },
    ],
    title: 'Sample Movie',
    overview: 'A fun adventure.',
}

test('renders with data and shows all details', async () => {
    const { container } = render(
        <TopSummary
            detailData={mockData}
            isLoading={false}
        />,
    )

    expect(screen.getByText('80%')).toBeInTheDocument()

    expect(screen.getByText('total: 200')).toBeInTheDocument()

    container.querySelectorAll('.MuiChip-root').forEach((chip) => {
        expect(chip).toBeInTheDocument()
    })

    expect(screen.getByText('Sample Movie')).toBeInTheDocument()
    expect(screen.getByText('A fun adventure.')).toBeInTheDocument()
})

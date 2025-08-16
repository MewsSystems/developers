import { render } from '@testing-library/react'
import { describe, expect, it } from 'vitest'

import MovieDetailInformation from './MovieDetailInformation'

import { movieDetailInformationMock } from '@/tests/mocks/movieMocks.ts'

describe('MovieDetailInformation', () => {
    it('renders MovieDetailInformation component with provided props', () => {
        const props = movieDetailInformationMock[0]

        const { getByTestId } = render(<MovieDetailInformation {...props} />)

        expect(getByTestId('movie-genre')).toBeInTheDocument()
        expect(getByTestId('movie-genre')).toHaveTextContent(
            'Action / Adventure'
        )
        expect(getByTestId('movie-release')).toBeInTheDocument()
        expect(getByTestId('movie-origin')).toBeInTheDocument()
        expect(getByTestId('movie-origin')).toHaveTextContent(
            'Origins: Original Title (English)'
        )
    })

    it('renders movie information with correct language name for French', () => {
        const props = movieDetailInformationMock[1]

        const { getByTestId } = render(<MovieDetailInformation {...props} />)

        expect(getByTestId('movie-origin')).toHaveTextContent(
            'Origins: Titre Original (French)'
        )
    })

    it('renders only the year of the release date', () => {
        const props = movieDetailInformationMock[0]

        const { getByTestId } = render(<MovieDetailInformation {...props} />)

        expect(getByTestId('movie-release')).toBeInTheDocument()
        // Check that the year is visible
        expect(getByTestId('movie-release')).toHaveTextContent('2022')
        // Check that the month/day is not visible
        expect(getByTestId('movie-release')).not.toHaveTextContent('01')
        // Check that the nothing else is visible
        expect(getByTestId('movie-release')).not.toHaveTextContent('Jan') // Check that the year is visible
    })
})

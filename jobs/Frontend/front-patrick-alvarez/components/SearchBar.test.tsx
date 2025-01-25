import { fireEvent, render, screen } from '@testing-library/react'
import SearchBar from './SearchBar'

describe('SearchBar', () => {
    it('renders with default placeholder', () => {
        render(<SearchBar />)

        const searchInput = screen.getByPlaceholderText('Search for a movie')
        expect(searchInput).toBeInTheDocument()
    })

    it('renders with custom placeholder', () => {
        render(<SearchBar placeholder="Custom placeholder" />)

        const searchInput = screen.getByPlaceholderText('Custom placeholder')
        expect(searchInput).toBeInTheDocument()
    })

    it('updates input value when typing', () => {
        render(<SearchBar />)

        const searchInput = screen.getByPlaceholderText('Search for a movie')
        fireEvent.change(searchInput, { target: { value: 'test movie' } })

        expect(searchInput).toHaveValue('test movie')
    })

    it('calls handleSearch callback when typing', () => {
        const handleSearch = jest.fn()
        render(<SearchBar handleSearch={handleSearch} />)

        const searchInput = screen.getByPlaceholderText('Search for a movie')
        fireEvent.change(searchInput, { target: { value: 'test movie' } })

        expect(handleSearch).toHaveBeenCalledWith('test movie')
    })

    it('applies custom className when provided', () => {
        render(<SearchBar className="custom-class" />)

        const searchInput = screen.getByPlaceholderText('Search for a movie')
        expect(searchInput).toHaveClass('custom-class')
    })
})

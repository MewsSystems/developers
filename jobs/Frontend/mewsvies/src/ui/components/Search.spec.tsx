import { render, fireEvent, screen } from '@testing-library/react'
import Search from './Search.tsx'

describe('Search', () => {
    it('should call onSearch when typing', async () => {
        const onSearch = jest.fn()
        render(<Search query="" onSearch={onSearch} />)
        const input = await screen.findByPlaceholderText(
            'Type to search a movie'
        )

        fireEvent.change(input, { target: { value: 'matrix' } })

        expect(onSearch).toHaveBeenCalledWith('matrix')
    })
})

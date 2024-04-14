import { render, screen } from '@testing-library/react'
import user from '@testing-library/user-event'
import App from './App'
import { describe, expect, it } from 'vitest'

describe('App', () => {
    it('renders the App component with main search input', () => {
        render(<App />)

        const input = screen.getByRole('textbox')
        expect(input).toBeInTheDocument()
    })

    it('can type in the search input', async () => {
        render(<App />)

        const input = screen.getByRole('textbox')
        await user.type(input, 'test')
        expect(input).toHaveValue('test')
    })
})

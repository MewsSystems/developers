import { render, screen } from '@testing-library/react'
import App from './App'
import { describe, expect, it } from 'vitest'

describe('App', () => {
    it('renders the App component with main search input', () => {
        render(<App />)

        const input = screen.getByRole('textbox')
        expect(input).toBeInTheDocument()
    })
})

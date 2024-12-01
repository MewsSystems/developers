import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import Footer from './Footer'

describe('Footer', () => {
  it('renders the footer with correct text and styles', () => {
    render(<Footer />)

    // Assert that the footer element is rendered
    const footer = screen.getByRole('contentinfo')
    expect(footer).toBeInTheDocument()

    // Assert that the footer contains the correct text
    const text = screen.getByText(/made with ❤️ by raffaele nicosia/i)
    expect(text).toBeInTheDocument()

    // Assert that the footer has the correct styles
    expect(footer).toHaveStyle({
      backgroundColor: 'var(--primary-brand-color-400)',
      padding: '1rem',
      height: '80px',
      flexShrink: '0',
    })
  })
})

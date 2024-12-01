import { describe, it, expect } from 'vitest'
import { render, screen } from '@testing-library/react'
import Header from './Header'

describe('Header', () => {
  it('renders the header with correct styles', () => {
    render(<Header />)

    // Select the styled header element
    const header = screen.getByRole('banner')

    // Assert that it has the correct styles
    expect(header).toHaveStyle({
      flexShrink: '0',
      height: '175px',
      padding: '1rem 1rem 0',
      backgroundImage: "url('/images/movie-posters-collage.jpg')",
      backgroundSize: 'cover',
    })
  })
})

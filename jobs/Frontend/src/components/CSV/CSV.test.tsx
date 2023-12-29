import '@testing-library/jest-dom'

import { render, screen } from '@testing-library/react'

import { CSV } from './CSV'

describe('CSV', () => {
  it('renders values with commas', () => {
    const names = ['Petr Kubicek', 'Anna Smith', 'John Doe']

    render(
      <>
        {names.map((name, index) => (
          <CSV value={name} key={index} index={index} />
        ))}
      </>,
    )

    const expectedText = names.join(', ')
    expect(screen.getByText(expectedText)).toBeInTheDocument()
  })
})

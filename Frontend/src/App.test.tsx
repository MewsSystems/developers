import React from 'react'
import { render } from '@testing-library/react'
import App from './App'

test('renders learn react link', () => {
  const { getByText } = render(<App />)
  const linkElement = getByText(/learn react/i)
  expect(linkElement).toBeInTheDocument()
})

function Button({ children }: React.PropsWithChildren<{}>) {
  return <button type="button">{children}</button>
}

test('renders without crashing', () => {
  const { container } = render(<Button>hello</Button>)
  expect(container).toBeInTheDocument()
})

import '@testing-library/jest-dom'
import { render, screen } from '@testing-library/react'
import { Searchbar } from './Searchbar'

jest.mock('use-debounce', () => ({
  useDebouncedCallback: jest.fn((fn) => fn),
}))

jest.mock('next/navigation', () => ({
  useSearchParams: () => new URLSearchParams(),
  useRouter: () => ({ push: jest.fn() }),
  usePathname: () => '/',
}))

jest.mock('@/hooks', () => ({
  useIsMobile: () => ({ isMobile: false }),
}))

describe('Searchbar', () => {
  it('renders with the correct placeholder', () => {
    render(
      <Searchbar
        placeholder="Search..."
        onChange={jest.fn()}
        onLoad={jest.fn()}
      />,
    )
    expect(screen.getByPlaceholderText('Search...')).toBeInTheDocument()
  })
})

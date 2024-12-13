import { describe, it, expect, vi } from 'vitest'
import { render, screen, fireEvent } from '@testing-library/react'
import Pagination from './Pagination'

describe('Pagination', () => {
  const mockSetCurrentPage = vi.fn()

  const renderComponent = (currentPage: number, totalPages: number) => {
    render(
      <Pagination
        currentPage={currentPage}
        setCurrentPage={mockSetCurrentPage}
        totalPages={totalPages}
      />
    )
  }

  it('renders the correct current page', () => {
    renderComponent(3, 10)

    const currentPageDisplay = screen.getByText('3')
    expect(currentPageDisplay).toBeInTheDocument()
  })

  it('disables "First" and "Previous" buttons on the first page', () => {
    renderComponent(1, 10)

    const firstButton = screen.getByRole('button', { name: /first/i })
    const previousButton = screen.getByRole('button', { name: /previous/i })

    expect(firstButton).toBeDisabled()
    expect(previousButton).toBeDisabled()
  })

  it('disables "Next" and "Last" buttons on the last page', () => {
    renderComponent(10, 10)

    const nextButton = screen.getByRole('button', { name: /next/i })
    const lastButton = screen.getByRole('button', { name: /last/i })

    expect(nextButton).toBeDisabled()
    expect(lastButton).toBeDisabled()
  })

  it('calls setCurrentPage with correct values when buttons are clicked', () => {
    renderComponent(5, 10)

    const firstButton = screen.getByRole('button', { name: /first/i })
    // const previousButton = screen.getByRole('button', { name: /previous/i })
    // const nextButton = screen.getByRole('button', { name: /next/i })
    const lastButton = screen.getByRole('button', { name: /last/i })

    // fireEvent.click(nextButton)
    // expect(mockSetCurrentPage).toHaveBeenCalledWith(6)

    // fireEvent.click(previousButton)
    // expect(mockSetCurrentPage).toHaveBeenCalledWith(5)

    fireEvent.click(firstButton)
    expect(mockSetCurrentPage).toHaveBeenCalledWith(1)
    console.info('First', mockSetCurrentPage)

    fireEvent.click(lastButton)
    expect(mockSetCurrentPage).toHaveBeenCalledWith(10)
  })
})

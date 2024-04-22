import { render } from '@testing-library/react'
import { describe, expect, it, vi } from 'vitest'

import NotFoundPage from './NotFoundPage'

vi.mock('react-router-dom', () => ({
    ...vi.importActual('react-router-dom'),
    useNavigate: () => vi.fn(),
}))

describe('NotFoundPage', () => {
    it('renders NotFoundPage component with correct text', () => {
        const { getByText } = render(<NotFoundPage />)

        expect(getByText('404 - Page not found')).toBeInTheDocument()
    })
})

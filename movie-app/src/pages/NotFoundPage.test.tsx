import { render } from '@testing-library/react'
import { describe, expect, it } from 'vitest'

import NotFoundPage from './NotFoundPage'

describe('NotFoundPage', () => {
    it('renders NotFoundPage component with correct text', () => {
        const { getByText } = render(<NotFoundPage />)

        expect(getByText('404 - Page not found')).toBeInTheDocument()
    })
})

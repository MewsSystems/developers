import React from 'react'
import { render } from '@testing-library/react'
import { Button } from '../../src/components/Button/Button'

describe('Button', () => {
    it('should render Button correctly', function() {
        const { getByText } = render(<Button>Click</Button>)
        expect(getByText('Click'))
    })
})

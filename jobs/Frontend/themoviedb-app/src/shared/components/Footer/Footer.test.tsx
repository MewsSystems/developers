import { screen } from '@testing-library/react';
import { expect } from 'vitest';
import { render } from '../../../tests/utils';
import Footer from './Footer';

describe('Footer', () => {
    it('should render Footer', () => {
        render(<Footer />);
        const footerTitle = screen.getByText('The MovieDB app');
        expect(footerTitle).toBeInTheDocument();
    });
});

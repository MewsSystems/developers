import { screen } from '@testing-library/react';
import { expect } from 'vitest';
import { render } from '../../../tests/utils';
import Header from './Header';

describe('Header', () => {
    it('should render Header', () => {
        render(<Header />);
        const headerTitle = screen.getByText('The MovieDB app');
        expect(headerTitle).toBeInTheDocument();
    });

    it('should have user icon', () => {
        render(<Header />);
        const userIcon = screen.getByTestId('user');
        expect(userIcon).toBeInTheDocument();
    });
});

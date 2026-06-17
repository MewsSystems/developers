import { screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { render } from '../tests/utils';
import Movies from './Movies';

describe('Button', () => {
    it('should render Movies', () => {
        render(<Movies />);
        screen.debug();
        expect(screen.getByText('No results found.')).toBeInTheDocument();
    });

    it('should show movies that contain input text', async () => {
        render(<Movies />);
        const searchQuery = 'Bad boys';
        const inputEl = screen.getByRole('textbox');
        await userEvent.type(inputEl, searchQuery);
        expect(inputEl).toHaveValue(searchQuery);
    });
});

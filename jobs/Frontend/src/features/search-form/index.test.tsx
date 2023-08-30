import { Provider } from 'react-redux';
import { store } from '../../state/store';
import { SearchForm } from '.';
import { MemoryRouter } from 'react-router-dom';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';

describe('SearchForm', () => {
    it('renders without crashing', () => {
        render(
            <Provider store={store}>
                <MemoryRouter>
                    <SearchForm />
                </MemoryRouter>
            </Provider>
        );
    });

    it('renders the search input', () => {
        render(
            <Provider store={store}>
                <MemoryRouter>
                    <SearchForm />
                </MemoryRouter>
            </Provider>
        );
        expect(screen.getByPlaceholderText('Search for a movie...')).toBeInTheDocument();
    });

    it('provides search results to a query', async () => {
        render(
            <Provider store={store}>
                <MemoryRouter>
                    <SearchForm />
                </MemoryRouter>
            </Provider>
        );

        const searchInput = screen.getByPlaceholderText('Search for a movie...');
        searchInput.focus();
        userEvent.type(searchInput, 'batman');

        await waitFor(() => {
            const element = screen.getByText(/Batman Begins/i);
            expect(element).toBeDefined();
        }, { timeout: 4000 });
    });
});

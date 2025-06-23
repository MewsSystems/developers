import { fireEvent, render, screen, waitFor } from '@testing-library/react';
import MovieSearch from './MovieSearch';
import { BrowserRouter } from 'react-router';
import * as movieService from '../../services/movieService';

beforeEach(() => render(<BrowserRouter><MovieSearch/></BrowserRouter>));
afterEach(() => jest.restoreAllMocks());

test('renders input and table', () => {
    // Check search input is rendered
    expect(screen.getByRole('textbox')).toBeInTheDocument();
    // Check table is rendered
    expect(screen.getByRole('table')).toBeInTheDocument();
});

test('search results are correctly displayed', async () => {
    // Mock service's fetchMovies
    jest.spyOn(movieService, 'fetchMovies').mockResolvedValue({
        results: [
            {id: 1, title: 'The Movie', release_date: '2020-12-12', overview: "The overview of The Movie. Excellent"},
            {id: 2, title: 'The Movie 2', release_date: '2022-12-12', overview: "The overview of The Movie 2. Excellent too"}
        ],
        total_results: 2,
    });

    // Simulate user input
    const input = screen.getByRole('textbox') as HTMLInputElement;
    fireEvent.change(input, {target: {value: 'movie'}});

    // Check movie titles are displayed
    await waitFor(() => {
      expect(screen.getByText('The Movie')).toBeInTheDocument();
      expect(screen.getByText('The Movie 2')).toBeInTheDocument();
    });
});

test('search is debounced', async () => {
    // Mock service's fetchMovies
    jest.spyOn(movieService, 'fetchMovies').mockResolvedValue({
        results: new Array(2),
        total_results: 2,
    });

    // Simulate user input
    const input = screen.getByRole('textbox') as HTMLInputElement;
    fireEvent.change(input, {target: {value: 'movie'}})

    // Check API not called right away
    expect(movieService.fetchMovies).not.toHaveBeenCalled();

    // Check API called after delay
    await waitFor(() => {
      expect(movieService.fetchMovies).toHaveBeenCalled();
    });
});

test('page retrieved corresponds to clicked page', async () => {
    // Mock service's fetchMovies
    jest.spyOn(movieService, 'fetchMovies').mockResolvedValue({
        results:  new Array(20),
        total_results: 60,
    });

    // Simulate user input
    const input = screen.getByRole('textbox') as HTMLInputElement;
    const searchTerm = 'movie';
    fireEvent.change(input, {target: {value: searchTerm}});

    await waitFor(() => {
        // Check service is called to retrieve page 1
        expect(movieService.fetchMovies).toHaveBeenCalledWith(searchTerm, 1);
        // Check there's a page 3 button and simulate click
        const page3Button = screen.getByText('3', { selector: 'button' });
        expect(page3Button).toBeInTheDocument();
        fireEvent.click(page3Button);
        // Check service is called to retrieve page 3
        expect(movieService.fetchMovies).toHaveBeenCalledWith(searchTerm, 3);
    });
});

test('new search resets to page 1', async () => {
    // Mock service's fetchMovies
    jest.spyOn(movieService, 'fetchMovies').mockResolvedValue({
        results:  new Array(20),
        total_results: 60,
    });

    // Simulate user input
    const firstInput = screen.getByRole('textbox') as HTMLInputElement;
    fireEvent.change(firstInput, {target: {value: 'movie'}});

    await waitFor(() => {
        // Simulate page 3 button click
        const page3Button = screen.getByText('3', { selector: 'button' });
        fireEvent.click(page3Button);
        // Check current page is page 3
        const activePageButton = screen.getByRole('button', { current: true });
        expect(activePageButton.textContent).toBe('3');
    });

    // Simulate user second input
    const secondInput = screen.getByRole('textbox') as HTMLInputElement;
    fireEvent.change(secondInput, {target: {value: 'mov'}});

    // Check current page is page 1
    await waitFor(() => {
        const activePageButton = screen.getByRole('button', { current: true });
        expect(activePageButton.textContent).toBe('1');
    });
});
import { render, waitFor } from '@testing-library/react';
import { useGetMovieList } from '@/hooks/useMovieService';
import MovieTable from '@/components/MovieTable';
import '@testing-library/jest-dom';

jest.mock('../../hooks/useMovieService', () => ({
    useGetMovieList: jest.fn(),
}));

class IntersectionObserverMock {
    constructor() { }

    observe() { }
    unobserve() { }
}

beforeAll(() => {
    // @ts-ignore
    global.IntersectionObserver = IntersectionObserverMock;
});

describe('MovieTable Component', () => {
    test('renders movie tiles', async () => {
        const movieList = [
            { id: 1, title: 'Movie 1' },
            { id: 2, title: 'Movie 2' },
        ];

        useGetMovieList.mockReturnValue({
            data: { pages: [{ results: movieList }] },
            fetchNextPage: jest.fn(),
        });

        const { getByText } = render(<MovieTable searchTerm="test" />);

        await waitFor(() => {
            expect(getByText('Movie 1')).toBeInTheDocument();
            expect(getByText('Movie 2')).toBeInTheDocument();
        });
    });

});

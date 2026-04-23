import { render, screen, waitFor } from "@testing-library/react";
import MovieDetail from "./MovieDetail";
import { BrowserRouter } from "react-router";
import * as movieService from '../../services/movieService';

test('renders movie details', async () => {
  // Mock service's fetchMovie
  jest.spyOn(movieService, 'fetchMovie').mockResolvedValue({
    id: 1,
    title: "The Title",
    overview: "The overview",
    poster_path: "posterpath",
    release_date: "2020/01/01",
  });

  // Mock useParams
  jest.mock('react-router', () => ({
    useParams: () => ({ id: '1' }),
  }));

  render(<BrowserRouter><MovieDetail/></BrowserRouter>);

  await waitFor(() => {
    // Check image is rendered
    expect(screen.getByRole('img')).toBeInTheDocument();
    // Check title is rendered
    expect(screen.getByText('The Title')).toBeInTheDocument();
    // Check overview is rendered
    expect(screen.getByText('The overview')).toBeInTheDocument();
    // Check release date is rendered
    expect(screen.getByText('Release date: 2020/01/01')).toBeInTheDocument();
  });
});
import React from 'react';
import { render, screen } from '@testing-library/react';
import { MovieDetailTitle } from './MovieDetailTitle';

describe('movie detail title component', () => {
  it('doesnt render a release date element when not provided', () => {
    render(<MovieDetailTitle title="Movie" genres={['a', 'b']} />);
    const releaseDateTitle = screen.queryByText('Release date');
    expect(releaseDateTitle).not.toBeInTheDocument();
  });

  it('renders a release date element provided', () => {
    render(<MovieDetailTitle title="Movie" releaseDate='2020-01-01' />);

    const releaseDateTitle = screen.queryByText('Release date');
    const releaseDate = screen.queryByText('2020-01-01');

    expect(releaseDateTitle).toBeInTheDocument();
    expect(releaseDate).toBeInTheDocument();
  });

  it('doesnt render a genre element when not provided', () => {
    render(<MovieDetailTitle title="Movie" releaseDate='2020-01-01' />);
    const genresTitle = screen.queryByText('Genres');
    expect(genresTitle).not.toBeInTheDocument();
  });

  it('renders a genre element provided', () => {
    render(<MovieDetailTitle title="Movie" genres={['a', 'b']} />);

    const genresTitle = screen.queryByText('Genres');
    const genres = screen.queryByText('a, b');

    expect(genresTitle).toBeInTheDocument();
    expect(genres).toBeInTheDocument();
  });

  it('doesnt render a runtime element when not provided', () => {
    render(<MovieDetailTitle title="Movie" releaseDate='2020-01-01' />);
    const runtimeTitle = screen.queryByText('Runtime');
    expect(runtimeTitle).not.toBeInTheDocument();
  });

  it('renders a runtime element provided', () => {
    render(<MovieDetailTitle title="Movie" runtime={105} />);
    
    const runtimeTitle = screen.queryByText('Runtime');
    const runtime = screen.queryByText('105 minutes');

    expect(runtimeTitle).toBeInTheDocument();
    expect(runtime).toBeInTheDocument();
  });
});

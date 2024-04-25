import { screen } from '@testing-library/react';
import { DetailsDisplay } from './Details';
import { jackReacherMovie, overview } from '../mockData/mockData';
import { setUp } from '../utils/testUtils';

describe('DetailsDisplay', () => {
  it('renders', async () => {
    setUp(<DetailsDisplay movie={jackReacherMovie} />);

    expect(await screen.findByText(/adult content: no/i)).toBeInTheDocument();
    expect(await screen.findByText(/Movie ID: 75780/i)).toBeInTheDocument();
    expect(
      await screen.findByText(`Overview: ${overview}`),
    ).toBeInTheDocument();
    expect(await screen.findByText(/popularity: 92.99/i)).toBeInTheDocument();
    expect(
      await screen.findByText(/release date: 20\/12\/2012/i),
    ).toBeInTheDocument();
    expect(
      await screen.findByText(/average rating: 6.62/i),
    ).toBeInTheDocument();
    expect(await screen.findByText(/vote count: 6748/i)).toBeInTheDocument();
  });
});

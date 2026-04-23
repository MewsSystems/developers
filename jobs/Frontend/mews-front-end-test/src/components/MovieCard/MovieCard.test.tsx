import { screen } from '@testing-library/react';
import { MovieCard } from './MovieCard';
import { jackReacher, jackReacherMovie } from '../../mockData/mockData';
import { setUp } from '../../utils/testUtils';

describe('MovieCard', () => {
  it('renders', async () => {
    setUp(<MovieCard movie={jackReacherMovie} />);

    expect(await screen.findByText(jackReacher)).toBeInTheDocument();
  });
});

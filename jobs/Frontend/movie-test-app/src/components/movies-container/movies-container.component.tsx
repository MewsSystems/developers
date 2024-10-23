import { MoviesGridContainer } from './movies-container.styles.tsx';

const MoviesContainer = ({ children }: { children: React.ReactNode[] }) => {
  return <MoviesGridContainer>{children}</MoviesGridContainer>;
};

export { MoviesContainer };

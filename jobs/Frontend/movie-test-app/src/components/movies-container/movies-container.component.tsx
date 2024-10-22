import { GridContainer } from './movies-container.styles.tsx';

const MoviesContainer = ({ children }: { children: React.ReactNode[] }) => {
  return <GridContainer>{children}</GridContainer>;
};

export { MoviesContainer };

import { StyledWrapperListMovies } from './WrapperListMovies.styles';

export const WrapperListMovies = ({ children, ...props }: { children: React.ReactNode }) => (
  <StyledWrapperListMovies {...props}>{children}</StyledWrapperListMovies>
);

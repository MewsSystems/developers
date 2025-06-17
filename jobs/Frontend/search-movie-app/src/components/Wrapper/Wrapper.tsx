import { StyledSearchMovieWrapper } from './Wrapper.styles';

const Wrapper = ({ children }: { children: React.ReactElement }) => {
  return <StyledSearchMovieWrapper>{children}</StyledSearchMovieWrapper>;
};

export { Wrapper };

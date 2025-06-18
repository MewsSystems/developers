import { Wrapper } from '../../../../components';
import {
  StyledNoMoviesFoundtitle,
  StyledNoMoviesFoundSubtitle,
  StyledNoMoviesFoundTextcontainer,
  StyledNoMoviesFoundImage,
} from './NoMoviesFound.styles';
import resultNotFound from '../../../../assets/result_not_found.png';

export const NoMoviesFound = () => {
  return (
    <Wrapper>
      <StyledNoMoviesFoundTextcontainer data-testid="no-movies-found">
        <StyledNoMoviesFoundImage src={resultNotFound} alt="Result not found" loading="lazy" />
        <StyledNoMoviesFoundtitle>Result not found</StyledNoMoviesFoundtitle>
        <StyledNoMoviesFoundSubtitle>
          We couln't find what you're looking for
        </StyledNoMoviesFoundSubtitle>
      </StyledNoMoviesFoundTextcontainer>
    </Wrapper>
  );
};

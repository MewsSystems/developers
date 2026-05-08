import { Wrapper } from '../../../../components';
import {
  StyledErrorLayoutTitle,
  StyledErrorLayoutSubtitle,
  StyledErrorLayoutTextcontainer,
  StyledErrorLayoutImage,
} from './ErrorLayout.styles';
import resultNotFound from '../../../../assets/result_not_found.png';
import { NO_MOVIES_FOUND_TEST_ID } from '../../../../constants';

export const ErrorLayout = ({ title, subtitle }: { title: string; subtitle?: string }) => {
  return (
    <Wrapper>
      <StyledErrorLayoutTextcontainer data-testid={NO_MOVIES_FOUND_TEST_ID}>
        <StyledErrorLayoutImage src={resultNotFound} alt={title} loading="lazy" />
        <StyledErrorLayoutTitle>{title}</StyledErrorLayoutTitle>
        {subtitle && <StyledErrorLayoutSubtitle>{subtitle}</StyledErrorLayoutSubtitle>}
      </StyledErrorLayoutTextcontainer>
    </Wrapper>
  );
};

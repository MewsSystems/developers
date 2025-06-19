import styled from 'styled-components';

const StyledErrorLayoutTitle = styled.p`
  font-size: 2.5rem;
  color: ${({ theme }) => theme.colors.onSurface};
  margin: 10px;
`;

const StyledErrorLayoutSubtitle = styled.p`
  font-size: 1.2rem;
  color: ${({ theme }) => theme.colors.onSurface};
`;

const StyledErrorLayoutTextcontainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
`;

const StyledErrorLayoutImage = styled.img`
  max-width: 400px;
  height: auto;
`;
export {
  StyledErrorLayoutImage,
  StyledErrorLayoutSubtitle,
  StyledErrorLayoutTextcontainer,
  StyledErrorLayoutTitle,
};

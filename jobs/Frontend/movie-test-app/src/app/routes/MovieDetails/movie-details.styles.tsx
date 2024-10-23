import styled from 'styled-components';
import { ColorsTheme } from '../../../assets/colors/theme-colors/colors.ts';

const MovieDetailsContainer = styled.div`
  display: flex;
  justify-content: start;
  align-items: center;
  flex-direction: column;
  color: ${ColorsTheme.primary};
  background: linear-gradient(
    to top,
    ${ColorsTheme.secondary} 0%,
    ${ColorsTheme.white}
  ); /* W3C, IE 10+/ Edge, Firefox 16+, Chrome 26+, Opera 12+, Safari 7+ */
  height: 100vh;
  white-space: break-spaces;
`;

const ImageContainer = styled.img`
  width: 100%;
  height: 100%;
  border-radius: 1rem;
`;

const OverviewContainer = styled.div`
  width: 60%;
  margin: 1rem;
`;

const DetailsContainer = styled.div`
  display: flex;
  justify-content: start;
  align-items: self-start;
  flex-direction: column;
  margin-left: 1rem;
  height: 100%;
`;

const MovieDetailGrid = styled.div`
  display: grid;
  grid-template-columns: 5fr 2fr;
  gap: 1rem;
  width: 60%;
`;
const RowCenteredContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: row;
`;

export {
  MovieDetailsContainer,
  ImageContainer,
  OverviewContainer,
  DetailsContainer,
  MovieDetailGrid,
  RowCenteredContainer,
};

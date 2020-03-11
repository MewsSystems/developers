import styled from 'styled-components';
import FlexContainer from './FlexContainer';

const MovieCard = styled(FlexContainer)`
  cursor: pointer;
  margin: 5px;
  display: flex;
  flex: 0 0 0;
  flex-direction: column;
  justify-content: space-between;
  border-style: solid;
  border-color: ${props => props.theme.border.primary};
  background-color: ${props => props.theme.background.withContent};
  border-radius: 10px;
  padding: 5px;
  min-width: 154px;
`;

export default MovieCard;

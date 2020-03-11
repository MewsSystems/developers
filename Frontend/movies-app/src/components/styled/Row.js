import styled from 'styled-components';

const Row = styled.div`
  width: 100%;
  display: flex;
  flex-direction: row;
  flex-wrap: wrap;
  justify-content: ${props => (props.align === 'right' ? 'flex-end' : 'flex-start')};
`;

export default Row;

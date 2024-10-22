import styled from 'styled-components';

const GridContainer = styled.div`
  display: grid;
  height: 100vh;
  grid-template-rows: 1fr 1fr 1fr;
  grid-template-columns: 1fr 1fr 1fr;
  grid-template-areas:
    'content content content'
    'content content content'
    'content content content';
  text-align: center;
  grid-gap: 0.25rem;
  transition: all 0.25s ease-in-out;
  color: white;
`;

export { GridContainer };

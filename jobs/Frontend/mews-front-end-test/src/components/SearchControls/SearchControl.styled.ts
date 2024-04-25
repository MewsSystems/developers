import styled from 'styled-components';

// Add the legend for the sake of exposing the component to Jest, but hide it.
const StyledLegend = styled.legend`
  position: absolute;
  overflow: hidden;
  height: 1px;
  width: 1px;
  margin: -1px;
  padding: 0;
`;

const StyledButton = styled.button`
  width: 25%;
  height: 2rem;
`;

const StyledFieldSet = styled.fieldset`
  padding: 0;
  border: none;
  display: flex;
  justify-content: space-around;
  align-items: center;
  margin-bottom: 1rem;
`;

export { StyledLegend, StyledButton, StyledFieldSet };

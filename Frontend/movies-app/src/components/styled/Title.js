import styled from 'styled-components';

const Title = styled.h1`
  font-style: ${props => (props.italic ? 'italic' : 'normal')};
  word-wrap: ${props => (props.wrap ? 'break-word' : 'normal')};
`;

export default Title;

import styled from 'styled-components';

const SubTitle = styled.h3`
  font-style: ${props => (props.italic ? 'italic' : 'normal')};
  word-wrap: ${props => (props.wrap ? 'break-word' : 'normal')};
`;

export default SubTitle;

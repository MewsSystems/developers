import styled from 'styled-components';

const getFontWeight = props => {
  if (props.bold) return 'bold';
  if (props.fontWeight) return props.fontWeight;
  return 'normal';
};

const Paragraph = styled.p`
  font-style: ${props => (props.italic ? 'italic' : 'normal')};
  font-weight: ${props => getFontWeight};
`;

export default Paragraph;

import styled from 'styled-components';

const Link = styled.a`
  color: ${props => props.theme.text.primary};
  &:hover {
    color: ${props => props.theme.text.disabled};
  }
`;

export default Link;

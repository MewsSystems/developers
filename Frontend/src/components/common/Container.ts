import styled from 'styled-components';

interface ContainerProps {
  isFullWidth?: boolean;
  paddingX?: number;
}

const Container = styled.div.attrs((props: ContainerProps) => ({
  maxWidth: props.isFullWidth ? 'none' : '900px',
  paddingX: props.isFullWidth ? 0 : props.paddingX || '2rem',
}))`
  margin: 0 auto;
  max-width: ${(props) => props.maxWidth};
  padding: 0 ${(props) => props.paddingX};
`;

export default Container;

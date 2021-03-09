import styled from 'styled-components';

interface ContainerProps {
  isFullWidth?: boolean;
  padding?: string;
}

const Container = styled.div.attrs(
  ({ isFullWidth, padding }: ContainerProps) => ({
    maxWidth: isFullWidth ? 'none' : '900px',
    padding: isFullWidth ? 0 : padding || '0 2rem',
  })
)`
  margin: 0 auto;
  width: 100%;
  min-width: 0;
  max-width: ${({ maxWidth }) => maxWidth};
  padding: ${({ padding }) => padding};
`;

export default Container;

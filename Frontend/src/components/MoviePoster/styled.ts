import styled from 'styled-components';

interface PlaceholderProps {
  height?: string;
  width?: string;
}

export const Placeholder = styled.div<PlaceholderProps>`
  background-color: gray;
  height: ${(props) => props.height || '100%'};
  width: ${(props) => props.width || '100%'};

  > img {
    width: 100%;
    height: 100%;
  }
`;

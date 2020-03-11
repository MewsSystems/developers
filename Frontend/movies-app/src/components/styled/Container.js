import styled from 'styled-components';

const Container = styled.div`
  margin: 0 auto;
  background: ${props => props.theme.background.primary};
  color: ${props => props.theme.text.primary};

  @media (min-width: 768px) {
    width: 768px;
    min-width: 768px;
  }

  @media (min-width: 1024px) {
    width: 1024px;
    min-width: 1024px;
  }

  @media (min-width: 1400px) {
    width: 1400px;
    min-width: 1400px;
  }

  @media (min-width: 2560px) {
    width: 2560px;
    min-width: 2560px;
  }
`;

export default Container;

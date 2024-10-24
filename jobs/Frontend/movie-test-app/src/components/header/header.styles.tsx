import styled from 'styled-components';

const HeaderContainer = styled.div<{ displayFullSearch: boolean }>`
  position: fixed;
  height: 5rem;
  box-shadow: 0px 3px 10px 0px rgba(38, 50, 56, 0.3);
  z-index: 100;
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
  @media screen and (max-width: 600px) {
    grid-template-columns: ${(props) => (props.displayFullSearch ? '1fr' : '1fr 1fr')};
  }
  flex-direction: row;
  width: 100%;
  top: 0;
  background-color: ${(props) => props.theme.secondary};
`;

const HeaderPlaceholder = styled.div`
  height: 5rem;
  display: block;
  position: relative;
`;

const LogoContainer = styled.div`
  cursor: pointer;
  margin: 0.2rem 2rem 0.2rem 2rem;
`;

const ButtonContainer = styled.div`
  cursor: pointer;
  display: flex;
  flex-direction: column;
  justify-content: center;
`;

const HeaderDivContainer = styled.div`
  width: 100%;
  display: flex;
  flex-direction: row;
  justify-content: space-around;
  align-items: center;
  color: ${(props) => props.theme.primary};
`;

const HeaderDivContainerStart = styled.div`
  width: 100%;
  display: flex;
  flex-direction: row;
  margin: 0.2rem 0.2rem 0.2rem 0.2rem;
  justify-content: start;
  color: ${(props) => props.theme.primary};
`;
const HeaderDivContainerEnd = styled.div`
  width: 100%;
  display: flex;
  flex-direction: row;
  justify-content: end;
  align-items: center;
  color: ${(props) => props.theme.primary};
`;

const HeaderDivRowContainer = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: start;
  align-items: center;
  color: ${(props) => props.theme.primary};
`;

export {
  HeaderContainer,
  LogoContainer,
  ButtonContainer,
  HeaderPlaceholder,
  HeaderDivContainer,
  HeaderDivRowContainer,
  HeaderDivContainerStart,
  HeaderDivContainerEnd,
};

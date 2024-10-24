import styled from 'styled-components';

const HeaderContainer = styled.div`
  position: fixed;
  height: 5rem;
  box-shadow: 0px 3px 10px 0px rgba(38, 50, 56, 0.3);
  z-index: 100;
  display: grid;
  grid-template-columns: 1fr 1fr 1fr;
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
  margin: 0.2rem;
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
`;

export { HeaderContainer, LogoContainer, ButtonContainer, HeaderPlaceholder, HeaderDivContainer };

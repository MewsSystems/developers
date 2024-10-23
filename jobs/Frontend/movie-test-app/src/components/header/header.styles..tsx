import styled from 'styled-components';

const HeaderContainer = styled.div`
  position: relative;
  box-shadow: 0px 3px 10px 0px rgba(38, 50, 56, 0.3);
  z-index: 100;
  display: flex;
  justify-content: space-evenly;
  align-items: center;
  flex-direction: row;
`;

const LogoContainer = styled.div`
  width: 20%;
  display: flex;
  flex-direction: row;
  justify-content: center;
`;

export { HeaderContainer, LogoContainer };

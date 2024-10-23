import styled from 'styled-components';
import { ColorsTheme } from '../../assets/colors/theme-colors/colors.ts';

const HeaderContainer = styled.div`
  position: fixed;
  height: 6rem;
  box-shadow: 0px 3px 10px 0px rgba(38, 50, 56, 0.3);
  z-index: 100;
  display: flex;
  justify-content: space-evenly;
  align-items: center;
  flex-direction: row;
  width: 100%;
  top: 0;
  background-color: ${ColorsTheme.secondary};
`;

const HeaderPlaceholder = styled.div`
  height: 6rem;
`;

const LogoContainer = styled.div`
  width: 20%;
  display: flex;
  flex-direction: row;
  justify-content: center;
`;

export { HeaderContainer, LogoContainer, HeaderPlaceholder };

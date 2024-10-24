import styled from 'styled-components';
import { ColorsTheme } from '../../assets/colors/theme-colors/colors.ts';

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
  background-color: ${ColorsTheme.secondary};
`;

const HeaderPlaceholder = styled.div`
  height: 5rem;
  display: block;
  position: relative;
`;

const LogoContainer = styled.div`
  cursor: pointer;
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
  justify-content: center;
`;

export { HeaderContainer, LogoContainer, ButtonContainer, HeaderPlaceholder, HeaderDivContainer };

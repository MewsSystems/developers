import { useLocation, useNavigate } from 'react-router';
import { MdLightMode, MdDarkMode, MdOutlineKeyboardReturn } from 'react-icons/md';
import {
  StyledHeaderWrapper,
  StyledHeaderText,
  StyledToggle,
  StyledGoBackButtonWrapper,
  StyledGoBackButtonContent,
  StyledGoBackButtonText,
} from './Header.styles';
import { useThemeStore } from '../../store/themeStore';
import { Button } from '../';
import { GO_BACK_BUTTON_TEST_ID } from '../../constants';
import { RETURN_TO_SEARCH_BUTTON_LABEL, TITLE_APP } from '../../constants/texts';

export const Header = () => {
  const toggleTheme = useThemeStore(state => state.toggleTheme);
  const theme = useThemeStore(state => state.theme);
  const navigate = useNavigate();
  const { pathname } = useLocation();

  const handleGoBackClick = () => {
    navigate(`/`);
  };

  return (
    <StyledHeaderWrapper>
      <StyledGoBackButtonWrapper>
        {pathname.includes('/details/') && (
          <Button onClick={handleGoBackClick} data-testid={GO_BACK_BUTTON_TEST_ID}>
            <StyledGoBackButtonContent>
              <MdOutlineKeyboardReturn fontSize={25} />
              <StyledGoBackButtonText>{RETURN_TO_SEARCH_BUTTON_LABEL}</StyledGoBackButtonText>
            </StyledGoBackButtonContent>
          </Button>
        )}
      </StyledGoBackButtonWrapper>
      <StyledHeaderText>{TITLE_APP}</StyledHeaderText>
      <StyledToggle onClick={toggleTheme}>
        {theme === 'dark' ? <MdLightMode /> : <MdDarkMode />}
      </StyledToggle>
    </StyledHeaderWrapper>
  );
};

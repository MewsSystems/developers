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
          <Button onClick={handleGoBackClick} data-testid="go-back-button">
            <StyledGoBackButtonContent>
              <MdOutlineKeyboardReturn fontSize={25} />
              <StyledGoBackButtonText>Return to search</StyledGoBackButtonText>
            </StyledGoBackButtonContent>
          </Button>
        )}
      </StyledGoBackButtonWrapper>
      <StyledHeaderText>Movie searching</StyledHeaderText>
      <StyledToggle onClick={toggleTheme}>
        {theme === 'dark' ? <MdLightMode /> : <MdDarkMode />}
      </StyledToggle>
    </StyledHeaderWrapper>
  );
};

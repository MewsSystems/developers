import { MdLightMode, MdDarkMode } from 'react-icons/md';
import { StyledHeaderWrapper, StyledHeaderText, StyledToggle } from './Header.styles';
import { useThemeStore } from '../../store/themeStore';

export const Header = () => {
  const toggleTheme = useThemeStore(state => state.toggleTheme);
  const theme = useThemeStore(state => state.theme);

  return (
    <StyledHeaderWrapper>
      <StyledHeaderText>Movie searching</StyledHeaderText>
      <StyledToggle onClick={toggleTheme}>
        {theme === 'dark' ? <MdLightMode /> : <MdDarkMode />}
      </StyledToggle>
    </StyledHeaderWrapper>
  );
};

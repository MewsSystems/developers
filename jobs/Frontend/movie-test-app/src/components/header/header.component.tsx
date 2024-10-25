import {
  ButtonContainer,
  HeaderContainer,
  HeaderDivContainer,
  HeaderDivContainerEnd,
  HeaderDivContainerStart,
  HeaderDivRowContainer,
  LogoContainer,
} from './header.styles.tsx';
import { logo_blue, logo_purple, logo_red, settings_icon } from '../../assets/images';
import FormInput from '../form-input';
import { useContext, useMemo, useState } from 'react';
import { GlobalContext } from '../../app/Provider.tsx';
import { useLocation, useNavigate } from 'react-router-dom';
import Button from '../button';
import useMediaQuery from '../../hooks/useMediaQuery.ts';
import Select from '../select';
import { ThemeColors } from '../../theme/theme.ts';
import useDebounce from '../../hooks/useDebouncer.ts';
import { useTheme } from 'styled-components';

const Header = () => {
  const [isSettingsOpen, setIsSettingsOpen] = useState(false);

  const { searchQuery, setSearchQuery, themeColor, setThemeColor } = useContext(GlobalContext);
  const [searchValue, setSearchValue] = useState(searchQuery);

  const location = useLocation();
  const navigate = useNavigate();

  const theme = useTheme();

  const isMobile = useMediaQuery(`(max-width: ${theme.breakpoints.tablet})`);

  const debouncedFunction = useDebounce((newSearchQuery) => {
    setSearchQuery(newSearchQuery);
    if (location.pathname !== '/movies') {
      navigate('/movies');
    }
  }, 300);

  const handleSearchInput = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchValue(e.target.value);
    debouncedFunction(e.target.value);
  };

  const handleChangeColorTheme = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const newColor = e.target.value;
    setThemeColor(newColor as ThemeColors);
  };

  const logo_image = useMemo(
    () => (themeColor === 'blue' ? logo_blue : themeColor === 'red' ? logo_red : logo_purple),
    [themeColor],
  );
  const hasBackButton = location.pathname !== '/movies';

  return (
    <HeaderContainer displayFullSearch={!(hasBackButton || isSettingsOpen)}>
      {(hasBackButton || isSettingsOpen || !isMobile) && (
        <HeaderDivContainerStart>
          {isSettingsOpen ? (
            <HeaderDivRowContainer>
              <Select
                onChange={handleChangeColorTheme}
                options={['blue', 'red', 'purple']}
                value={themeColor}
                label={'Theme Color'}
              />
            </HeaderDivRowContainer>
          ) : (
            <>
              {hasBackButton && (
                <ButtonContainer>
                  <Button onClick={() => navigate(-1)}>{'< BACK'}</Button>
                </ButtonContainer>
              )}
            </>
          )}
        </HeaderDivContainerStart>
      )}
      {!isMobile && (
        <HeaderDivContainer>
          <LogoContainer onClick={() => navigate('/movies')}>
            <img src={logo_image} alt={'logo'} height={70} />
          </LogoContainer>
        </HeaderDivContainer>
      )}
      <HeaderDivContainerEnd>
        <FormInput
          label={'Search movies'}
          onChange={handleSearchInput}
          value={searchValue}
          displayFullSearch={!(hasBackButton || isSettingsOpen)}
        />
        <LogoContainer>
          <img src={settings_icon} alt={'settings'} height={25} onClick={() => setIsSettingsOpen(!isSettingsOpen)} />
        </LogoContainer>
      </HeaderDivContainerEnd>{' '}
    </HeaderContainer>
  );
};

export default Header;

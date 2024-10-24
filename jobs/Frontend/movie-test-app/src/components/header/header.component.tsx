import { ButtonContainer, HeaderContainer, HeaderDivContainer, LogoContainer } from './header.styles.tsx';
import { logo_blue, logo_purple, logo_red, settings_icon } from '../../assets/images';
import FormInput from '../form-input';
import { useContext, useMemo, useState } from 'react';
import { GlobalContext } from '../../app/Provider.tsx';
import { useLocation, useNavigate } from 'react-router-dom';
import Button from '../button';
import useMediaQuery from '../../hooks/useMediaQuery.ts';
import Select from '../select';
import { ThemeColors } from '../../assets/colors/theme/theme.ts';

const Header = () => {
  const [isSettingsOpen, setIsSettingsOpen] = useState(false);

  const { searchQuery, setSearchQuery, themeColor, setThemeColor } = useContext(GlobalContext);

  const location = useLocation();
  const navigate = useNavigate();

  const isMobile = useMediaQuery('(max-width: 600px)');
  const ultraSmall = useMediaQuery('(max-width: 400px)');

  const handleSearchInput = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchQuery(e.target.value);
    if (location.pathname !== '/movies') {
      navigate('/movies');
    }
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
        <HeaderDivContainer>
          {!isSettingsOpen && (
            <ButtonContainer>
              {hasBackButton && <Button onClick={() => navigate(-1)}>{'< BACK'}</Button>}
            </ButtonContainer>
          )}
          {isSettingsOpen && (
            <HeaderDivContainer>
              <h4>{!ultraSmall && <>Theme</>} Color: </h4>{' '}
              <Select onChange={handleChangeColorTheme} options={['blue', 'red', 'purple']} />
            </HeaderDivContainer>
          )}
        </HeaderDivContainer>
      )}
      {!isMobile && (
        <HeaderDivContainer>
          <LogoContainer onClick={() => navigate('/movies')}>
            <img src={logo_image} alt={'logo'} height={70} />
          </LogoContainer>
        </HeaderDivContainer>
      )}
      <HeaderDivContainer>
        <FormInput label={'Search movies'} onChange={handleSearchInput} value={searchQuery} />
        <LogoContainer>
          {' '}
          <img src={settings_icon} alt={'settings'} height={25} onClick={() => setIsSettingsOpen(!isSettingsOpen)} />
        </LogoContainer>
      </HeaderDivContainer>{' '}
    </HeaderContainer>
  );
};

export default Header;

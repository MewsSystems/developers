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
import { FC, useContext, useMemo, useState } from 'react';
import { GlobalContext } from '../../app/Provider.tsx';

import Button from '../button';
import Select from '../select';
import { ThemeColors } from '../../theme/theme.ts';

export interface HeaderProps {
  searchQuery: string;
  handleUpdateSearchQuery: (query: string) => void;
  isMobile: boolean;
  hasBackButton?: boolean;
  handlePressBackButton?: () => void;
  handleClickLogo?: () => void;
}

const Header: FC<HeaderProps> = ({
  searchQuery,
  handleUpdateSearchQuery,
  isMobile,
  hasBackButton,
  handlePressBackButton,
  handleClickLogo,
}) => {
  const { themeColor, setThemeColor } = useContext(GlobalContext);
  const [searchValue, setSearchValue] = useState(searchQuery);
  const [isSettingsOpen, setIsSettingsOpen] = useState(false);

  const handleSearchInput = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchValue(e.target.value);
    handleUpdateSearchQuery(e.target.value);
  };

  const handleChangeColorTheme = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const newColor = e.target.value;
    setThemeColor(newColor as ThemeColors);
  };

  const logo_image = useMemo(
    () => (themeColor === 'blue' ? logo_blue : themeColor === 'red' ? logo_red : logo_purple),
    [themeColor],
  );

  return (
    <HeaderContainer displayFullSearch={!(hasBackButton || isSettingsOpen)} data-testid={'header'}>
      {(hasBackButton || isSettingsOpen || !isMobile) && (
        <HeaderDivContainerStart>
          {isSettingsOpen ? (
            <HeaderDivRowContainer>
              <Select
                onChange={handleChangeColorTheme}
                options={['blue', 'red', 'purple']}
                value={themeColor}
                label={'Theme Color'}
                data-testid={'theme-select'}
              />
            </HeaderDivRowContainer>
          ) : (
            <>
              {hasBackButton && (
                <ButtonContainer>
                  <Button onClick={handlePressBackButton} data-testid={'back-button'}>
                    {'< BACK'}
                  </Button>
                </ButtonContainer>
              )}
            </>
          )}
        </HeaderDivContainerStart>
      )}
      {!isMobile && (
        <HeaderDivContainer>
          <LogoContainer onClick={handleClickLogo} data-testid={'logo-image'}>
            <img src={logo_image} alt={'logo'} height={70} />
          </LogoContainer>
        </HeaderDivContainer>
      )}
      <HeaderDivContainerEnd>
        <FormInput
          label={'Search movies'}
          onChange={handleSearchInput}
          value={searchValue}
          $displayFullSearch={!(hasBackButton || isSettingsOpen)}
          data-testid={'search-input'}
        />
        <LogoContainer data-testid={'settings-button'} onClick={() => setIsSettingsOpen(!isSettingsOpen)}>
          <img src={settings_icon} alt={'settings'} height={25} />
        </LogoContainer>
      </HeaderDivContainerEnd>{' '}
    </HeaderContainer>
  );
};

export default Header;

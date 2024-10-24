import { ButtonContainer, HeaderContainer, HeaderDivContainer, LogoContainer } from './header.styles.tsx';
import {
  logo_blue,
  logo_purple,
  logo_blue_mobile,
  logo_purple_mobile,
  logo_red_mobile,
  logo_red,
} from '../../assets/images';
import FormInput from '../form-input';
import { useContext } from 'react';
import { GlobalContext } from '../../app/Provider.tsx';
import { useLocation, useNavigate } from 'react-router-dom';
import Button from '../button';

const Header = () => {
  const { searchQuery, setSearchQuery, themeColor, setThemeColor } = useContext(GlobalContext);

  const location = useLocation();

  const handleSearchInput = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchQuery(e.target.value);
    if (location.pathname !== '/movies') {
      navigate('/movies');
    }
  };

  const navigate = useNavigate();

  const logo_image = themeColor === 'blue' ? logo_blue : themeColor === 'red' ? logo_red : logo_purple;
  const logo_image_mobile =
    themeColor === 'blue' ? logo_blue_mobile : themeColor === 'red' ? logo_red_mobile : logo_purple_mobile;

  return (
    <HeaderContainer>
      <HeaderDivContainer>
        <ButtonContainer>
          {location.pathname !== '/movies' && <Button onClick={() => navigate(-1)}>{'< BACK'}</Button>}
        </ButtonContainer>
        <select onChange={(e) => setThemeColor(e.target.value)}>
          <option value="blue">Blue</option>
          <option value="red">Red</option>
          <option value="purple">Purple</option>
        </select>
      </HeaderDivContainer>
      <HeaderDivContainer>
        <LogoContainer onClick={() => navigate('/movies')}>
          <img src={logo_image} alt={'logo'} height={70} />
        </LogoContainer>
      </HeaderDivContainer>
      <HeaderDivContainer>
        <FormInput label={'Search Movies'} onChange={handleSearchInput} value={searchQuery} />
      </HeaderDivContainer>
    </HeaderContainer>
  );
};

export default Header;

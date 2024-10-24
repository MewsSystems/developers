import { ButtonContainer, HeaderContainer, HeaderDivContainer, LogoContainer } from './header.styles.tsx';
import { logoImage } from '../../assets/base64-images/base64-images.ts';
import FormInput from '../form-input';
import { useContext } from 'react';
import { GlobalSearchContext } from '../../app/Provider.tsx';
import { useLocation, useNavigate } from 'react-router-dom';
import Button from '../button';

const Header = () => {
  const { searchQuery, setSearchQuery } = useContext(GlobalSearchContext);

  const location = useLocation();

  const handleSearchInput = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchQuery(e.target.value);
    if (location.pathname !== '/movies') {
      navigate('/movies');
    }
  };

  const navigate = useNavigate();

  return (
    <HeaderContainer>
      <HeaderDivContainer>
        <ButtonContainer>
          {location.pathname !== '/movies' && <Button onClick={() => navigate(-1)}>BACK</Button>}
        </ButtonContainer>
      </HeaderDivContainer>
      <HeaderDivContainer>
        <LogoContainer onClick={() => navigate('/movies')}>
          <img src={logoImage} alt={'logo'} height={70} />
        </LogoContainer>
      </HeaderDivContainer>
      <HeaderDivContainer>
        <FormInput label={'Search Movies'} onChange={handleSearchInput} value={searchQuery} />
      </HeaderDivContainer>
    </HeaderContainer>
  );
};

export default Header;

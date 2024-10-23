import { HeaderContainer, LogoContainer } from './header.styles..tsx';
import { logoImage } from '../../assets/base64images/movieImages.ts';
import FormInput from '../form-input';
import { useContext } from 'react';
import { GlobalSearchContext } from '../../app/Provider.tsx';

const Header = () => {
  const { searchQuery, setSearchQuery } = useContext(GlobalSearchContext);

  const handleSearchInput = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchQuery(e.target.value);
  };

  return (
    <HeaderContainer>
      <LogoContainer>
        <img src={logoImage} alt={'logo'} height={50} />
      </LogoContainer>
      <FormInput label={'Search Movies'} onChange={handleSearchInput} value={searchQuery} />
    </HeaderContainer>
  );
};

export default Header;

import { HeaderContainer, LogoContainer } from './header.styles.tsx';
import { logoImage } from '../../assets/base64-images/base64-images.ts';
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
        <img src={logoImage} alt={'logo'} height={70} />
      </LogoContainer>
      <FormInput label={'Search Movies'} onChange={handleSearchInput} searchQuery={searchQuery} />
    </HeaderContainer>
  );
};

export default Header;

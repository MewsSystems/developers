import { UserCircle } from '@styled-icons/heroicons-solid';
import StyledHeader from './Header.styles';

const Header = () => {
    return (
        <StyledHeader>
            <h2>The MovieDB app</h2>
            <UserCircle data-testid="user" size={36} />
        </StyledHeader>
    );
};

export default Header;

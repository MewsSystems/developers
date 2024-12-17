import {
  AppName,
  Header as StyledHeader,
} from "@/components/Header/HeaderStyle";

const Header: React.FC = () => {
  return (
    <StyledHeader>
      <AppName>MovieFan</AppName>
    </StyledHeader>
  );
};

export default Header;

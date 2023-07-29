import { FC } from "react";
import { Link } from "react-router-dom";
import { styled } from "styled-components";

const Wrapper = styled.div`


  background-color: #00ffd5;
  ul,li{
    
    display:flex;
    text-decoration:none;
    list-style-type: none;
    justify-content: center;
  }
  
`;

const StyledLink = styled(Link)`
  color: #3f298d;
  font-weight: bold;
  text-decoration:none;
  padding:12px;
`;
const NavigationBar: FC<{}> = () => {

  
  return (
    <Wrapper>
      <nav>
        <ul>
          <li>
            <StyledLink to='/'>Search</StyledLink>
          </li>
          <li>
            <StyledLink to='/movies/1'>Higlighted movie</StyledLink>
          </li>
          <li>
            <StyledLink to='/nothing-here'>Nothing Here</StyledLink>
          </li>
        </ul>
      </nav>
    </Wrapper>
  );
}

export default NavigationBar;

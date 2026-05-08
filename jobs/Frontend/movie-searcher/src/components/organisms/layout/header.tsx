import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import logoCinema from "../../../assets/images/logoCinema.png";
import { AutocompleteSearcher } from "../../molecules/search/autocomplete-searcher";
import styled from "styled-components";

export const Header = () => {
  const nav = useNavigate();
  const [isScrolled, setIsScrolled] = useState(false);

  useEffect(() => {
    const handleScroll = () => {
      setIsScrolled(window.scrollY > 0);
    };

    window.addEventListener("scroll", handleScroll);
    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, []);

  return (
    <HeaderContainer isScrolled={isScrolled}>
      <LogoContainer onClick={() => nav("/")}>
        <img src={logoCinema} alt="cine_logo" style={{ width: "120px" }} />
        <LogoText>Movie Searcher</LogoText>
      </LogoContainer>
      <SearchContainer>
        <AutocompleteSearcher
          size="small"
          placeholder="Search..."
          color="#c4ab9c"
        />
      </SearchContainer>
    </HeaderContainer>
  );
};

const HeaderContainer = styled.div<{ isScrolled: boolean }>`
  position: fixed;
  top: 0;
  left: 0;
  z-index: 50;
  display: flex;
  justify-content: space-between;
  align-items: center;
  height: 120px;
  width: 100%;
  transition: background-color 0.3s ease, box-shadow 0.3s ease;
  background-color: ${(props) => (props.isScrolled ? "black" : "transparent")};
  box-shadow: ${(props) =>
    props.isScrolled ? "inset 0 -10px 15px rgba(203,165,123,0.4)" : "none"};
`;

const LogoContainer = styled.div`
  display: flex;
  align-items: center;
  width: 50%;
  margin: 2px;
  cursor: pointer;
`;

const LogoText = styled.div`
  font-size: 25px;
  color: #e9dcd6;
  font-family: "Mono", sans-serif;
  font-weight: bold;
`;

const SearchContainer = styled.div`
  width: 20%;
  margin-right: 4px;
  display: none;

  @media (min-width: 768px) {
    display: block;
  }
`;

import React, { memo, useCallback } from "react";
import { StyledHeaderLogo, StyledHeader } from "./Header.styles";
import Search from "../Search/Search";

const Header = () => {
  const handleScrollToTopRequest = useCallback(() => {
    window.scrollTo({ top: 0, behavior: "smooth" });
  }, []);

  return (
    <StyledHeader>
      <StyledHeaderLogo
        src="flixview.png"
        alt="logo"
        onClick={handleScrollToTopRequest}
      />
      <Search />
    </StyledHeader>
  );
};

export default memo(Header);

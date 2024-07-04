import React, { createContext, useState, PropsWithChildren } from "react";

export const SearchContext = createContext({
  searchTerms: "",
  // @ts-ignore - We will override this value when we will use the provider
  setSearchTerms: (searchTerms: string) => {},
});

export const SearchContextProvider: React.FC<PropsWithChildren<{}>> = ({ children }) => {

  const [searchTerms, setSearchTerms] = useState("");

  return (
    <SearchContext.Provider
      value={{ searchTerms, setSearchTerms }}
    >
      {children}
    </SearchContext.Provider>
  );
};
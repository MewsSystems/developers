import React, { createContext, useState, PropsWithChildren } from "react";

/**
 * This context is used to store the search terms that the user has entered in the search input.
 * It allow us to keep the last search terms in memory and use it to fetch the movies when the user
 * navigate back to the search page.
 */
export const SearchContext = createContext({
  searchTerms: "",
  // @ts-ignore - We will override this value when we will use the provider
  setSearchTerms: (searchTerms: string) => {},
});

export const SearchContextProvider: React.FC<PropsWithChildren<{}>> = ({
  children,
}) => {
  const [searchTerms, setSearchTerms] = useState("");

  return (
    <SearchContext.Provider value={{ searchTerms, setSearchTerms }}>
      {children}
    </SearchContext.Provider>
  );
};
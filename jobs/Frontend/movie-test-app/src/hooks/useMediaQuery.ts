import React from 'react';

const useMediaQuery = (query: string) => {
  const [matches, setMatches] = React.useState(window.matchMedia(query).matches);

  React.useEffect(() => {
    const matchQueryList = window.matchMedia(query);

    const handleChange = (e: { matches: boolean | ((prevState: boolean) => boolean) }) => {
      setMatches(e.matches);
    };

    matchQueryList.addEventListener('change', handleChange);

    return () => {
      matchQueryList.removeEventListener('change', handleChange);
    };
  }, [query]);

  return matches;
};

export default useMediaQuery;

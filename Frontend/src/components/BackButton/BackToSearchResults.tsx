import { useSelector } from 'react-redux';
import { useRouteMatch } from 'react-router';
import { searchSelector } from '../../redux/searchReducer';
import BackButton from './BackButton';

interface BackToSearchResultsProps {
  searchResultsPath: string;
}

function BackToSearchResults({ searchResultsPath }: BackToSearchResultsProps) {
  const match = useRouteMatch({ path: searchResultsPath, exact: true });
  const { query } = useSelector(searchSelector);

  if (match || !query) {
    return null;
  }

  return <BackButton />;
}

export default BackToSearchResults;

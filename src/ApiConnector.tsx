import { connect } from 'react-redux';
import { useSearchService } from './hooks/useSearchService';
import { FC, useEffect } from 'react';
import { updateResults } from './actions/results';
import { Dispatch } from 'redux';
import { SearchState } from './reducers/searchReducer';
import { Movie, Person } from './types';

export type State = {
  search: SearchState;
  results: { results?: Movie[] & Person[] };
  detail: Movie & Person;
};

const ApiConnector: FC<{
  query?: string;
  page?: number;
  searchType?: string;
  queryParams?: Record<string, string>;
  dispatch?: Dispatch;
}> = ({ query, page, searchType, queryParams, dispatch }): null => {
  const { results } = useSearchService({ query, page, searchType, queryParams });

  useEffect(() => {
    if (results?.results) {
      dispatch(updateResults(results));
    }
  }, [results]);
  return null;
};

const mapStateToProps = (state: State) => {
  return state.search;
};

export default connect(mapStateToProps)(ApiConnector);

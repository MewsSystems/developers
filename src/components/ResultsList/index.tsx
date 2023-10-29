import React, { FC, useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { fetchNextPage, fetchPrevPage, fetchSpecificPage } from '../../actions/search';
import { ListContainer } from './styles';
import { Dispatch } from 'redux';
import { Item } from './Item';
import { Pagination } from '../Pagination';
import { useWindowWidth } from '../../hooks/useWindowWidth';
import { setSelectedResult } from '../../actions/detail';
import { SearchResponse, Movie, Person } from '../../types';
import { NoResults } from '../NoResults';

const ResultsList: FC<{
  search?: any;
  results?: SearchResponse;
  dispatch: Dispatch;
}> = ({ results, search, dispatch }) => {
  const [selectedCard, setSelectedCard] = useState<number | undefined>();
  const { isMobile } = useWindowWidth();

  const { total_pages, results: searchResults, page } = results || {};

  useEffect(() => {
    if (!searchResults?.length) return;
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }, [page]);

  const handleResultSelection = (result: Movie) => {
    dispatch?.(setSelectedResult(result));
  };

  if (!searchResults?.length) {
    return <NoResults>🔍 Sorry, No results for that search, please try again 🔍</NoResults>;
  }

  return (
    <>
      <ListContainer>
        {searchResults?.map((result: Movie & Person) => {
          const isSelected = selectedCard === result.id;

          return (
            <Item
              key={result.id}
              result={result}
              isSelected={isMobile && isSelected}
              toggleInfo={(id) => setSelectedCard(id)}
              onNavigation={handleResultSelection}
              itemType={search.searchType}
            />
          );
        })}
      </ListContainer>

      {searchResults?.length && total_pages > 1 && (
        <Pagination
          onNextClick={() => dispatch(fetchNextPage())}
          onPrevClick={() => dispatch(fetchPrevPage())}
          onPageClick={(page) => dispatch(fetchSpecificPage(page))}
          totalPages={total_pages}
          currentPage={page}
        />
      )}
    </>
  );
};

const mapStateToProps = (state: any) => {
  return state;
};

export default connect(mapStateToProps)(ResultsList);

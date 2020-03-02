import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import styled from 'styled-components'
import throttle from 'lodash/throttle';

import { RootState, Dispatch } from 'domains/reducers';
import {
  clearResults,
  fetchAssets,
  setQuery,
  getCurrentPage,
  getQuery,
  getResultIds,
  getTotalPages,
  isFetching as isCurrentlyFetching,
} from 'domains/ducks/assets';
import Item from 'components/Item';
import InputWithBorder from 'components/Input';
import constants from 'cssConstants';

const mapStateToProps = (rootState: RootState) => ({
  currentPage: getCurrentPage(rootState),
  isFetching: isCurrentlyFetching(rootState),
  query: getQuery(rootState),
  resultIds: getResultIds(rootState),
  totalPages: getTotalPages(rootState),
});

const mapDispatchToProps = (dispatch: Dispatch) => (bindActionCreators({
  clearResults,
  fetchAssets,
  setQuery,
}, dispatch));

type Props = ReturnType<typeof mapStateToProps> & ReturnType<typeof mapDispatchToProps>;

const StyledWrapper = styled.div`
  text-align: center;
`;
const StyledItem = styled(Item)`
  display: flex;
`;
const StyledContainer = styled.ul`
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  padding: 0;
  margin: 0;
`;
const StyledNoResults = styled.div`
  color: ${constants.ACCENT};
  font-size: 2rem;
`;
const StyledInput = styled(InputWithBorder)`
  margin-bottom: 3rem;
`;
const StyledPagination = styled.a`
  font-size: 1.5rem;
  margin-top: 2rem;
  display: block;
  background: red;
  padding: 1.5rem;
  background: ${constants.ACCENT};
  color: white;
  cursor: pointer;
`;

class Search extends React.PureComponent<Props> {
  componentDidMount() {
    const { resultIds, query } = this.props;
    if (query && !resultIds.length) { this.props.fetchAssets(query); }
  }

  throttledFetch = throttle((query) => this.props.fetchAssets(query), 100)

  fetchMore = (e: React.MouseEvent<HTMLAnchorElement>) => {
    const {
      query,
      currentPage,
    } = this.props;

    this.props.fetchAssets(query, currentPage + 1);
  }

  handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;

    this.props.setQuery(value);
    if (value) {
      this.throttledFetch(value);
    } else {
      this.props.clearResults();
    }
  }

  render() {
    const { resultIds, query, isFetching, totalPages, currentPage } = this.props;

    return (
      <StyledWrapper>
        <StyledInput
          type="text"
          onChange={this.handleChange}
          value={query}
          required
          autoFocus
          placeholder="Search..."
        />
        {
          resultIds.length ? <StyledContainer>
            {resultIds.map(id => (
              <StyledItem key={id} id={id}/>
            ))}
          </StyledContainer> :
          query && !isFetching ? <StyledNoResults>No results for "{query}"</StyledNoResults>
          : null
        }
        {(currentPage < totalPages) ? (
          <StyledPagination onClick={this.fetchMore}>Load More...</StyledPagination>
        ) : null}
      </StyledWrapper>
    );
  }
};

export default connect(mapStateToProps, mapDispatchToProps)(Search);

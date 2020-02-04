import React from "react";
import { RouteComponentProps } from "react-router-dom";
import { Header, Image, Input, Table } from "semantic-ui-react";
import styled from "styled-components";
import { searchForMovie } from "util/theMovieDb";

import { getImageUrl } from "../../util/theMovieDb";
import TableFooter from "./TableFooter";

const StyledTable = styled(Table)`
  width: 75vw !important;
`;

const StyledSearchDiv = styled.div`
  margin: auto;
  margin-top: 3em;
  width: fit-content;
`;

const StyledTableRow = styled(Table.Row)`
  padding-top: 0.5em !important;
  padding-bottom: 0.5em !important;
`;

export type SearchProps = RouteComponentProps;

export interface SearchState {
  searchResults: {
    title: string;
    id: number;
    posterUrl: string | null;
    releaseDate: Date;
  }[];
  pageNum: number;
  totalPages: number;
  query: string;
}

class Search extends React.Component<SearchProps, SearchState> {
  state: SearchState = {
    searchResults: [],
    pageNum: 0,
    totalPages: 0,
    query: ""
  };

  _search = async (query: string, pageNum?: number) => {
    const response = await searchForMovie(query, pageNum);
    const results = response?.results;
    if (response && results)
      this.setState({
        searchResults:
          results.map(({ id, title, poster_path, release_date }) => ({
            id,
            title,
            posterUrl: poster_path ? getImageUrl(poster_path) : poster_path,
            releaseDate: new Date(release_date)
          })) || [],
        totalPages: response.total_pages,
        pageNum: response.page,
        query
      });
  };

  _handleClick = (e: React.MouseEvent<HTMLElement>) => {
    const movieId = e.currentTarget.dataset.id;

    this.props.history.push(`/movie/${movieId}`);
  };

  _changePage = (page: number | "next" | "prev") => {
    let pageNum = page;
    switch (page) {
      case "next":
        pageNum = Math.min(this.state.pageNum + 1, this.state.totalPages);
        break;
      case "prev":
        pageNum = Math.max(this.state.pageNum - 1, 0);
    }
    if (typeof pageNum === "number") this._search(this.state.query, pageNum);
  };

  render() {
    const rows = this.state.searchResults.map(res => (
      <StyledTableRow
        key={"movie-search-results_" + res.id}
        data-id={res.id}
        onClick={this._handleClick}
      >
        <Table.Cell>
          <Header image as="h4">
            <Image src={res.posterUrl} size="small" />
            <Header.Content>
              {res.title} ({res.releaseDate.getFullYear()})
            </Header.Content>
          </Header>
        </Table.Cell>
      </StyledTableRow>
    ));

    return (
      <StyledSearchDiv>
        <Input
          placeholder="Search..."
          onChange={(e, data) => {
            this._search(data.value);
          }}
        />
        <StyledTable>
          <Table.Body>
            {rows.length > 0 ? (
              rows
            ) : (
              <Table.Row>
                <Table.Cell>No results</Table.Cell>
              </Table.Row>
            )}
          </Table.Body>

          <TableFooter
            currPage={this.state.pageNum}
            totalPages={this.state.totalPages}
            changePage={this._changePage}
          />
        </StyledTable>
      </StyledSearchDiv>
    );
  }
}

export default Search;

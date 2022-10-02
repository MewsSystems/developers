import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import { useDispatch, useSelector } from 'react-redux';
import {
  clearData, setQuery, setData, setPage,
} from '../../redux/searchSlice';
import { SEARCH_ENDPOINT, API_AUTH } from '../../utils';
import { RootState } from '../../redux/store';
import { IconsSearch, IconsClear } from '../icons';

const InputWrapper = styled.div`
  margin: 0 auto;
  max-width: 600px;
  position: relative;
  
  input {
    background: #fff;
    border: 1px solid #fff;
    border-radius: 8px;
    font-size: 20px;
    padding: 16px 24px 16px 70px;
    transition: box-shadow .2s;
    width: 100%;
    
    &:active,   
    &:focus {
      outline: 0;
      border-color: #fff;
      box-shadow: 0 0 20px #fff; 
    }    
  }
`;

const ClearButton = styled.button`
  background: transparent;
  color: #999;
  cursor: pointer;
  padding: 0 16px;
  position: absolute;
  right: 0;
  top: 0;
  bottom: 0;
  border: 0;
  
  &:hover,
  &:active,
  &:focus {
    color: #000;
  }
`;

const SearchIcon = styled(IconsSearch)`
  margin: auto;
  position: absolute;
  top: 0;
  bottom: 0;
  left: 20px;
`;

export default function SearchInput() {
  const dispatch = useDispatch();
  const { currentPage, searchQuery } = useSelector((state: RootState) => state.search);
  const [searchTerm, setSearchTerm] = useState(searchQuery);

  useEffect(
    () => {
      const abortController = new AbortController();
      const { signal } = abortController;

      dispatch(setQuery(searchTerm));

      if (searchTerm.length) {
        fetch(`${SEARCH_ENDPOINT}?${API_AUTH}&query=${searchTerm}&page=${currentPage}`, { signal })
          .then(async (response) => response.json())
          .then((data: IFetchData) => {
            dispatch(setData(data));
          })
          .catch((err: Error) => {
            if (err.name !== 'AbortError') {
              // handle errors but ignore AbortErrors
            }
          });
      } else {
        dispatch(clearData());
      }

      return function cleanup() {
        abortController.abort();
      };
    },
    [searchTerm, currentPage],
  );

  useEffect(
    () => {
      // reset pager on search term change but remember it
      // when returning from movie detail
      if (searchQuery !== searchTerm) {
        dispatch(setPage(1));
      }
    },
    [searchTerm],
  );

  return (
    <InputWrapper>
      <SearchIcon width="28px" height="28px" color="#999" />
      <input onChange={(e) => setSearchTerm(e.target.value)} placeholder="Enter search term" value={searchTerm} />
      {searchTerm !== ''
        && (
          <ClearButton type="button" onClick={() => setSearchTerm('')} title="Clear search term">
            <IconsClear />
          </ClearButton>
        )}
    </InputWrapper>
  );
}

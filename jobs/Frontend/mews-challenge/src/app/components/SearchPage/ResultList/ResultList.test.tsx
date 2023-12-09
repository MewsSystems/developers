import React, { FC } from 'react';
import { render, screen } from '@testing-library/react';
import { Provider, useDispatch } from 'react-redux';
import { RootState } from '../../../../app/store';
import userEvent from '@testing-library/user-event';
import ResultList from './ResultList';
import { Movie } from '../../../types';
import { configureStore } from '@reduxjs/toolkit';
import appReducer from '../../../services/appReducer';
import { Root } from 'react-dom/client';

const getStore = (query: string) =>
  configureStore({
    reducer: { app:appReducer },
    preloadedState: {
      app: {
        query,
        page: 1,
        totalPages: 1,
        movie: null
      }
    }
  })

describe('ResultList unit tests', () => {
  test('shows message when nothing has been typed in the search box', async () => {
    render(
      <Provider store={getStore("jkdshfdkjsfhksfhksfh")}>
        <ResultList movies={[]} />
      </Provider>)
    expect(await screen.findByText(/no results/i)).toBeVisible()
  });

});
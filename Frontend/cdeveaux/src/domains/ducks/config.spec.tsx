import React from 'react';
import configureStore from 'redux-mock-store' //ES6 modules
import thunk from 'redux-thunk'
import MockAdapter from 'axios-mock-adapter';
import throttle from 'lodash/throttle';

import { getState as getStateUtil } from './utils/getState';
import axios from 'domains/API/axios';

import reducer, {
  defaultState,
  getBaseUrl,
  getSizes,
  storeConfig,
  loadConfig,
  ImageSizesEnums,
} from './config';

const middlewares = [thunk];
const mockStore = configureStore(middlewares)
const getState = getStateUtil(reducer, defaultState, 'config');
const store = mockStore(getState);

describe('Config reducer', () => {
  beforeEach(() => { store.clearActions(); });

  it('should return initialState by default', () => {
    const state = reducer(undefined, { type: 'none' });
    expect(state).toEqual(defaultState);
  });

  it('should handle LOAD_CONFIG', () => {
    const state = reducer(undefined, loadConfig());
    expect(state).toEqual(defaultState);
  });

  it('should handle STORE_CONFIG', () => {
    const url = 'base_URL';
    store.dispatch(storeConfig({
      base_url: url,
      sizes: {
        backdrop_sizes: ['w100'],
        logo_sizes: ['w200'],
        poster_sizes: ['w300'],
        profile_sizes: ['w400'],
        still_sizes: ['w500'],
      },
    }));
    const state = store.getState();
    expect(getBaseUrl(state)).toEqual(url);
    expect(getSizes(state, ImageSizesEnums.BACKDROP)).toEqual(['w100']);
    expect(getSizes(state, ImageSizesEnums.LOGO)).toEqual(['w200']);
    expect(getSizes(state, ImageSizesEnums.POSTER)).toEqual(['w300']);
    expect(getSizes(state, ImageSizesEnums.PROFILE)).toEqual(['w400']);
    expect(getSizes(state, ImageSizesEnums.STILL)).toEqual(['w500']);
  });
});

import React from 'react';
import configureStore from 'redux-mock-store' //ES6 modules
import thunk from 'redux-thunk'
import MockAdapter from 'axios-mock-adapter';
import throttle from 'lodash/throttle';

import { getState as getStateUtil } from './utils/getState';
import axios from 'domains/API/axios';

import reducer, {
  defaultState,
  storeAssets,
  fetchResultsSuccess,
  clearResults,
  fetchAssets,
  setTimestamp,
  getAssets,
  getAssetById,
  getResultIds,
  getTimestamp,
} from './assets';

const mock = new MockAdapter(axios);

const asset1 = {
  id: 'asset1_id',
  title: 'first asset',
};
const asset2 = {
  id: 'asset2_id',
  title: 'second asset',
};
const asset3 = {
  id: 'asset3_id',
  title: 'Asset number 3',
};
const asset4 = {
  id: 'asset4_id',
  title: 'asset 4',
};

const middlewares = [thunk];
const mockStore = configureStore(middlewares);
const getState = getStateUtil(reducer, defaultState, 'assets');
const store = mockStore(getState);

const getWithDelay = (delay, asset) => () =>
  new Promise((resolve, reject) => {
    setTimeout(() => {
      resolve([200, {
        page: 1,
        total_results: 500,
        total_pages: 5,
        results: [asset],
      }]);
    }, delay);
  });

describe('Assets reducer', () => {
  beforeEach(() => { store.clearActions(); });

  it('should return initialState by default', () => {
    const state = reducer(undefined, { type: 'none' });
    expect(state).toEqual(defaultState);
  });

  it('should handle STORE_ASSETS', () => {
    const assets = [asset1, asset2, asset3];

    store.dispatch(storeAssets(assets));
    let state = store.getState();
    expect(getAssetById(state, asset1.id)).toEqual(asset1);
    expect(getAssetById(state, asset2.id)).toEqual(asset2);
    expect(getAssetById(state, asset3.id)).toEqual(asset3);
    expect(Object.keys(getAssets(state)).length).toEqual(3);

    // Multiple store assets should concat and not erase
    store.dispatch(storeAssets([asset4]));
    state = store.getState();
    expect(getAssetById(state, asset1.id)).toEqual(asset1);
    expect(getAssetById(state, asset4.id)).toEqual(asset4);
    expect(Object.keys(getAssets(state)).length).toEqual(4);

    // Assets with the same id, should be merged
    const updatedAsset = { ...asset1, title: 'new title asset 1' };
    store.dispatch(storeAssets([updatedAsset]));
    state = store.getState();
    expect(getAssetById(state, asset1.id)).toEqual(updatedAsset);
    expect(Object.keys(getAssets(state)).length).toEqual(4);
  });

  it('should handle STORE_RESULTS', () => {
    const assetIds = [asset1.id, asset2.id, asset3.id];

    store.dispatch(fetchResultsSuccess({
      ids: assetIds,
      page: 1,
      totalPages: 2,
    }));
    expect(getResultIds(getState(store.getActions()))).toEqual(assetIds);
  });

  it('should handle CLEAR_RESULTS', () => {
    const assetIds = [asset1.id, asset2.id, asset3.id];

    // State with results
    store.dispatch(fetchResultsSuccess({
      ids: assetIds,
      page: 1,
      totalPages: 2,
    }));
    expect(getResultIds(getState(store.getActions()))).toEqual(assetIds);

    // Store cleared
    store.dispatch(clearResults());
    expect(getResultIds(getState(store.getActions()))).toEqual([])
  });

  it('should handle SET_TIMESTAMP', () => {
    const now = Date.now();
    store.dispatch(setTimestamp(now));
    expect(getTimestamp(getState(store.getActions()))).toEqual(now);
  });

  it('should handle race conditions', async (done) => {
    const getRandomMs = () => Math.floor(Math.random() * 100);
    let now = Date.now();
    const spy = jest
      .spyOn(global.Date, 'now')
      .mockImplementation(() => new Date(now++).valueOf());
    mock
      .onGet('/search/movie').replyOnce(getWithDelay(getRandomMs(), asset1))
      .onGet('/search/movie').replyOnce(getWithDelay(getRandomMs(), asset2))
      .onGet('/search/movie').replyOnce(getWithDelay(getRandomMs(), asset3))
      .onGet('/search/movie').replyOnce(getWithDelay(getRandomMs(), asset4))
      .onGet('/search/movie').replyOnce(getWithDelay(getRandomMs(), asset3));

    const throttledFetchAssets = throttle(fetchAssets, 100);
    await Promise.all([
      store.dispatch(throttledFetchAssets('t')),
      store.dispatch(throttledFetchAssets('te')),
      store.dispatch(throttledFetchAssets('tes')),
      store.dispatch(throttledFetchAssets('test')),
      store.dispatch(throttledFetchAssets('tes')),
    ]);

    const state = getState(store.getActions());
    // Result should be the last action executed
    expect(getResultIds(state)).toEqual([asset3.id]);
    spy.mockClear();
    done();
  });
});

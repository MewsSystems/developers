import React from 'react';

import reducer, { defaultState, storeAssets, storeResults } from './assets';

const assets = [{
  id: 414,
  title: 'first asset',
}, {
  id: 142,
  title: 'second asset',
}, {
  id: 291,
  title: 'Asset number 3',
}, {
  id: 781,
  title: 'asset 4',
}];


describe('Assets reducer', () => {
  it('should return initialState by default', () => {
    const state = reducer();
    expect(state).toEqual(defaultState);
  });

  it('should handle STORE_ASSETS and serialize assets', () => {
    let state = reducer({}, storeAssets([assets[0], assets[1], assets[2]]));
    let expectedState = {
      [assets[0].id]: assets[0],
      [assets[1].id]: assets[1],
      [assets[2].id]: assets[2],
    };
    expect(state.assets).toEqual(expectedState);

    // Merge old assets with new one
    state = reducer(state, storeAssets([assets[3]]));
    expectedState = {
      ...expectedState,
      [assets[3].id]: assets[3],
    };
    expect(state.assets).toEqual(expectedState);

    // Properly erase with last assets
    state = reducer(state, storeAssets([{ ...assets[0], title: 'erased title' }]));
    expectedState = {
      ...expectedState,
      [assets[0].id]: {
        ...assets[0],
        title: 'erased title',
      },
    };
    expect(state.assets).toEqual(expectedState);
  });

  it('should handle STORE_RESULTS', () => {
    const state = reducer({}, storeResults([assets[0].id, assets[1].id, assets[2].id]));
    expect(state.resultIds).toEqual([assets[0].id, assets[1].id, assets[2].id]);
  });
});

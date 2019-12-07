import React from "react";
import thunk from "redux-thunk";
import { createStore, applyMiddleware } from "redux";
import { Provider } from "react-redux";
import { persistStore, persistReducer } from "redux-persist";
import storage from "redux-persist/lib/storage";
import { PersistGate } from "redux-persist/integration/react";
import styled from "styled-components";
import { Selectors } from "./components/Selectors";
import { RateList } from "./components/RateList";
import { reducer } from "./store/reducer";

const AppWrapper = styled.div`
  text-align: center;
`;

export const App = () => {
  const persistConfig = {
    key: "root",
    storage
  };

  const persistedReducer = persistReducer(persistConfig, reducer);

  const store = createStore(persistedReducer, applyMiddleware(thunk));
  const persistor = persistStore(store);

  return (
    <Provider store={store}>
      <PersistGate loading={null} persistor={persistor}>
        <AppWrapper>
          <Selectors />
          <RateList />
        </AppWrapper>
      </PersistGate>
    </Provider>
  );
};

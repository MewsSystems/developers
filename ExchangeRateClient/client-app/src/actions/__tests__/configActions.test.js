import configureMockStore from "redux-mock-store";
import thunk from "redux-thunk";

import mockAxios from "axios";
import expect from "expect";
import configuration from "../../records/config";
import rates from "../../records/rates";

import { selectIds, fetchConfigAction, fetchRatesAction } from "../index";

const middlewares = [thunk];
const mockStore = configureMockStore(middlewares);

test("selectIds function should create an action", () => {
  const ids = ["1", "2", "3"];
  const action = selectIds(ids);
  expect(action).toMatchSnapshot();
});

test("fetchRatesAction function should create actions", () => {
  mockAxios.get.mockImplementationOnce(() =>
    Promise.resolve({
      data: rates,
    }),
  );
  const store = mockStore({});
  store.dispatch(fetchRatesAction(["1", "2"])).then(() => {
    expect(store.getActions()).toMatchSnapshot();
  });
});

test("fetchConfigAction function should create actions", () => {
  mockAxios.get.mockImplementationOnce(() => Promise.resolve({ data: configuration }));

  const store = mockStore({});
  store.dispatch(fetchConfigAction()).then(() => {
    expect(store.getActions()).toMatchSnapshot();
  });
});

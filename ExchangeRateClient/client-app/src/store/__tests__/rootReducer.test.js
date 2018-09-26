// @flow
import rootReducer, { initalState } from "../rootReducer";

describe("rootReducer", () => {
  test("should return initialState", () => {
    // $FlowTest
    const state = rootReducer(undefined, {});
    expect(state).toEqual(initalState);
  });

  test("should handle CONFIGS_FETCH_START action", () => {
    const state = rootReducer(initalState, { type: "CONFIGS_FETCH_START" });
    expect(state.isFetchingConfig).toEqual(true);
  });

  test("should handle CONFIG_FETCH_SUCCESS action", () => {
    const state = rootReducer(initalState, {
      type: "CONFIG_FETCH_SUCCESS",
      payload: {
        selectedRates: [
          "1993f7b9-f9be-551a-beac-312d6befd0cf",
          "41cae0fd-b74d-5304-a45c-ba000471eabd",
        ],
        data: {
          "41cae0fd-b74d-5304-a45c-ba000471eabd": {
            currencies: [
              { code: "PHP", name: "Philippines Peso" },
              { code: "DZD", name: "Algeria Dinar" },
            ],
          },
          "1993f7b9-f9be-551a-beac-312d6befd0cf": {
            currencies: [
              { code: "CZK", name: "Czech Republic Koruna" },
              { code: "LSL", name: "Lesotho Loti" },
            ],
          },
        },
      },
    });
    expect(state).toMatchSnapshot();
  });

  test("should handle RATES_FETCH_START action", () => {
    const state = rootReducer(initalState, {
      type: "RATES_FETCH_START",
      payload: ["1993f7b9-f9be-551a-beac-312d6befd0cf", "41cae0fd-b74d-5304-a45c-ba000471eabd"],
    });
    expect(state).toMatchSnapshot();
  });

  test("should handle RATES_FETCH_SUCCESS action", () => {
    const state = rootReducer(initalState, {
      type: "RATES_FETCH_SUCCESS",
      payload: {
        "1993f7b9-f9be-551a-beac-312d6befd0cf": 2.4015,
        "41cae0fd-b74d-5304-a45c-ba000471eabd": 2.4907,
      },
    });
    expect(state).toMatchSnapshot();
  });

  test("should handle CONFIGS_FETCH_FAIL action", () => {
    const state = rootReducer(initalState, {
      type: "CONFIGS_FETCH_FAIL",
      payload: {
        error: {
          config: {
            transformRequest: {},
            transformResponse: {},
            timeout: 240000,
            xsrfCookieName: "XSRF-TOKEN",
            xsrfHeaderName: "X-XSRF-TOKEN",
            maxContentLength: -1,
            headers: { Accept: "application/json, text/plain, */*" },
            method: "get",
            baseURL: "http://localhost:3000/",
            url: "http://localhost:3000/configuration",
            "axios-retry": { retryCount: 0, lastRequestTime: 1537218227843 },
          },
          request: {},
        },
      },
    });
    expect(state).toMatchSnapshot();
  });

  test("should handle RATES_FETCH_FAIL action", () => {
    const state = rootReducer(initalState, {
      type: "RATES_FETCH_FAIL",
      payload: {
        error: {
          config: {
            transformRequest: {},
            transformResponse: {},
            timeout: 240000,
            xsrfCookieName: "XSRF-TOKEN",
            xsrfHeaderName: "X-XSRF-TOKEN",
            maxContentLength: -1,
            headers: { Accept: "application/json, text/plain, */*" },
            method: "get",
            baseURL: "http://localhost:3000/",
            url: "http://localhost:3000/rates",
            "axios-retry": { retryCount: 0, lastRequestTime: 1537218227843 },
          },
          request: {},
        },
      },
    });
    expect(state).toMatchSnapshot();
  });

  test("should handle SELECT_RATES_IDS action", () => {
    const state = rootReducer(initalState, {
      type: "SELECT_RATES_IDS",
      payload: {
        ids: ["41cae0fd-b74d-5304-a45c-ba000471eabd", "5b428ac9-ec57-513d-8a08-20199469fb4d"],
      },
    });
    expect(state).toMatchSnapshot();
  });
});

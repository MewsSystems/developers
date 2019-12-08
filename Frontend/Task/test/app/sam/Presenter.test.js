jest.mock("sam/Actions");
jest.mock("sam/Renderer");

import { present, nextAction } from "sam/Presenter";
import { updateCurrencyPairValues, decrementCounter, fetchCurrencyPairs } from "sam/Actions";
import { CONTROL_STATES } from "sam/Model";
import { render } from "sam/Renderer";
import { CURRENCY_PAIR_1, CURRENCY_PAIR_2, CURRENCY_PAIR_3 } from "Data";
import { CURRENCY_PAIRS_BY_IDS, INC_RATE_1, DEC_RATE_2, STG_RATE_3 } from "Data";
import { loadConfiguration } from "../../../app/sam/Actions";
import { getCopyOfCurrencyPairsById } from "../../Data";

describe("Presenter", () => {
    let state = null;
    beforeEach(() => {
        jest.clearAllMocks();
        render.mockImplementation(value => {
            state = value;
        });
    });
    describe("Next action", () => {
        let result = null;
        describe("on no action", () => {
            beforeEach(() => {
                result = nextAction({ control: CONTROL_STATES.NO_ACTION });
            });
            it("returns false", () => {
                expect(result).toBe(false);
            });
            it("doesn't invoke any action", () => {
                expect(fetchCurrencyPairs).not.toBeCalled();
                expect(updateCurrencyPairValues).not.toBeCalled();
                expect(decrementCounter).not.toBeCalled();
            });
        });
        describe("on time to update", () => {
            beforeEach(() => {
                result = nextAction({
                    control: CONTROL_STATES.TIME_TO_UPDATE,
                    currencyPairsById: { a: {}, b: {} },
                    endpoint: "localhost"
                });
            });
            it("returns false", () => {
                expect(result).toBe(false);
            });
            it("invokes update currency pairs values", () => {
                expect(updateCurrencyPairValues).toBeCalledTimes(1);
                expect(updateCurrencyPairValues).toBeCalledWith("localhost", ["a", "b"]);
            });
        });
        describe("on first fetch", () => {
            beforeEach(() => {
                result = nextAction({
                    control: CONTROL_STATES.FIRST_FETCH,
                    currencyPairsById: { a: {}, b: {} },
                    endpoint: "localhost"
                });
            });
            it("returns true", () => {
                expect(result).toBe(true);
            });
            it("invokes update currency pairs values", () => {
                expect(updateCurrencyPairValues).toBeCalledTimes(1);
                expect(updateCurrencyPairValues).toBeCalledWith("localhost", ["a", "b"]);
            });
        });
        describe("on decrement", () => {
            beforeEach(() => {
                result = nextAction({
                    control: CONTROL_STATES.DECREMENT
                });
            });
            it("returns false", () => {
                expect(result).toBe(false);
            });
            it("invokes decrement", () => {
                expect(decrementCounter).toBeCalledTimes(1);
            });
        });
        describe("on get configuration", () => {
            beforeEach(() => {
                result = nextAction({
                    control: CONTROL_STATES.GET_CONFIGURATION
                });
            });
            it("returns false", () => {
                expect(result).toBe(false);
            });
            it("fetches currency pairs", () => {
                expect(fetchCurrencyPairs).toBeCalledTimes(1);
            });
        });
        describe("on get configuration from db", () => {
            beforeEach(() => {
                result = nextAction({
                    control: CONTROL_STATES.GET_CONFIGURATION_FROM_DB
                });
            });
            it("returns false", () => {
                expect(result).toBe(false);
            });
            it("loads configuration", () => {
                expect(loadConfiguration).toBeCalledTimes(1);
            });
        });
    });
    describe("present message and counter", () => {
        const MODEL = {
            control: CONTROL_STATES.NO_ACTION,
            currencyPairsById: CURRENCY_PAIRS_BY_IDS,
            selectedPairIds: new Set()
        };
        describe("on time to update", () => {
            beforeEach(() => {
                MODEL.control = CONTROL_STATES.TIME_TO_UPDATE;
                present(MODEL);
            });
            it("generates message 'Updating'", () => {
                expect(render).toBeCalledTimes(1);
                expect(state.message).toEqual("Updating...");
            });
        });
        describe("on decrement and 4 secs", () => {
            beforeEach(() => {
                MODEL.control = CONTROL_STATES.DECREMENT;
                MODEL.counter = 4;
                present(MODEL);
            });
            it("generates message 'Next update in 4 secs'", () => {
                expect(render).toBeCalledTimes(1);
                expect(state.message).toEqual("Next update in 4 secs");
            });
        });
        describe("on decrement after fail and 4 secs", () => {
            beforeEach(() => {
                MODEL.control = CONTROL_STATES.DECREMENT;
                MODEL.lastFailed = true;
                MODEL.counter = 4;
                present(MODEL);
            });
            it("generates message 'Last update failed. Next in 4 secs'", () => {
                expect(render).toBeCalledTimes(1);
                expect(state.message).toEqual("Last update failed. Next in 4 secs");
            });
        });
    });
    describe("present", () => {
        describe("on no render", () => {
            beforeEach(() => {
                present({ control: CONTROL_STATES.FIRST_FETCH, currencyPairsById: {} });
            });
            it("doesn't call render", () => {
                expect(render).not.toBeCalled();
            });
        });
        describe("on empty model", () => {
            beforeEach(() => {
                present({
                    control: CONTROL_STATES.NO_ACTION,
                    currencyPairsById: {},
                    selectedPairIds: new Set()
                });
            });
            it("creates empty initial state", () => {
                expect(render).toBeCalledTimes(1);
                expect(state).toEqual({
                    pairs: [],
                    rates: [],
                    message: "Loading...",
                    selected: []
                });
            });
        });
        describe("on full model", () => {
            let currencyPairsById = null;
            beforeEach(() => {
                currencyPairsById = getCopyOfCurrencyPairsById();
                currencyPairsById[CURRENCY_PAIR_1.id].trend = INC_RATE_1.trend;
                currencyPairsById[CURRENCY_PAIR_1.id].rate = INC_RATE_1.rate;
                currencyPairsById[CURRENCY_PAIR_1.id].selected = true;
                currencyPairsById[CURRENCY_PAIR_2.id].trend = DEC_RATE_2.trend;
                currencyPairsById[CURRENCY_PAIR_2.id].rate = DEC_RATE_2.rate;
                currencyPairsById[CURRENCY_PAIR_3.id].trend = STG_RATE_3.trend;
                currencyPairsById[CURRENCY_PAIR_3.id].rate = STG_RATE_3.rate;
                present({
                    control: CONTROL_STATES.NO_ACTION,
                    currencyPairsById: currencyPairsById
                });
            });
            it("creates correct pairs with first pair selected", () => {
                expect(render).toBeCalledTimes(1);
                expect(state.pairs[0]).toEqual(currencyPairsById[CURRENCY_PAIR_1.id]);
                expect(state.pairs[1]).toEqual(currencyPairsById[CURRENCY_PAIR_2.id]);
                expect(state.pairs[2]).toEqual(currencyPairsById[CURRENCY_PAIR_3.id]);
            });
            it("creates copies of pairs", () => {
                expect(state.pairs[0]).not.toBe(currencyPairsById[CURRENCY_PAIR_1.id]);
                expect(state.pairs[1]).not.toBe(currencyPairsById[CURRENCY_PAIR_2.id]);
                expect(state.pairs[2]).not.toBe(currencyPairsById[CURRENCY_PAIR_3.id]);
            });
            it("creates rates only with selected first pair", () => {
                expect(render).toBeCalledTimes(1);
                expect(state.selected[0]).toEqual(currencyPairsById[CURRENCY_PAIR_1.id]);
                expect(state.selected[0]).not.toBe(currencyPairsById[CURRENCY_PAIR_1.id]);
            });
        });
    });
});

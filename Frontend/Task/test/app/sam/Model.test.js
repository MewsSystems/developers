jest.mock("sam/Presenter");
jest.mock("data/Configuration");

import { Model } from "sam/Model";
import { CONTROL_STATES } from "sam/Model";
import {
    Init,
    ToggleCurrencyPair,
    DecrementCounter,
    FailCurrencyPairUpdate,
    UpdateCurrencyPairValues,
    ConfigureCurrencyPairs
} from "sam/Proposals";
import { present } from "sam/Presenter";
import {
    ENDPOINT,
    INTERVAL,
    CURRENCY_PAIR_1,
    CURRENCY_PAIR_2,
    CURRENCY_PAIR_3,
    CURRENCY_PAIRS_BY_IDS,
    RATE_1,
    INC_RATE_1,
    RATE_2,
    DEC_RATE_2,
    RATE_3,
    STG_RATE_3,
    RATES_1,
    RATES_2
} from "Data";
import { getCopyOfCurrencyPairsById } from "../../Data";
import { configurationRepo } from "../../../app/data/Configuration";
import { LoadCurrencyPairs, FailConfigurationLoad } from "../../../app/sam/Proposals";

describe("Model", () => {
    let model = null;
    beforeEach(() => {
        jest.clearAllMocks();
        model = new Model();
    });
    it("can be created", () => {
        expect(model).toBeDefined();
    });
    it("has control state as no action", () => {
        expect(model.control).toBe(CONTROL_STATES.NO_ACTION);
    });

    describe("after init proposal", () => {
        beforeEach(() => {
            model.accept(new Init(ENDPOINT, INTERVAL));
        });
        it("presents itself", () => {
            expect(present).toBeCalledWith(model);
        });
        it("sets endpoint and interval", () => {
            expect(model.endpoint).toEqual(ENDPOINT);
            expect(model.interval).toEqual(INTERVAL);
        });
        it("sets control state as get configuration from db", () => {
            expect(model.control).toBe(CONTROL_STATES.GET_CONFIGURATION_FROM_DB);
        });
    });

    describe("on configure currency pairs proposal", () => {
        beforeEach(() => {
            model.accept(new ConfigureCurrencyPairs(getCopyOfCurrencyPairsById()));
        });
        it("stores pairs", () => {
            expect(model.currencyPairsById).toEqual(CURRENCY_PAIRS_BY_IDS);
        });
        it("sets control state as first fetch", () => {
            expect(model.control).toBe(CONTROL_STATES.FIRST_FETCH);
        });
        it("persists state", () => {
            expect(configurationRepo.storeConfiguration).toBeCalledTimes(1);
            expect(configurationRepo.storeConfiguration).toBeCalledWith(model.currencyPairsById);
        });
        describe("on failed rates update", () => {
            beforeEach(() => {
                model.accept(new FailCurrencyPairUpdate());
            });
            it("leaves control state as first fetch", () => {
                expect(model.control).toBe(CONTROL_STATES.FIRST_FETCH);
            });
        });

        describe("on first update of rates", () => {
            beforeEach(() => {
                model.accept(new UpdateCurrencyPairValues(RATES_1));
            });
            it("sets control state as decrement", () => {
                expect(model.control).toBe(CONTROL_STATES.DECREMENT);
            });
            it("computes trend as unknown and saves rate values", () => {
                const EXPECTED = {};
                EXPECTED[CURRENCY_PAIR_1.id] = { trend: "?", value: RATE_1 };
                EXPECTED[CURRENCY_PAIR_2.id] = { trend: "?", value: RATE_2 };
                EXPECTED[CURRENCY_PAIR_3.id] = { trend: "?", value: RATE_3 };
                const entries = Object.entries(model.currencyPairsById);
                expect(entries.length).toEqual(3);
                for (let [id, pair] of entries) {
                    expect(pair.trend).toEqual(EXPECTED[id].trend);
                    expect(pair.rate).toEqual(EXPECTED[id].value);
                }
            });
            describe("on next toggle currency pair 2 proposal", () => {
                const CURRENCY_PAIR = CURRENCY_PAIR_2;
                beforeEach(() => {
                    model.accept(new ToggleCurrencyPair(CURRENCY_PAIR.id));
                });
                it("sets control state as no action", () => {
                    expect(model.control).toBe(CONTROL_STATES.NO_ACTION);
                });
                it("stores currency pair 2 as selected", () => {
                    expect(model.currencyPairsById[CURRENCY_PAIR.id].selected).toBe(true);
                });
                describe("on another toggle of the same currency pair", () => {
                    beforeEach(() => {
                        model.accept(new ToggleCurrencyPair(CURRENCY_PAIR.id));
                    });
                    it("deletes currency pair from selection", () => {
                        expect(model.currencyPairsById[CURRENCY_PAIR.id].selected).toBe(false);
                    });
                });
                describe("on an decrement proposal", () => {
                    beforeEach(() => {
                        model.accept(new DecrementCounter());
                    });
                    it("sets control state as decrement", () => {
                        expect(model.control).toBe(CONTROL_STATES.DECREMENT);
                    });
                });
            });
            describe("on second update of rates", () => {
                beforeEach(() => {
                    model.accept(new UpdateCurrencyPairValues(RATES_2));
                });
                it("computes trend for pair 1 as '^' and for pair 2 as 'v'", () => {
                    const EXPECTED = [
                        [CURRENCY_PAIR_1.id, INC_RATE_1],
                        [CURRENCY_PAIR_2.id, DEC_RATE_2],
                        [CURRENCY_PAIR_3.id, STG_RATE_3]
                    ];
                    const entries = Object.entries(model.currencyPairsById);
                    expect(entries.length).toEqual(EXPECTED.length);
                    for (let [id, rate] of EXPECTED) {
                        expect(model.currencyPairsById[id].trend).toEqual(rate.trend);
                        expect(model.currencyPairsById[id].rate).toEqual(rate.value);
                    }
                });
            });
        });
        describe("counter", () => {
            describe("after interval is set to 2000 ms", () => {
                beforeEach(() => {
                    model.accept(new Init(ENDPOINT, 2000));
                });
                describe("on first update of rates", () => {
                    beforeEach(() => {
                        model.accept(new UpdateCurrencyPairValues(RATES_1));
                    });
                    it("is set to 2 seconds", () => {
                        expect(model.counter).toEqual(2);
                    });
                    describe("after decrement", () => {
                        beforeEach(() => {
                            model.accept(new DecrementCounter());
                        });
                        it("is 1 second", () => {
                            expect(model.counter).toEqual(1);
                        });
                        describe("after another decrement", () => {
                            beforeEach(() => {
                                model.accept(new DecrementCounter());
                            });
                            it("is 0 seconds", () => {
                                expect(model.counter).toEqual(0);
                            });
                            it("sets control as time to update", () => {
                                expect(model.control).toBe(CONTROL_STATES.TIME_TO_UPDATE);
                            });
                        });
                        describe("after failed update", () => {
                            beforeEach(() => {
                                model.accept(new FailCurrencyPairUpdate());
                            });
                            it("is set to 2 seconds again", () => {
                                expect(model.counter).toEqual(2);
                            });
                        });
                    });
                });
            });
        });
    });
    describe("on load of the currency pairs", () => {
        beforeEach(() => {
            model.accept(new LoadCurrencyPairs(getCopyOfCurrencyPairsById()));
        });
        it("sets control state as first fetch", () => {
            expect(model.control).toBe(CONTROL_STATES.FIRST_FETCH);
        });
    });

    describe("on empty load of the currency pairs", () => {
        beforeEach(() => {
            model.accept(new LoadCurrencyPairs({}));
        });
        it("sets control state as get configuration", () => {
            expect(model.control).toBe(CONTROL_STATES.GET_CONFIGURATION);
        });
    });

    describe("on failed load of the configuration", () => {
        beforeEach(() => {
            model.accept(new FailConfigurationLoad());
        });
        it("sets control state as get configuration", () => {
            expect(model.control).toBe(CONTROL_STATES.GET_CONFIGURATION);
        });
    });

    describe("on failed update", () => {
        beforeEach(() => {
            model.accept(new FailCurrencyPairUpdate());
        });
        it("sets control state as decrement", () => {
            expect(model.control).toBe(CONTROL_STATES.DECREMENT);
        });
    });
});

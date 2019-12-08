import { ConfigurationRepo, CONFIGURATION_DB } from "data/Configuration";

import { mockIndexedDB } from "TestUtils";
mockIndexedDB();
import { CURRENCY_PAIR_1 } from "Data";

describe("Configuration", () => {
    let repo = null;
    const mockGetDB = () => {
        repo.getDB = jest.fn(() => ({
            then: callback => callback(window.indexedDB.mock.openRequest.result)
        }));
    };
    beforeEach(() => {
        repo = new ConfigurationRepo();
    });
    it("can be created", () => {
        expect(repo).not.toBeNull();
    });
    describe("on first request for db", () => {
        let promise = null;
        beforeEach(() => {
            promise = repo.getDB();
        });
        it("opens a configuration db", () => {
            expect(window.indexedDB.open).toBeCalledTimes(1);
            expect(window.indexedDB.open).toBeCalledWith(CONFIGURATION_DB);
        });
        it("sets handlers for open request", () => {
            const request = window.indexedDB.mock.openRequest;
            expect(request.onerror).toBeDefined();
            expect(request.onupgradeneeded).toBeDefined();
            expect(request.onsuccess).toBeDefined();
        });
        describe("on success", () => {
            beforeEach(() => {
                window.indexedDB.mock.openRequest.onsuccess();
            });
            it("resolves with result", () => {
                return promise.then(result => {
                    expect(result).toBe(window.indexedDB.mock.openRequest.result);
                });
            });
        });

        describe("on upgrade needed", () => {
            beforeEach(() => {
                window.indexedDB.mock.openRequest.onupgradeneeded({
                    target: {
                        result: window.indexedDB.mock.openRequest.result
                    }
                });
            });
            it("creates indices on object store", () => {
                const db = window.indexedDB.mock.openRequest.result;
                const objectStore = window.indexedDB.mock.objectStore;
                expect(db.createObjectStore).toBeCalledWith(CONFIGURATION_DB, {
                    keyPath: "id"
                });
                expect(objectStore.createIndex).toBeCalledTimes(5);
            });
        });
    });
    describe("on get configuration", () => {
        let promise = null;
        beforeEach(() => {
            mockGetDB();
            promise = repo.getConfiguration();
            window.indexedDB.mock.openCursorAction.onsuccess({
                target: {
                    result: {
                        value: CURRENCY_PAIR_1,
                        continue: () => {
                            window.indexedDB.mock.openCursorAction.onsuccess({
                                target: {
                                    result: null
                                }
                            });
                        }
                    }
                }
            });
        });
        it("creates transaction", () => {
            return promise.then(result => {
                const currencyPairsById = {};
                currencyPairsById[CURRENCY_PAIR_1.id] = CURRENCY_PAIR_1;
                expect(result).toEqual(currencyPairsById);
            });
        });
    });
    describe("on store configuration", () => {
        let promise = null;
        beforeEach(() => {
            mockGetDB();
            const currencyPairsById = {};
            currencyPairsById[CURRENCY_PAIR_1.id] = CURRENCY_PAIR_1;
            promise = repo.storeConfiguration(currencyPairsById);
        });
        it("defines handlers on transaction", () => {
            const objectStoreFactory = window.indexedDB.mock.objectStoreFactory;
            const objectStore = window.indexedDB.mock.objectStore;
            expect(objectStoreFactory.oncomplete).toBeDefined();
            expect(objectStore.put).toBeCalledWith(CURRENCY_PAIR_1);
        });
        it("resolves on complete", () => {
            const objectStoreFactory = window.indexedDB.mock.objectStoreFactory;
            objectStoreFactory.oncomplete();
            return promise.then(() => {
                expect(true).toEqual(true);
            });
        });
    });
});

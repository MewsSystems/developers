import { CurrencyPair } from "./CurrencyPair";

export const CONFIGURATION_DB = "Configuration";

export class ConfigurationRepo {
    constructor() {
        this._db = null;
    }

    getConfiguration() {
        return this.getDB().then(db => {
            return this._getConfiguration(db);
        });
    }

    storeConfiguration(currencyPairsById) {
        return this.getDB().then(db => {
            return this._storeConfiguration(db, currencyPairsById);
        });
    }

    getDB() {
        return this._db
            ? Promise.resolve(this._db)
            : new Promise((resolve, reject) => {
                  const dbOpenRequest = window.indexedDB.open(CONFIGURATION_DB);
                  dbOpenRequest.onerror = reject;
                  dbOpenRequest.onupgradeneeded = event => {
                      const db = event.target.result;
                      db.onerror = reject;
                      const objectStore = db.createObjectStore(CONFIGURATION_DB, { keyPath: "id" });
                      objectStore.createIndex("frCode", "frCode", { unique: false });
                      objectStore.createIndex("frName", "frName", { unique: false });
                      objectStore.createIndex("toCode", "toCode", { unique: false });
                      objectStore.createIndex("toName", "toName", { unique: false });
                      objectStore.createIndex("selected", "selected", { unique: false });
                  };
                  dbOpenRequest.onsuccess = () => {
                      resolve(dbOpenRequest.result);
                  };
              });
    }

    _getConfiguration(db) {
        return new Promise(resolve => {
            const objectStore = db.transaction(CONFIGURATION_DB).objectStore(CONFIGURATION_DB);
            const currencyPairs = {};
            objectStore.openCursor().onsuccess = event => {
                const cursor = event.target.result;
                if (cursor) {
                    const currencyPair = cursor.value;
                    currencyPairs[currencyPair.id] = CurrencyPair.fromCursor(cursor);
                    cursor.continue();
                } else {
                    resolve(currencyPairs);
                }
            };
        });
    }

    _storeConfiguration(db, currencyPairsById) {
        return new Promise((resolve, reject) => {
            const transaction = db.transaction(CONFIGURATION_DB, "readwrite");
            transaction.oncomplete = resolve;
            transaction.onerror = reject;
            const objectStore = transaction.objectStore(CONFIGURATION_DB);
            for (let currencyPair of Object.values(currencyPairsById)) {
                objectStore.put(currencyPair);
            }
        });
    }
}

export const configurationRepo = new ConfigurationRepo();

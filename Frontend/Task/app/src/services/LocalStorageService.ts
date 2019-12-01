import CurrencyPair from "../models/Pair";
import { LocalStorageDTO } from "../models/DTOs";
import Currency from "../models/Currency";

/**Used as singleton */

class LocalStorageService {
  storage: Storage;
  canBackup: Boolean;

  constructor() {
    this.storage = window.localStorage;
    this.canBackup = this.canBackupCheck();
  }
  /**
   * Returns true if LocalStorage exists and can be used
   */
  canBackupCheck(): Boolean {
    try {
      var testString = "testjnsdfosodfkoisdfsdxcvxxvc";
      this.storage.setItem(testString, testString);
      this.storage.removeItem(testString);

      return true;
    } catch (e) {
      return false;
    }
  }
  /**
   *
   * @param currencyPairs from state
   * Returns true if object is saved to localStorage
   */
  backup(currencyPairs: Record<string, CurrencyPair>): Boolean {
    var backupObject: LocalStorageDTO = {};
    Object.keys(currencyPairs).forEach(id => {
      backupObject[id] = {
        currencies: currencyPairs[id].currencies.map(({ name, code }) => {
          return {
            name,
            code
          };
        }),
        shown: currencyPairs[id].shown
      };
    });
    if (this.canBackup) {
      this.storage.setItem("configData", JSON.stringify(backupObject));
      return true;
    } else {
      return false;
    }
  }
  /**
   * Returns false if LS is not accessible in the browser or when no data are stored,
   * LocalStorageDTO elsewhere
   */
  load(): LocalStorageDTO | false {
    if (this.canBackup) {
      var data: string | null = this.storage.getItem("configData");
      if (data === null) {
        return false;
      } else {
        var object = JSON.parse(data);
        var returnObject: LocalStorageDTO = {};

        Object.keys(object).forEach(id => {
          returnObject[id] = {
            currencies: object[id].currencies.map(
              (currency: Object) => currency as Currency
            ),
            shown: object[id].shown
          };
        });
        return returnObject;
      }
    } else {
      return false;
    }
  }
}

const singleton = new LocalStorageService();
export default singleton;

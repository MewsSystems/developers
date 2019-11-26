import CurrencyPair from "../models/Pair";
import { LocalStorageDTO } from "../models/DTOs";
import Currency from "../models/Currency";

/**Generates a class, which will handle local storage. On init -> check if LS can be used,
 *  then use that storage on call whether the LS can be used  */
/**Pointa - uložiť ids a is saved */
/**Uložit to vo formáte, ako to vieme dofre deserializovať, t.j.všetko o CurrencyPair - cucrr[] a show  */
/**TODO: fix reducers - new reducer, action -> RESOLVE_CONFIG */

/**Potrebujeme to savenut po prvom ulozeni */
class LocalStorageService {
  storage: Storage;
  canBackup: Boolean;

  constructor() {
    this.storage = window.localStorage;
    this.canBackup = this.canBackupCheck();
  }
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

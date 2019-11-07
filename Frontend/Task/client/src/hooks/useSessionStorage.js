import { useState, useEffect } from 'react';
import { prop, isNil, identity } from 'ramda';

const supportsSessionStorage = () => {
    if (prop('sessionStorage', window) && !isNil(window.sessionStorage)) {
        return true;
    } else {
        console.log('ERROR - Session storage not supported by browser, data will not be saved');
        return false;
    }
};

export default function (sessionStorageKey) {

    if (!supportsSessionStorage()) {
        return [null, identity];
    } else {
        const [storedValue, setStoredValue] = useState(() => {
                return prop(sessionStorageKey, sessionStorage)
                    ? JSON.parse(sessionStorage.getItem(sessionStorageKey))
                    : null;
            }
        );

        useEffect(() => {
            sessionStorage.setItem(sessionStorageKey, JSON.stringify(storedValue));
            return () => sessionStorage.clear();
        }, [storedValue, sessionStorageKey]);

        return [storedValue, setStoredValue];
    }

};

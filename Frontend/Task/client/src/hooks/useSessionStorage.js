import { useState, useEffect } from 'react';
import { prop, isNil } from 'ramda';

const supportsSessionStorage = () => {
    if (prop('sessionStorage', window) && !isNil(window.sessionStorage)) {
        return true;
    } else {
        console.log('ERROR - Session storage not supported by browser, data will not be saved');
        return false;
    }
};

// I cannot add cleanup here because for some reason it is triggered at page refresh
// I use then session storage instead of local storage, to have data cleaned up at the end of session
export default function (sessionStorageKey, initialValue) {

    if (!supportsSessionStorage()) {
        return useState(initialValue);
    } else {
        const [value, setValue] = useState(() => {
                return prop(sessionStorageKey, sessionStorage)
                    ? JSON.parse(sessionStorage.getItem(sessionStorageKey))
                    : initialValue;
            }
        );

        useEffect(() => {
            sessionStorage.setItem(sessionStorageKey, JSON.stringify(value));
        }, [value]);

        return [value, setValue];
    }
};

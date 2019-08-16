import {Storage} from "../../types/app";

export const userRates = {
    get: () => {
        return JSON.parse(localStorage.getItem(Storage.currentRates));
    },
    set: (value: string[]) => {
        localStorage.setItem(Storage.currentRates, JSON.stringify(value));
    }
};

export const appConfig = {
    get: () => {
        return JSON.parse(localStorage.getItem(Storage.currentConfig));
    },
    set: (value: string[]) => {
        localStorage.setItem(Storage.currentConfig, JSON.stringify(value));
    }
};

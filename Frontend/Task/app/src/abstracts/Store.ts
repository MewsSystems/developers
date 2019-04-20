import Singleton from "./Singleton";

class Store extends Singleton {

    public set(key: string, value: Object | string) {
        window.localStorage.setItem(key, JSON.stringify(value));
    }
    
    public get(key: string) {
        let json: string | null = window.localStorage.getItem(key);
        if (json === null) {
            return null;
        }
        try {
            return JSON.parse(json);
        } catch (e) {
            console.error("Store exception");
            return null;
        }
    }
}

export = Store;
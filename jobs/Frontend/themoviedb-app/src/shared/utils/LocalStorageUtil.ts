export interface LocalData {
    searchQuery?: string;
    pageNumber?: number;
    fromMovieDetailsPage?: boolean;
}

class LocalStoreUtil {
    SERVICE_PATH = '';

    public static set<K extends keyof LocalData>(key: K, value: LocalData[K]) {
        const stringifiedValue = JSON.stringify(value);
        localStorage.setItem(key, stringifiedValue);
    }

    public static get<K extends keyof LocalData>(
        key: K
    ): LocalData[K] | undefined {
        const value = localStorage.getItem(key);
        try {
            if (value) {
                return JSON.parse(value);
            }
        } catch {
            if (typeof value === 'string') {
                return value as unknown as LocalData[K];
            }
        }
        return undefined;
    }

    public static remove<K extends keyof LocalData>(key: K) {
        localStorage.removeItem(key);
    }

    public static clearAll() {
        localStorage.clear();
    }
}

export default LocalStoreUtil;

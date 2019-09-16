import 'url-search-params-polyfill';
import { UrlParams } from "containers/types";

export const initUrlParams = (): UrlParams => {
    const urlParams = new URLSearchParams(location.search);

    return {
        searchTerm: urlParams.get('q') || ""
    }
}
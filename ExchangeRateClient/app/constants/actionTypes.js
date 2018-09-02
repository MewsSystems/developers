import { keyMirror } from 'ramda-extension';

export default keyMirror({
    GET_CURRENCY_PAIRS_STARTED: null,
    GET_CURRENCY_PAIRS_SUCCEEDED: null,
    GET_CURRENCY_PAIRS_FAILED: null,

    INIT_CURRENCY_PAIRS_ORDERING: null,
    ORDER_CURRENCY_PAIRS: null,

    GET_RATES_STARTED: null,
    GET_RATES_SUCCEEDED: null,
    GET_RATES_FAILED: null,

    REMOVE_ERROR_MESSAGE: null,

    UPDATE_RATES_HISTORY: null,
    INIT_RATES_HISTORY: null,

    TOGGLE_TRACKING: null,
    COLLECTIVE_TOGGLE_TRACKING: null,

    SAVE_SETTINGS: null,

    SHOW_COUNTDOWN: null,
    UPDATE_CURRENT_PAGE: null,
});

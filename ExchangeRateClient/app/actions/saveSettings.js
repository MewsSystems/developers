import { actionTypes } from '../constants';

export default (config) => ({
    type: actionTypes.SAVE_SETTINGS,
    payload: config,
});

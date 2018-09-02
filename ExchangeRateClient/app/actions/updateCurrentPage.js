import { assocPath, insertAll } from 'ramda';
import { actionTypes } from '../constants';
import { getUiControl } from '../selectors';

export default (tableName) => (newPage) => (dispatch, getState) => {
    const pathToTablePage = insertAll(2, tableName, ['uiControl', 'tables', 'currentPage']);
    const updatedState = assocPath(pathToTablePage, newPage, getState());
    dispatch({ type: actionTypes.UPDATE_CURRENT_PAGE, payload: getUiControl(updatedState) });
};

export default (apiActionType, apiCall, onSuccess, onError) => (dispatch, getState) => {
    dispatch({ type: `${apiActionType}_STARTED` });
    apiCall.then(onSuccess(dispatch, getState), onError(dispatch, getState));
};

import axios from 'axios';
export const UPDATEVALUESBEGIN = 'UPDATEVALUESBEGIN';
export const UPDATEVALUESSUCCESS = 'UPDATEVALUESSUCCESS';
export const UPDATEVALUESERROR = 'UPDATEVALUESERROR';
export const FILTERPAIRS = 'FILTERPAIRS';

export const GETPAIRSBEGIN = 'GETPAIRSBEGIN';
export const GETPAIRSSUCCESS = 'GETPAIRSSUCCESS';
export const GETPAIRSERROR = 'GETPAIRSERROR';

export const updateValuesBegin = () => ({
  type: UPDATEVALUESBEGIN
});

export const updateValuesSuccess = (values) => ({
  type: UPDATEVALUESSUCCESS,
  values
});

export const updateValuesError = (e) => ({
  type: UPDATEVALUESERROR,
  error: e
});

export const getPairsBegin = () => ({
  type: GETPAIRSBEGIN
});

export const getPairsSuccess = (pairs) => ({
  type: GETPAIRSSUCCESS,
  pairs
})

export const getPairsError = (e) => {
  return {
    type: GETPAIRSERROR,
    error: e
  }
};

export const filterPairs = (query, type) => ({
  type: FILTERPAIRS,
  query,
  query_type: type
})

async function fetchValues(currencyPairIds) {
  let response = null;
  try{
    let {data, status} = await axios.request({
      method: 'get',
      url: 'http://localhost:3000/rates',
      params: {
        currencyPairIds
      },
      responseType: 'json'
    });
    if(parseInt(status) == 200 && data && data.rates){
      response = data.rates;
    }else{
      throw TypeError('Could not retreive pair values');
    }
  }catch(e){
    response = String(e);
  }

  return response;
}

async function fetchPairs() {
  let response = null;
  try{
    let {data, status} = await axios.request({
      method: 'get',
      url: 'http://localhost:3000/configuration',
      responseType: 'json'
    });
    if (parseInt(status) === 200 && data.currencyPairs){
      response = data.currencyPairs;
    }else{
      throw TypeError('Request failed');
    }
  }catch(e){
    response = String(e);
  }
  return response;
}

export const getValues = () => {
  return (dispatch, getState) => {
    const state = getState();
    const initialLoad = state.app.initialLoad;
    if(!initialLoad)
      return;

    const pairs = state.app.pairs;
    let selected_pairs = Object.keys(pairs);

    dispatch(updateValuesBegin());

    fetchValues(selected_pairs).then((response)=>{
      if(response.constructor === Object && response){
        dispatch(updateValuesSuccess(response));
      }else{
        dispatch(updateValuesError(String(response)));
      }
    }).catch((e)=>{
      dispatch(updateValuesError(String(e)));
    })
  }
}

export const setIntervalFetchValues = () => {
  return (dispatch) => {
    return setInterval(() => dispatch(getValues()), 10000);
  }
}



export const getPairs = () => {
  return (dispatch) => {
    dispatch(getPairsBegin());
    fetchPairs().then((response)=>{
      if(response.constructor === Object){
        dispatch(getPairsSuccess(response));
        dispatch(filterPairs());
        setTimeout(()=>dispatch(getValues()), 1000);
      }else{
        dispatch(getPairsError(response));
        setTimeout(()=>dispatch(getPairs()), 10000);
      }
    }).catch((e)=>{
      console.log(e);
      dispatch(getPairsError(String(e)));
      setTimeout(()=>dispatch(getPairs()), 10000);
    });
  }
}

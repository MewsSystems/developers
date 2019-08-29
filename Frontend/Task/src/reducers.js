const initialState = {
  pairs: {},
  pair_names: {},
  pair_codes: {},
  values: {},
  error_pairs: false,
  error_message_pairs: '',
  error_message_values: '',
  error_values: false,
  loading_pairs: true,
  loading_values : true,
  query: localStorage.getItem('query') || '',
  query_codes: localStorage.getItem('query_codes') || '',
  initialLoad: false,
  pairs_to_show: []
};

export default function appReducer (state = initialState, action){
  if(action.type === 'UPDATEVALUESBEGIN') {
    return Object.assign({}, state, {
      loading_values: true,
      error_values: false
    });
  } else if (action.type === 'UPDATEVALUESSUCCESS'){
    return Object.assign({}, state, {
      values: action.values,
      loading_values: false,
      error_values: false
    });
  } else if (action.type === 'UPDATEVALUESERROR') {
    return Object.assign({}, state, {
      loading_values: false,
      error_values: true,
      error_message_values: action.error
    });
  } else if (action.type === 'GETPAIRSBEGIN') {
    return Object.assign({}, state, {
      pairs: {},
      loading_pairs: true,
      error_pairs: false
    });
  } else if (action.type === 'GETPAIRSSUCCESS') {
    let pair_names = {};
    let pair_codes = {};
    for(let key in action.pairs){
      let [first_currency, second_currency] = action.pairs[key];
      let name = first_currency.name+"/"+second_currency.name;
      let code = first_currency.code+"/"+second_currency.code;
      pair_names[key] = name;
      pair_codes[key] = code;
    }
    return Object.assign({}, state, {
      pairs: action.pairs,
      loading_pairs: false,
      error_pairs: false,
      pair_names,
      pair_codes,
      pairs_to_show: Object.keys(pair_names),
      initialLoad: true
    });
  } else if (action.type === 'GETPAIRSERROR') {
    return Object.assign({}, state, {
      pairs: {},
      loading_pairs: false,
      error_pairs: true,
      error_message_pairs: action.error
    });
  } else if (action.type === 'FILTERPAIRS') {
    let query_storage_item = action.query_type;
    let query = '';
    let query_codes = '';
    let selected_pairs = [];

    let pair_names = state.pair_names;
    let pair_codes = state.pair_codes;

    if(query_storage_item === 'codes'){
      query_codes = action.query || '';
    }else{
      query_codes = state.query_codes || '';
    }

    if(query_storage_item === 'names'){
      query = action.query || '';
    }else{
      query = state.query || '';
    }

    localStorage.setItem('query_codes', query_codes);
    localStorage.setItem('query', query);

    for(let key in pair_names){

      if(query && query !== '' && pair_names[key]){
        let name = pair_names[key];
        let pattern = new RegExp(query, "i");

        if(!pattern.test(name)){
          continue;
        }
      }

      if(query_codes && query_codes !== '' && pair_codes[key]){
        let code = pair_codes[key];
        let pattern = new RegExp(query_codes, "i");

        if(!pattern.test(code)){
          continue;
        }
      }
      selected_pairs.push(key);
    }

    if(query === '' && query_codes === ''){
      selected_pairs = Object.keys(state.pair_names);
    }

    return Object.assign({},state,{
      pairs_to_show: selected_pairs,
      query,
      query_codes
    });
  }
  return state;
}

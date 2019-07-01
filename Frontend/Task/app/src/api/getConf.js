import * as R from 'ramda';
import qs from 'qs';

export const fetchConfig = async () => {
  try {
    const config = await fetch('http://localhost:3000/configuration').then(
      response => response.json()
    );
    return config;
  } catch (err) {
    console.log(err);
  } finally {
    console.log('finally!!!');
  }
};

// const formatMe = async () => await fetchConfig;
// const currencyPairIds = R.keys(formatMe());
// console.log("currPair format", currencyPairIds);
// fetch("http://localhost:3000/configuration")
//   .then(function(response) {
//     return response.json();
//   })
//   .then(myJson => {
//     console.log(myJson);
//     setConfig(myJson);
//     const RR = {
//       currencyPairIds: R.keys(myJson.currencyPairs)
//     };
//     console.log(RR);
//     fetch(`http://localhost:3000/rates?${qs.stringify(RR)}`).then(response =>
//       console.log(response)
//     );
//   });

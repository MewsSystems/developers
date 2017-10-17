const isOk = res => res.status >= 200 && res.status < 300;

const callApi = (url, params) =>
    new Promise((resolve, reject) => {
        fetch(url, params)
            .then(res => {
              if (isOk(res)) {
                return res.json();
              }
              reject();
            })
            .then(data => resolve(data))
            .catch(err => reject(err));
    });

const getConfig = () => callApi('/api/configuration');

const getRatesQuery = (ids) => ids.map(id => `currencyPairIds=${id}`).join('&');
const getRates = (ids = []) => callApi(`/api/rates?${getRatesQuery(ids)}`);

export {getRates, getConfig};

import axios from 'axios';


/**
 * Axios fetch
 */
export const fetchData = ({
  method = 'get',
  headers = {
    Accept: 'application/json',
    'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
  },
  url,
  data,
  timeout = 6000,
  params,
}) => axios({
  method,
  headers,
  url,
  data,
  timeout,
  params,
});

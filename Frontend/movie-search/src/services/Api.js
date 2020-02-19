import axios from 'axios';
import { BASE_API_URL } from './Constants';

export function processRequest(url = '', method = 'GET') {
  let headers = {
    'Content-Type': 'application/json',
  };

  return axios({
    method,
    headers,
    crossDomain: true,
    url: `${BASE_API_URL}${url}&api_key=03b8572954325680265531140190fd2a`,
  })
}

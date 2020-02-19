import axios from 'axios';
import { BASE_API_URL } from './Constants';

export function processRequest(url = '', method = 'GET', data = {}) {
  const access_token = localStorage.getItem('access_token');
  let headers = {
    'Content-Type': 'application/json',
  };

  if(url !== 'authentication')
    headers = { ...headers, 'Authorization': `Bearer ${access_token}` };

  return axios({
    method,
    data: JSON.stringify(data),
    headers,
    crossDomain: true,
    url: `${BASE_API_URL}${url}&api_key=03b8572954325680265531140190fd2a`,
  })
}

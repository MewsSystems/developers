import { fetchApi } from './api';
import * as Endpoints from '../constants/endpoints';
import { CONFIGURATION } from '../constants/actionTypes';

/**
 * Gets initial app configuration
 * @returns {function(*)}
 */
export default function getInitialConfiguration() {
  return dispatch => dispatch(fetchApi(
    Endpoints.CONFIGURATION,
    CONFIGURATION,
  ));
}

import { createGetRequest } from '../api';

/**
 * Currency model
 */
class Currency {
  /**
   * Load rates configuration
   * @return {Promise<*>}
   */
  static async loadConfiguration() {
    try {
      const response = await createGetRequest('/configuration');
      console.log('');

      return response.data;
    } catch (response) {
      throw response.data || 'Failed to load configuration';
    }
  }

  /**
   * Load rates
   * @param {Object} params
   * @return {Promise<*>}
   */
  static async loadRates(params = {}) {
    try {
      const response = await createGetRequest('/rates', params);

      return response.data;
    } catch (response) {
      throw response.data || 'Failed to load rates';
    }
  }
}

export default Currency;
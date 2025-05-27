import { createApiClient } from '@services/fetch-rest-client';
import { RestClient } from '@services/types/rest-api';
export const movieApi: RestClient = createApiClient();

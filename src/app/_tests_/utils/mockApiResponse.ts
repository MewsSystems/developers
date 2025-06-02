import type {Page} from '@playwright/test';
import type {API_STATUS_MESSAGE} from '../../../api/movieApi/constants';
import {MOVIE_API_BASE_URL} from '../../../api/movieApi/constants';

type MockApiResponseParams = {
  page: Page;
  url: string;
  status: number;
  statusCode: number;
  statusMessage: (typeof API_STATUS_MESSAGE)[keyof typeof API_STATUS_MESSAGE];
};

export const mockApiResponse = async ({
  page,
  url,
  status,
  statusCode,
  statusMessage,
}: MockApiResponseParams) => {
  const requestUrl = `${MOVIE_API_BASE_URL}${url}*`;

  await page.route(requestUrl, async (route) => {
    await route.fulfill({
      status,
      contentType: 'application/json',
      body: JSON.stringify({
        success: false,
        status_code: statusCode,
        status_message: statusMessage,
      }),
    });
  });
};

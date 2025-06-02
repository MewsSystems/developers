import {expect, test} from '@playwright/test';
import {
  API_STATUS_MESSAGE,
  ERRORS_BY_HTTP_STATUS,
  MOVIE_API_BASE_URL,
} from '../../../api/movieApi/constants';
import {mockApiResponse} from '../utils/mockApiResponse';

const MOVIE_ID = '603';
const API_PATH = `/movie/${MOVIE_ID}`;

test.describe('API errors', () => {
  test.describe('<MovieDetailsPage />', () => {
    test('should display the "Resource not found" error message', async ({page}) => {
      await mockApiResponse({
        page,
        url: API_PATH,
        status: 404,
        statusCode: 34,
        statusMessage: API_STATUS_MESSAGE.RESOURCE_NOT_FOUND,
      });

      const apiResponsePromise = page.waitForResponse(`${MOVIE_API_BASE_URL}${API_PATH}*`);
      await page.goto(API_PATH);
      await apiResponsePromise;

      await expect(
        page.getByText(ERRORS_BY_HTTP_STATUS[404][API_STATUS_MESSAGE.RESOURCE_NOT_FOUND]),
      ).toBeVisible({
        timeout: 10_000,
      });
      await expect(page.getByRole('button', {name: 'Go to Homepage'})).toBeVisible();

      await page.getByRole('button', {name: 'Go to Homepage'}).click();
      await expect(page).toHaveURL('/');
    });
    test('should display the "Invalid movie ID provided." error message', async ({page}) => {
      await mockApiResponse({
        page,
        url: API_PATH,
        status: 500,
        statusCode: 44,
        statusMessage: API_STATUS_MESSAGE.INVALID_ID,
      });

      const apiResponsePromise = page.waitForResponse(`${MOVIE_API_BASE_URL}${API_PATH}*`);
      await page.goto(API_PATH);
      await apiResponsePromise;

      await expect(
        page.getByText(ERRORS_BY_HTTP_STATUS[500][API_STATUS_MESSAGE.INVALID_ID]),
      ).toBeVisible({timeout: 10_000});
      await expect(page.getByRole('button', {name: 'Go to Homepage'})).toBeVisible();

      await page.getByRole('button', {name: 'Go to Homepage'}).click();
      await expect(page).toHaveURL('/');
    });
    test('should display the "Invalid API key" error message', async ({page}) => {
      const SEARCH_API_PATH = '/search/movie';

      await mockApiResponse({
        page,
        url: SEARCH_API_PATH,
        status: 401,
        statusCode: 7,
        statusMessage: API_STATUS_MESSAGE.INVALID_API_KEY,
      });

      const apiResponsePromise = page.waitForResponse(`${MOVIE_API_BASE_URL}${SEARCH_API_PATH}*`);
      await page.goto('/?search=Matrix');
      await apiResponsePromise;

      await expect(
        page.getByText(ERRORS_BY_HTTP_STATUS[401][API_STATUS_MESSAGE.INVALID_API_KEY]),
      ).toBeVisible({timeout: 10_000});
      await expect(page.getByRole('button', {name: 'Go to Homepage'})).toBeVisible();

      await page.getByRole('button', {name: 'Go to Homepage'}).click();
      await expect(page).toHaveURL('/');
    });
  });
  test.describe('<MoviesListPage />', () => {
    test('should display the "Invalid API key" error message on direct URL navigation', async ({
      page,
    }) => {
      const SEARCH_API_PATH = '/search/movie';

      await mockApiResponse({
        page,
        url: SEARCH_API_PATH,
        status: 401,
        statusCode: 7,
        statusMessage: API_STATUS_MESSAGE.INVALID_API_KEY,
      });

      await page.goto('/?search=Matrix');
      await page.waitForResponse(`${MOVIE_API_BASE_URL}${SEARCH_API_PATH}*`);

      await expect(
        page.getByText(ERRORS_BY_HTTP_STATUS[401][API_STATUS_MESSAGE.INVALID_API_KEY]),
      ).toBeVisible({timeout: 10_000});
      await expect(page.getByRole('button', {name: 'Go to Homepage'})).toBeVisible();

      await page.getByRole('button', {name: 'Go to Homepage'}).click();
      await expect(page).toHaveURL('/');
    });
  });
});

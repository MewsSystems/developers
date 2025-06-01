import {expect, test} from '@playwright/test';
import {API_STATUS_MESSAGE, MOVIE_API_BASE_URL} from '../../api/movieApi/constants.ts';

test.describe('API errors', () => {
  test('should display the "Resource not found" error message', async ({page}) => {
    await page.route(`${MOVIE_API_BASE_URL}/movie/603*`, async (route) => {
      await route.fulfill({
        status: 404,
        contentType: 'application/json',
        body: JSON.stringify({
          success: false,
          status_code: 34,
          status_message: API_STATUS_MESSAGE.RESOURCE_NOT_FOUND,
        }),
      });
    });

    const apiResponsePromise = page.waitForResponse(`${MOVIE_API_BASE_URL}/movie/603*`);
    await page.goto('/movies/603');
    await apiResponsePromise;

    await expect(page.getByText("We couldn't find what you're looking for.")).toBeVisible({
      timeout: 10000,
    });
    await expect(page.getByRole('button', {name: 'Go to Homepage'})).toBeVisible();

    await page.getByRole('button', {name: 'Go to Homepage'}).click();
    await expect(page).toHaveURL('/');
  });
  test('should display the "Invalid movie ID provided." error message', async ({page}) => {
    await page.route(`${MOVIE_API_BASE_URL}/movie/603*`, async (route) => {
      await route.fulfill({
        status: 500,
        contentType: 'application/json',
        body: JSON.stringify({
          success: false,
          status_code: 44,
          status_message: API_STATUS_MESSAGE.INVALID_ID,
        }),
      });
    });

    const apiResponsePromise = page.waitForResponse(`${MOVIE_API_BASE_URL}/movie/603*`);
    await page.goto('/movies/603');
    await apiResponsePromise;

    await expect(page.getByText('Invalid movie ID provided.')).toBeVisible({timeout: 10000});
    await expect(page.getByRole('button', {name: 'Go to Homepage'})).toBeVisible();

    await page.getByRole('button', {name: 'Go to Homepage'}).click();
    await expect(page).toHaveURL('/');
  });
  test('should display the "Invalid API key" error message', async ({page}) => {
    await page.route(`${MOVIE_API_BASE_URL}/search/movie*`, async (route) => {
      await route.fulfill({
        status: 401,
        contentType: 'application/json',
        body: JSON.stringify({
          success: false,
          status_code: 7,
          status_message: API_STATUS_MESSAGE.INVALID_API_KEY,
        }),
      });
    });

    const apiResponsePromise = page.waitForResponse(`${MOVIE_API_BASE_URL}/search/movie*`);
    await page.goto('/?search=Matrix');
    await apiResponsePromise;

    await expect(
      page.getByText('Your API key is invalid. Please check your configuration.'),
    ).toBeVisible({timeout: 10000});
    await expect(page.getByRole('button', {name: 'Go to Homepage'})).toBeVisible();

    await page.getByRole('button', {name: 'Go to Homepage'}).click();
    await expect(page).toHaveURL('/');
  });
});

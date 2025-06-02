import {expect, test} from '@playwright/test';

const SEARCH_INPUT_PLACEHOLDER = 'Start typing to discover';

test.describe('Search and navigation', () => {
  test('should show empty state after clicking "Clear X"', async ({page}) => {
    await page.goto('/');

    const searchInput = page.getByPlaceholder(SEARCH_INPUT_PLACEHOLDER);
    await searchInput.fill('Matrix');

    await page.waitForResponse(
      (response) => response.url().includes('/search/movie') && response.status() === 200,
    );

    await page.getByRole('button', {name: 'Clear search'}).click();

    await expect(searchInput).toHaveValue('');
    await expect(page).toHaveURL('/');
    await expect(page.getByText('Ready to explore?')).toBeVisible();
    await expect(page.getByText('Your next favorite movie is just a search away!')).toBeVisible();
  });
  test('should search for a movie, navigate to the details page and verify the content', async ({
    page,
  }) => {
    await page.goto('/');

    const searchInput = page.getByPlaceholder(SEARCH_INPUT_PLACEHOLDER);
    await searchInput.fill('Matrix');

    await page.waitForResponse(
      (response) => response.url().includes('/search/movie') && response.status() === 200,
    );
    await page.waitForSelector('[role="progressbar"]', {state: 'hidden'});

    const movieTitles = await page.locator('h2').all();
    expect(movieTitles.length).toBeGreaterThan(0);

    const titles = await Promise.all(movieTitles.map((title) => title.textContent()));
    const matrixMovieIndex = titles.findIndex((title) => title?.toLowerCase().includes('matrix'));

    expect(matrixMovieIndex).toBeGreaterThanOrEqual(0);

    await movieTitles[matrixMovieIndex].click();
    await page.waitForResponse(
      (response) => response.url().includes('/movie/') && response.status() === 200,
    );
    await page.waitForSelector('[role="progressbar"]', {state: 'hidden'});

    const movieTitle = await page.locator('h1').first().textContent();
    expect(movieTitle?.toLowerCase()).toContain('matrix');

    await expect(page.locator('p').first()).toBeVisible();
    await expect(page.getByText('Release Date')).toBeVisible();
    await expect(page.getByText('★')).toBeVisible();

    await page.getByText('Back to search').click();
    await expect(page).toHaveURL(new RegExp('/?search=Matrix'));
    await expect(searchInput).toHaveValue('Matrix');
  });
  test('should show empty state if not matching results', async ({page}) => {
    await page.goto('/');

    const searchInput = page.getByPlaceholder(SEARCH_INPUT_PLACEHOLDER);
    await searchInput.fill('xxxxxxxxxxxxxxxxxxx');

    await page.waitForResponse(
      (response) => response.url().includes('/search/movie') && response.status() === 200,
    );
    await page.waitForSelector('[role="progressbar"]', {state: 'hidden'});

    await expect(
      page.getByText('We could not find any movies matching your search criteria.'),
    ).toBeVisible();
  });
});

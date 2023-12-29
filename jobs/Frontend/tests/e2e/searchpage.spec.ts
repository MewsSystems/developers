import { expect, test } from '@playwright/test'

test('should find specific movie, open its detail and get back to search page', async ({
  page,
}) => {
  await page.goto('/')
  await page.fill('input[name="searchbar"]', 'ready player one')

  // user finds one movie
  await expect(page.getByTestId('movie')).toHaveCount(1)
  await expect(page.getByTestId('paginator-button')).toHaveCount(3) // one page, previous and next button

  await page.getByTestId('poster-image').click()
  // user gets redirected to movie detail
  await expect(page).toHaveURL(/\/movie\/[0-9a-zA-Z]+/)

  await page.getByTestId('back-button').click()
  // user gets redirected back to search
  await expect(page).not.toHaveURL(/\/movie\/[0-9a-zA-Z]+/)
})

test('should reflect search in URL', async ({ page }) => {
  await page.goto('/')

  // adds 'query="read"' to browser history
  await page.fill('input[name="searchbar"]', 'read')
  await expect(page).toHaveURL(/.*\?query=read/)

  // adds 'query="ready player one"' to browser history
  await page.fill('input[name="searchbar"]', 'ready player one')
  await expect(page).toHaveURL(/.*\?query=ready\+player\+one/)

  // go back in history to 'query="read"'
  await page.goBack()
  await expect(page).toHaveURL(/.*\?query=read/)

  await page.fill('input[name="searchbar"]', '')
  await expect(page).not.toHaveURL(/.*\?query/)
})

test('should have working paginator', async ({ page }) => {
  await page.goto('/')

  await expect(page.getByTestId('paginator-button')).toHaveCount(7) // five pages, previous and next button
  await expect(page.getByTestId('movie-name')).toHaveCount(20) // 20 movies per page
  const firstOnPage = await page.getByTestId('movie-name').first().textContent()

  // go to second page
  await page.getByTestId('paginator-button').nth(3).click()
  // first movie on second page is different from the first movie on first page
  await expect(page.getByTestId('movie-name').first()).not.toHaveText(
    firstOnPage ?? '',
  )

  // go back to page one with previous button
  await page.getByTestId('paginator-button').nth(1).click()
  // if the movie is the same we know we are back on the page one successfully
  await expect(page.getByTestId('movie-name').first()).toHaveText(
    firstOnPage ?? '',
  )
})

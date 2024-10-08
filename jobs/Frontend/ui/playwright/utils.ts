import { Page } from "@playwright/test"

export const buildLocalUrl = (port: string, path: string = "") =>
  `http://localhost:${port}${path}`

type TestUtilsArgs = {
  page: Page
}
export const createTestUtils = (params: TestUtilsArgs) => {
  const { page } = params
  const pageObjects = {}

  return {
    po: pageObjects,
    page,
  }
}

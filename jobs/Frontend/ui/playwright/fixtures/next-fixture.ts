import { Page, Route, test as base } from "@playwright/test"
import { type SetupServer } from "msw/node"

import { server } from "test/server"

import { setupNextServer } from "../setup"
import { buildLocalUrl, createTestUtils } from "../utils"

export const test = base.extend<{
  utils: ReturnType<typeof createTestUtils>
  port: string
  serverRequestInterceptor: SetupServer
  interceptBrowserRequest: (
    url: string | RegExp,
    options: Parameters<Route["fulfill"]>[0],
  ) => Promise<void>
  revalidatePath: (page: Page, path: string) => Promise<void>
}>({
  baseURL: async ({ port }, use) => {
    await use(buildLocalUrl(port))
  },
  utils: async ({ page }, use) => {
    const u = createTestUtils({ page })

    await use(u)
  },
  serverRequestInterceptor: [
    async ({}, use) => {
      server.listen({ onUnhandledRequest: "bypass" })

      await use(server)

      server.close()
    },
    {
      scope: "test",
    },
  ],
  interceptBrowserRequest: [
    async ({ page }, use) => {
      async function interceptBrowserRequest(
        url: string | RegExp,
        options: Parameters<Route["fulfill"]>[0],
      ) {
        await page.route(url, async (route) => {
          await route.fulfill(options)
        })
      }

      await use(interceptBrowserRequest)
    },
    { scope: "test" },
  ],
  revalidatePath: async ({ port }, use) => {
    async function revalidatePath(page: Page, path: string) {
      await page.goto(buildLocalUrl(port, `/api/revalidatePath?path=${path}`))
    }

    await use(revalidatePath)
  },
  port: [
    async ({}, use) => {
      const port = await setupNextServer()

      await use(port)
    },
    { auto: true, scope: "test" },
  ],
})

export default test

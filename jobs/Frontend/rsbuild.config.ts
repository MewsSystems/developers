import { defineConfig } from '@rsbuild/core'
import { pluginReact } from '@rsbuild/plugin-react'

export default defineConfig({
  plugins: [pluginReact()],
  html: {
    template: './static/index.html',
    favicon: './static/favicon.png',
  },
  source: {
    define: {
      PUBLIC_API_KEY: JSON.stringify(process.env.PUBLIC_API_KEY),
      PUBLIC_API_URL: JSON.stringify(process.env.PUBLIC_API_URL),
    },
  },
})

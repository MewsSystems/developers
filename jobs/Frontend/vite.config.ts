//import { resolve } from 'path'
import { defineConfig } from 'vitest/config'
import react from '@vitejs/plugin-react'
import { TanStackRouterVite } from '@tanstack/router-vite-plugin'

// https://vitejs.dev/config/
export default defineConfig({
	plugins: [react(), TanStackRouterVite()],
	test: {
		// 👋 add the line below to add jsdom to vite
		environment: 'jsdom',
		globals: true,
		setupFiles: './src/tests/setup.ts',
	},
	resolve: {
		alias: {
			'@': '/src',
		},
	},
})

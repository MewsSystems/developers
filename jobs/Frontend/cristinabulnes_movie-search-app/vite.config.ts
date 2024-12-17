import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";
import EnvironmentPlugin from "vite-plugin-environment";

// https://vite.dev/config/
export default defineConfig({
	plugins: [react(), EnvironmentPlugin("all")],
	define: {
		"process.env": process.env,
	},
});

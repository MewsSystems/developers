import { defineConfig } from "vite";
import react from "@vitejs/plugin-react-swc";
import richSvg from "vite-plugin-react-rich-svg";

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    react({ plugins: [["@swc/plugin-styled-components", {}]] }),
    richSvg(),
  ],
  resolve: {
    alias: {
      src: "/src",
    },
  },
  server: {
    port: 3000,
  },
});

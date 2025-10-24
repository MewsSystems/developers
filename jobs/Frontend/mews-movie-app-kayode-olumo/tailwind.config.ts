import type { Config } from "tailwindcss";

export default {
  content: ["./app/**/*.{ts,tsx}", "./components/**/*.{ts,tsx}"],
  darkMode: "class",
  theme: {
    extend: {
      colors: {
        background: "#0b0b0b",
        foreground: "#f3f4f6",
        muted: "#9ca3af",
        card: "#161616",
        border: "#27272a",
        accent: "#00c2a8"
      },
      boxShadow: { 
        card: "0 8px 24px rgba(0,0,0,0.4)" 
      },
      borderRadius: { 
        xl: "1rem", 
        "2xl": "1.25rem" 
      }
    }
  },
  plugins: []
} satisfies Config;

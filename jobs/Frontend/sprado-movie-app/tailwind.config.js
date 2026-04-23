/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./src/**/*.{ts,tsx}'],
  theme: {
    extend: {},
  },
  theme: {
    extend: {
      colors: {
        darkDefault: "#121212",
        darkSoft: "#1F1F1F",
      },
      fontFamily: {
        body: ['Open Sans', 'sans-serif'],
        title: ['Libre Baskerville', 'serif'],
      },
    },
    
  },
  plugins: [
    function ({ addBase, theme }) {
      addBase({
        'body': { fontFamily: theme('fontFamily.body') },
      });
    },
  ],
}
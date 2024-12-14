/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./src/**/*.{ts,tsx}'],
  theme: {
    extend: {},
  },
  theme: {
    extend: {
      colors: {
        navyBlue: '#181E4B',
        turquoise: '#04BFAE',
        goldenYellow: '#FFC229',
        salmonPink: '#F36B77',
        lightGrey: '#EBEBF0',
        fontGrey: '#84829A',
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
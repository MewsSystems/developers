/** @type {import('tailwindcss').Config} */
// eslint-disable-next-line
const defaultTheme = require('tailwindcss/defaultTheme')

export default {
    content: ['./index.html', './src/**/*.{js,jsx,ts,tsx}'],
    corePlugins: {
        preflight: false,
    },
    theme: {
        fontFamily: {
            sans: ['Kanit', ...defaultTheme.fontFamily.sans],
        },
        container: {
            center: true,
            screens: {
                none: '100%',
                sm: '640px',
                md: '768px',
            },
            padding: {
                DEFAULT: '1.25rem',
            },
        },
        extend: {
            colors: {
                primary: {
                    main: '#AADBE7',
                    contrastText: '#525B69',
                },
                secondary: {
                    main: '#16174C',
                },
            },
        },
    },
    plugins: ['prettier-plugin-tailwindcss'],
}

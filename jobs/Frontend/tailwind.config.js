/** @type {import('tailwindcss').Config} */
export default {
    content: ['./index.html', './src/**/*.{js,jsx,ts,tsx}'],
    corePlugins: {
        preflight: false,
    },
    theme: {
        container: {
            center: true,
            padding: {
                DEFAULT: '1.25rem',
            },
        },
        extend: {},
    },
    plugins: ['prettier-plugin-tailwindcss'],
}

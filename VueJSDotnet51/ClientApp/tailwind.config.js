module.exports = {
    purge: {
        enabled: false,
        content: ['./index.html', './src/**/*.{vue,js,ts}']
    },
    darkMode: false, // or 'media' or 'class'
    theme: {
        extend: {},
    },
    variants: {
        extend: {},
    },
    plugins: [
        require('@tailwindcss/forms'),
    ],
}

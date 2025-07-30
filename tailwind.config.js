/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './**/*.razor',
        './**/*.cshtml',
        './**/*.html',
        './**/*.js'
    ],
    safelist: [
        {
            pattern: /text-(gray|pink|red|green|blue|purple|yellow)-(100|200|300|400|500|600|700|800|900)/,
        },
        {
            pattern: /bg-(gray|pink|red|green|blue|purple|yellow)-(100|200|300|400|500|600|700|800|900)/,
        },
        {
            pattern: /bg-gradient-to-(r|l|t|b)/,
        },
        {
            pattern: /from-(gray|pink|purple)-(100|500|700)/,
        },
        {
            pattern: /to-(gray|pink|purple)-(100|500|700)/,
        },
    ],
    theme: {
        extend: {},
    },
    plugins: [],
}

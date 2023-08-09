module.exports = {
  root: true,
  env: {
    node: true,
    "vue/setup-compiler-macros": true
  },
  'extends': [
    'plugin:vue/vue3-essential',
    'eslint:recommended',
    '@vue/typescript/recommended'
  ],
  parserOptions: {
    ecmaVersion: 2020
  },
  rules: {
    'no-console': process.env.NODE_ENV === 'production' ? 'warn' : 'off',
    'no-debugger': process.env.NODE_ENV === 'production' ? 'warn' : 'off'
  },
  overrides: [
    {
        // Apply rule override only to files with the following extensions
        files: ['*.vue', '*.ts'],
        rules: {
            '@typescript-eslint/ban-types': [
                'error',
                {
                    extendDefaults: true,
                    types: {
                        '{}': false,
                    },
                },
            ],
        },
    },
]
}

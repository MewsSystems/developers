module.exports = {
    "env": {
        "browser": true,
        "es6": true,
				"jest": true
    },
    "extends": "airbnb",
    "globals": {
        "Atomics": "readonly",
        "SharedArrayBuffer": "readonly"
    },
    "parserOptions": {
        "ecmaFeatures": {
            "jsx": true
        },
        "ecmaVersion": 2018,
        "sourceType": "module"
    },
		"parser": "babel-eslint",
    "plugins": [
        "react"
    ],
    "rules": {
			"indent": "off",
			"no-tabs": 0,
			"no-mixed-spaces-and-tabs": 0,
			'no-plusplus': [2, { allowForLoopAfterthoughts: true }],
			"no-param-reassign": 0
    }
};

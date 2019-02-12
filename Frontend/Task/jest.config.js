module.exports = {
	roots: [`src`],
	collectCoverageFrom: [`scr/**/*.{js,jsx}`],
	coverageThreshold: {
		global: {
			statements: 98,
			branches: 91,
			functions: 98,
			lines: 98
		}
	},
	moduleDirectories: [
		`node_modules`,
		`src`
	],
	unmockedModulePathPatterns: [
		`node_modules/react/`
	],
	testRegex: `tests/.*\\.test\\.js$`
}

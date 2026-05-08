export default {
	preset: "ts-jest",
	testEnvironment: "jsdom",
	setupFilesAfterEnv: ["<rootDir>/src/setupTests.ts"],
	moduleNameMapper: {
		"^.+\\.module\\.(css|scss)$": "identity-obj-proxy",
		"\\.(css|scss)$": "identity-obj-proxy",
	},
	transform: {
		"^.+\\.(ts|tsx)$": "ts-jest",
	},
	testPathIgnorePatterns: ["/node_modules/", "/dist/"],
	globals: {
		"ts-jest": {
			tsconfig: "<rootDir>/tsconfig.app.json",
		},
	},
};

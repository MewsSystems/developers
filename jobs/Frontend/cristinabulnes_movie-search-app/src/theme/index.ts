export const theme = {
	palette: {
		mode: "light",
		common: {
			black: "#000",
			white: "#fff",
		},
		primary: {
			main: "#007bff",
			light: "#007bff",
			dark: "#0056b3",
			contrastText: "#fff",
		},
		secondary: {
			main: "#6c757d",
			light: "#6c757d",
			dark: "#343a40",
			contrastText: "#fff",
		},
		error: {
			main: "#dc3545",
			light: "#f8d7da",
			dark: "#a71d2a",
			contrastText: "#fff",
		},
		warning: {
			main: "#ffc107",
			light: "#ffc107",
			dark: "#856404",
			contrastText: "rgba(0, 0, 0, 0.87)",
		},
		info: {
			main: "#17a2b8",
			light: "#17a2b8",
			dark: "#0c5460",
			contrastText: "#fff",
		},
		success: {
			main: "#28a745",
			light: "#28a745",
			dark: "#1e7e34",
			contrastText: "#fff",
		},
		grey: {
			50: "#f8f9fa",
			100: "#f1f3f5",
			200: "#e9ecef",
			300: "#dee2e6",
			400: "#ced4da",
			500: "#adb5bd",
			600: "#6c757d",
			700: "#495057",
			800: "#343a40",
			900: "#212529",
		},
		text: {
			primary: "rgba(0, 0, 0, 0.87)",
			secondary: "rgba(0, 0, 0, 0.54)",
			disabled: "rgba(0, 0, 0, 0.38)",
		},
		background: {
			default: "#f8f9fa",
			dark: "#242424",
			paper: "#fff",
		},
		divider: "rgba(0, 0, 0, 0.12)",
	},

	spacing: (factor: number) => `${factor * 8}px`,
	shadows: {
		default: "0 2px 8px rgba(0, 0, 0, 0.1)",
		hover: "0 4px 12px rgba(0, 0, 0, 0.2)",
		focus: "0 0 0 4px rgba(0, 123, 255, 0.5)",
	},
	size: {
		small: "8px",
		regular: "16px",
		large: "24px",
	},
	typography: {
		fontFamily: `"Roboto", "Helvetica", "Arial", sans-serif`,
		fontSize: {
			small: "14px",
			medium: "16px",
			large: "18px",
			xLarge: "24px",
		},
		fontWeight: {
			light: "300",
			regular: "400",
			medium: "500",
			bold: "700",
		},
		h1: {
			fontFamily: `"Roboto", "Helvetica", "Arial", sans-serif`,
			fontWeight: "300",
			fontSize: "6rem",
			lineHeight: 1.167,
		},
		h2: {
			fontFamily: `"Roboto", "Helvetica", "Arial", sans-serif`,
			fontWeight: "300",
			fontSize: "3.75rem",
			lineHeight: 1.2,
		},
		h3: {
			fontFamily: `"Roboto", "Helvetica", "Arial", sans-serif`,
			fontWeight: "400",
			fontSize: "3rem",
			lineHeight: 1.167,
		},
		h4: {
			fontFamily: `"Roboto", "Helvetica", "Arial", sans-serif`,
			fontWeight: "400",
			fontSize: "2.125rem",
			lineHeight: 1.235,
		},
		h5: {
			fontFamily: `"Roboto", "Helvetica", "Arial", sans-serif`,
			fontWeight: "400",
			fontSize: "1.5rem",
			lineHeight: 1.334,
		},
		h6: {
			fontFamily: `"Roboto", "Helvetica", "Arial", sans-serif`,
			fontWeight: "700",
			fontSize: "1.25rem",
			lineHeight: 1.6,
		},
		subtitle1: {
			fontFamily: `"Roboto", "Helvetica", "Arial", sans-serif`,
			fontWeight: "400",
			fontSize: "1rem",
			lineHeight: 1.75,
		},
		subtitle2: {
			fontFamily: `"Roboto", "Helvetica", "Arial", sans-serif`,
			fontWeight: "500",
			fontSize: "0.875rem",
			lineHeight: 1.57,
		},
		body1: {
			fontFamily: `"Roboto", "Helvetica", "Arial", sans-serif`,
			fontWeight: "400",
			fontSize: "1rem",
			lineHeight: 1.5,
		},
		body2: {
			fontFamily: `"Roboto", "Helvetica", "Arial", sans-serif`,
			fontWeight: "400",
			fontSize: "0.875rem",
			lineHeight: 1.43,
		},
		button: {
			fontFamily: `"Roboto", "Helvetica", "Arial", sans-serif`,
			fontWeight: "500",
			fontSize: "0.875rem",
			lineHeight: 1.75,
			textTransform: "uppercase",
		},
	},
	borderRadius: {
		small: "4px",
		regular: "8px",
		large: "16px",
	},
};

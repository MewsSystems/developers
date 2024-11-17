import { Theme } from './types';


export const darkTheme: Theme = {
    name: 'dark',
    colors: {
      primary: '#4db6ac',
      secondary: '#80cbc4',
      background: '#121212',
      surface: '#1e1e1e',
      text: {
        primary: '#ffffff',
        secondary: '#b3b3b3',
      },
      button: {
        background: '#4db6ac',
        hover: '#80cbc4',
        text: '#000000',
        inverseText: '#ffffff',
      },
      card: {
        background: '#1e1e1e',
        shadow: '0 2px 8px rgba(0, 0, 0, 0.2)',
        hoverShadow: '0 4px 16px rgba(0, 0, 0, 0.4)',
      },
      rating: {
        background: 'rgba(0, 0, 0, 0.75)',
        text: '#ffffff',
        highRating: {
          border: '#ffd700',
          text: '#ffd700',
        },
      },
      input: {
        background: '#2d2d2d',
        border: '#404040',
        focusBorder: '#4db6ac',
      },
    },
    spacing: {
      xs: '4px',
      sm: '8px',
      md: '16px',
      lg: '24px',
      xl: '32px',
    },
    borderRadius: {
      sm: '4px',
      md: '8px',
      lg: '16px',
    },
  };
  
  export const lightTheme: Theme = {
    name: 'light',
    colors: {
      primary: '#26a69a',
      secondary: '#4db6ac',
      background: '#f5f5f5',
      surface: '#ffffff',
      text: {
        primary: '#333333',
        secondary: '#666666',
      },
      button: {
        background: '#26a69a',
        hover: '#4db6ac',
        text: '#ffffff',
        inverseText: '#000000',
      },
      card: {
        background: '#ffffff',
        shadow: '0 2px 8px rgba(0, 0, 0, 0.1)',
        hoverShadow: '0 4px 16px rgba(0, 0, 0, 0.2)',
      },
      rating: {
        background: 'rgba(0, 0, 0, 0.75)',
        text: '#ffffff',
        highRating: {
          border: '#ffd700',
          text: '#ffd700',
        },
      },
      input: {
        background: '#ffffff',
        border: '#e0e0e0',
        focusBorder: '#26a69a',
      },
    },
    spacing: {
      xs: '4px',
      sm: '8px',
      md: '16px',
      lg: '24px',
      xl: '32px',
    },
    borderRadius: {
      sm: '4px',
      md: '8px',
      lg: '16px',
    },
  };

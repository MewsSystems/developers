import { cleanup, render } from '@testing-library/react';
import { FC, PropsWithChildren } from 'react';
import { Provider } from 'react-redux';
import { MemoryRouter } from 'react-router-dom';
import { ThemeProvider } from 'styled-components';
import { afterEach, vi } from 'vitest';
import { store } from '../core/store/store';
import theme from '../shared/styles/constants/theme';

afterEach(() => {
    cleanup();
});

const AllProviders: FC<PropsWithChildren> = ({ children }) => {
    return (
        <Provider store={store}>
            <ThemeProvider theme={theme}>
                <MemoryRouter>{children}</MemoryRouter>
            </ThemeProvider>
        </Provider>
    );
};

function customRender(ui: React.ReactElement, options = {}) {
    return render(ui, {
        wrapper: AllProviders,
        ...options,
    });
}

export const mockIntersectionObserver = () => {
    window.IntersectionObserver = vi.fn().mockReturnValue({
        observe: vi.fn(),
        unobserve: vi.fn(),
        disconnect: vi.fn(),
    });
};

export const mockResizeObserver = () => {
    window.ResizeObserver = vi.fn().mockReturnValue({
        observe: vi.fn(),
        unobserve: vi.fn(),
        disconnect: vi.fn(),
    });
};

export * from '@testing-library/react';
export { customRender as render };

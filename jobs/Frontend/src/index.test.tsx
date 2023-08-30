import { render, screen } from "@testing-library/react";
import App from "App";
import { ErrorBoundary } from "react-error-boundary";
import { Provider } from "react-redux";
import { store } from "state/store";

describe('Root component', () => {
    it('handles errors without crashing', () => {
        const fallback = () => (<p>A problem was encountered</p>);
        const ErrorThrower = () => { throw new Error('ERROR'); }

        render(
            <ErrorBoundary FallbackComponent={fallback}>
                <Provider store={store}>
                    <App />
                    <ErrorThrower />
                </Provider>
            </ErrorBoundary>
        );
        expect(screen.getByText('A problem was encountered')).toBeInTheDocument();
    });
});
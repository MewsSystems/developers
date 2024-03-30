import { cleanup, render } from "@testing-library/react";
import { afterEach } from "vitest";
import { Provider } from "react-redux";
import { RootState, setupMockStore } from "../redux/store";
import { BrowserRouter } from "react-router-dom";

afterEach(() => {
  cleanup();
});

function customRender(
  ui: React.ReactElement,
  options: { state?: RootState; withRouter?: boolean } = {
    state: undefined,
    withRouter: false,
  }
) {
  const store = setupMockStore(options.state);

  return render(ui, {
    // wrap provider(s) here if needed
    wrapper: ({ children }) =>
      options.withRouter ? (
        <BrowserRouter>
          <Provider store={store}>{children}</Provider>
        </BrowserRouter>
      ) : (
        <Provider store={store}>{children}</Provider>
      ),
    ...options,
  });
}

// eslint-disable-next-line react-refresh/only-export-components
export * from "@testing-library/react";
export { default as userEvent } from "@testing-library/user-event";
// override render export
export { customRender as render };

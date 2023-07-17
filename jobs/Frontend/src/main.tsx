import { StrictMode } from "react";
import ReactDOM from "react-dom/client";

import App from "./App.tsx";
import { Provider } from "react-redux";
import { store } from "./store.ts";

ReactDOM.createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <Provider store={store}>
      <App />
    </Provider>
  </StrictMode>
);

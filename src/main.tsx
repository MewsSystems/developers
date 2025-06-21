import { StrictMode } from "react"
import { createRoot } from "react-dom/client"
import { BrowserRouter } from "react-router"
import App from "./App.tsx"

const isMockingEnabled = import.meta.env.VITE_ENABLE_MSW === "true"

async function enableMocking() {
  if (isMockingEnabled) {
    const { worker } = await import("./test/mocks/browser")

    await worker.start({
      onUnhandledRequest: "bypass",
    })
  }
}

enableMocking().then(() => {
  createRoot(document.getElementById("root")!).render(
    <StrictMode>
      <BrowserRouter>
        <App />
      </BrowserRouter>
    </StrictMode>
  )
})

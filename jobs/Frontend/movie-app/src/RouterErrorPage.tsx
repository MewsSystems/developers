import { useRouteError } from "react-router-dom";
import Header from "./components/Header";
import Error from "./components/Error";
function RouterErrorPage() {
  interface RouteError {
    data: string;
    error: {
      columnNumber: number;
      fileName: string;
      lineNumber: number;
      message: string;
      stack: string;
    };
    internal: boolean;
    status: number;
    statusText: string;
  }

  const error = useRouteError() as RouteError;
  return (
    <div id="error-page">
      <Header />
      <Error errorMessage={error?.statusText || error?.error.message} />
    </div>
  );
}
export default RouterErrorPage;

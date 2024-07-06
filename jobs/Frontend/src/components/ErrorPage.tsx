import React from "react";

/**
 * The ErrorPage component used to display a general error message when an error occurs while rendering a page.
 */
const ErrorPage = () => {
  return (
    <div className="h-screen w-screen flex justify-center items-center">
      <div className="flex flex-row items-center text-red-500">
        <p className="text-lg">
          An error occurred while rendering the page. Try again later.
        </p>
      </div>
    </div>
  );
};

export default ErrorPage;

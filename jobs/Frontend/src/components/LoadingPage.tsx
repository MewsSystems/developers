import React from "react";
import { Loader2 } from "lucide-react";

/**
 * The LoadingPage component used to display a loading spinner while a route is loading.
 */
const LoadingPage = () => {
  return (
    <div className="h-screen w-screen flex justify-center items-center">
      <div className="flex flex-row items-center">
        <Loader2 className="mr-2 h-4 w-4 animate-spin" />
        <p className="text-lg">Loading...</p>
      </div>
    </div>
  );
};

export default LoadingPage;

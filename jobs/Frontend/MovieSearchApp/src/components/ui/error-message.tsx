import { Frown } from "lucide-react";

export const ErrorMessage = () => {
  return (
    <div className="h-full flex justify-center items-center">
      <Frown className="mr-2" />
      Unexpected error.
    </div>
  );
};

import { Loader2 } from "lucide-react";

type LoadingIndicatorProps = {
  text: string;
};

export const LoadingIndicator: React.FC<LoadingIndicatorProps> = ({ text }) => {
  return (
    <div className="flex justify-center items-center">
      <Loader2 className="animate-spin mr-2" />
      {text}
    </div>
  );
};

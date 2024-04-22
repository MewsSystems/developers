import { LoadingSpinner } from "@/design-system/components/ui/spinner";
import { cn } from "@/design-system/lib/utils";
import { BaseComponentProps } from "@/types";

export function LoadingScreen({ className, ...props }: BaseComponentProps) {
  return (
    <div
      className={cn(className, "flex items-center justify-center")}
      {...props}
    >
      <LoadingSpinner className="h-20 w-20" />
    </div>
  );
}

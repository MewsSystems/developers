import { Skeleton } from "@/app/components/ui"
import { cn } from "@/app/lib/utils"

export const MovieCardSkeleton = () => {
  return (
    <Skeleton className={cn("aspect-[2/3] bg-gray-300 w-full h-full flex")} />
  )
}

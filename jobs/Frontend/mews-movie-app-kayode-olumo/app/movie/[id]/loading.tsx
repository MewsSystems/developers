import { Skeleton } from "@/components/ui/skeleton"
import { Container } from "@/components/Container"

export default function MovieLoading() {
  return (
    <div className="min-h-screen bg-background">
      <header className="border-b bg-background sticky top-0 z-10 backdrop-blur-sm bg-background/95">
        <Container className="py-4">
          <Skeleton className="h-10 w-32" />
        </Container>
      </header>

      <main>
        <Container className="py-6 sm:py-8 lg:py-12">
          <div className="grid lg:grid-cols-[350px_1fr] gap-6 sm:gap-8 lg:gap-12">
          <div className="mx-auto lg:mx-0 w-full max-w-sm lg:max-w-none">
            <Skeleton className="aspect-[2/3] w-full rounded-xl" />
          </div>

          <div className="space-y-6 sm:space-y-8">
            <div>
              <Skeleton className="h-12 w-3/4 mb-3" />
              <Skeleton className="h-6 w-1/2" />
            </div>

            <div className="flex flex-wrap gap-4 sm:gap-6">
              <Skeleton className="h-6 w-20" />
              <Skeleton className="h-6 w-16" />
              <Skeleton className="h-6 w-12" />
            </div>

            <div>
              <Skeleton className="h-6 w-20 mb-3" />
              <div className="flex flex-wrap gap-2">
                <Skeleton className="h-8 w-16 rounded-full" />
                <Skeleton className="h-8 w-20 rounded-full" />
                <Skeleton className="h-8 w-14 rounded-full" />
              </div>
            </div>

            <div>
              <Skeleton className="h-6 w-24 mb-3" />
              <div className="space-y-2">
                <Skeleton className="h-4 w-full" />
                <Skeleton className="h-4 w-full" />
                <Skeleton className="h-4 w-3/4" />
              </div>
            </div>

            <div className="grid sm:grid-cols-2 gap-4">
              <Skeleton className="h-32 w-full rounded-xl" />
              <Skeleton className="h-32 w-full rounded-xl" />
            </div>
          </div>
          </div>
        </Container>
      </main>
    </div>
  )
}

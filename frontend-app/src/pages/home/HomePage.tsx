import { Movies } from '@/widgets/movies/ui';

/**
 * Home page component with movie search functionality
 */
export function HomePage() {

  return (
    <div className="container mx-auto px-4 py-8">
      <Movies />
    </div>
  );
}
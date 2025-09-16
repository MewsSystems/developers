import { createFileRoute } from '@tanstack/react-router'
import {Index} from '@/pages/movies-list/ui';

export const Route = createFileRoute('/_auth/movies')({
  component: Index,
})

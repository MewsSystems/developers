import { Home } from '@/pages/Home'
import { createFileRoute } from '@tanstack/react-router'
import { z } from 'zod'

const movieSearchSchema = z.object({
  page: z.number().default(1),
  search: z.string().default(''),
})

export const Route = createFileRoute('/')({
  component: Home,
  validateSearch: movieSearchSchema,
})

import { getConfiguration } from '@/entities/configuration/api/configurationApi'
import {
    useQuery,
} from '@tanstack/react-query'

export default function useQueryConfiguration() {
    return useQuery({ queryKey: ['configuration'], queryFn: () => getConfiguration() })
}